using Autofac;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PT.BuildingBlocks.Abstractions;
using PT.Data;
using PT.Infrastructure.Abstractions;
using PT.Infrastructure.Mappings;
using System;

namespace PT.Infrastructure.DependencyResolution
{
    public static class DependencyResolver
	{
		public static void Initialize(IServiceCollection services, IConfiguration config)
		{
			Settings.Default = new Settings(config);
			services.AddSingleton<ISettings>(Settings.Default);
			services.AddDatabaseServices(Settings.Default);
			services.AddMappingServices();
			services.AddHttpContextAccessor();

		}

		/// <summary>
		/// Adds repositories to DI service collection.
		/// </summary>
		/// <param name="services">DI services collection.</param>
		/// <param name="configuration">App configuration</param>
		private static void AddDatabaseServices(this IServiceCollection services, ISettings settings)
		{
			services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(settings.ConnectionString));
		}

		private static void AddMappingServices(this IServiceCollection services)
		{
			var mapperConfigExpression = EntityConverter.GetDefaultMapperConfiguration(new string[]
			{
				typeof(ClientMapping).Assembly.GetName().Name
			});

			services.AddSingleton(mapperConfigExpression);
			services.AddScoped<IEntityConverter>((ctx) =>
			{
				var scope = ctx.GetService<ILifetimeScope>();
				return new EntityConverter(ctx.GetService<Action<IMapperConfigurationExpression>>(), scope.Resolve);
			});
		}
	}
}
