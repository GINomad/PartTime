using NLog;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Identity;
using PT.Identity.API.Extensions;
using PT.Identity.Infrastructure.Database.Users;
using PT.Identity.Infrastructure.Extensions;

namespace PT.Identity.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

            builder.Services
                .ConfigureResponseCaching()
                .ConfigureCors(builder.Configuration)
                .ConfigureLoggerService()
                .ConfigureSqlContext(builder.Configuration);

            builder.Services
                .ConfigureIdentity()
                .ConfigureApplicationCookies()
                .ConfigureAntiforgeryProtection()
                .ConfigureControllers();

            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.ConfigureSwagger();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseResponseCaching();

            app.UseCors("ClientUi");

            app.Use(async (context, next) =>
            {
                var isUnsafeApiRequest =
                    context.Request.Path.StartsWithSegments("/api")
                    && !context.Request.Path.StartsWithSegments("/api/antiforgery")
                    && (HttpMethods.IsPost(context.Request.Method)
                        || HttpMethods.IsPut(context.Request.Method)
                        || HttpMethods.IsPatch(context.Request.Method)
                        || HttpMethods.IsDelete(context.Request.Method));

                if (isUnsafeApiRequest)
                {
                    var antiforgery = context.RequestServices.GetRequiredService<IAntiforgery>();
                    await antiforgery.ValidateRequestAsync(context);
                }

                await next();
            });

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();
            app.MapGet("/api/antiforgery/token", (IAntiforgery antiforgery, HttpContext context) =>
            {
                var tokens = antiforgery.GetAndStoreTokens(context);

                return Results.Ok(new { token = tokens.RequestToken });
            });

            app.MapGroup("/api/account").MapIdentityApi<User>();
            app.MapPost("/api/account/logout", async (SignInManager<User> signInManager) =>
            {
                await signInManager.SignOutAsync();
                return Results.NoContent();
            }).RequireAuthorization();

            app.Run();
        }
    }
}
