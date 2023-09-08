using NLog;
using PT.Identity.API.Extensions;
using PT.Identity.Infrastructure.Abstractions.Logging;
using PT.Identity.Infrastructure.Logging;
using PT.Identity.Infrastructure.Extensions;

namespace PT.Identity.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            ILoggerService logger = new LoggerService();

            builder.Services
                .ConfigureResponseCaching()
                .ConfigureLoggerService()
                .ConfigureSqlContext(builder.Configuration);

            builder.Services.AddAuthentication();
            builder.Services
                .ConfigureIdentity()
                .ConfigureJWT(builder.Configuration)
                .ConfigureControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.ConfigureSwagger();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseResponseCaching();

            app.UseAuthentication();
            app.UseAuthorization(); ;


            app.MapControllers();

            app.Run();
        }
    }
}