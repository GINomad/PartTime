using Autofac;
using AutofacContrib.SolrNet;
using AutoMapper;
using EventBus;
using EventBus.Abstractions;
using EventBus.EventBusRabbitMQ;
using Lease.Core.Template.Domain.Aggregates;
using Lease.Core.Template.Handlers.Command;
using Lease.Core.Template.Handlers.Query;
using Lease.Core.Template.Queries;
using Lease.Core.Template.Services.Interfaces;
using Lease.Core.Template.ViewModel;
using Lease.Core.TemplateDocument.Domain.Aggregates;
using Lease.Core.TemplateDocument.Handlers.Command;
using Lease.Core.TemplateDocument.Handlers.Query;
using Lease.Core.TemplateDocument.Services;
using Lease.Core.TemplateDocument.ViewModel;
using Lease.Infrastructure.Abstractions;
using Lease.Infrastructure.Configuration;
using Lease.Infrastructure.Data;
using Lease.Infrastructure.Data.QueryBuilder;
using Lease.Infrastructure.Mappings;
using Lease.Infrastructure.Services.File;
using Lease.Infrastructure.Services.HelloSign;
using Lease.Infrastructure.Services.HelloSign.Helpers.Adapters;
using Lease.Infrastructure.Services.HelloSign.Helpers.Extensions;
using Lease.Infrastructure.Services.HelloSign.Helpers.Formatting;
using Lease.Infrastructure.Services.HelloSign.Interfaces;
using Lease.Infrastructure.Services.Logger;
using Lease.Infrastructure.Services.Logger.Interfaces;
using Merlin.BuildingBlocks;
using Merlin.BuildingBlocks.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using SolrNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Lease.Infrastructure.DependencyResolution
{
    public static class DependencyResolver
	{
		public static void Initialize(IServiceCollection services, IConfiguration config, ContainerBuilder builder)
        {
			Settings.Default = new Settings(config);
			services.AddSingleton<ISettings>(Settings.Default);
			services.AddDatabaseServices(Settings.Default);
			AddCommandHandlers(builder);
			services.AddMappingServices();
			services.AddCustomServices();
			services.AddEventPublishers();
			AddQueryHandlers(builder);
			RegisterSolr(Settings.Default, builder);
			ConfigureEventBus(services, config);
		}
		/// <summary>
		/// Adds domain models to DI service collection.
		/// </summary>
		/// <param name="services">DI services collection.</param>
		public static void AddCommandHandlers(ContainerBuilder builder)
		{
			builder.RegisterType<CreateTemplateCommandHandler>().AsImplementedInterfaces();
			builder.RegisterType<UpdateTemplateCommandHandler>().AsImplementedInterfaces();
			builder.RegisterType<CreateTemplateDocumentCommandHandler>().AsImplementedInterfaces();
			builder.RegisterType<UpdateTemplateDocumentCommandHandler>().AsImplementedInterfaces();
			builder.RegisterType<DeleteTemplateCommandHandler>().AsImplementedInterfaces();
			builder.RegisterType<SendSigningRequestCommandHandler>().AsImplementedInterfaces();
		}

		public static void RegisterSolr(ISettings settings, ContainerBuilder builder)
        {
			var serverUrl = settings.SolrRootUrl;
            var module = new SolrNetModule(new List<ISolrServer>
            {
                new SolrServer("template", serverUrl + settings.SolrTemplateListCollection, typeof(TemplateViewModel).AssemblyQualifiedName),
				new SolrServer("template-field", serverUrl + settings.SolrTemplateFieldCollection, typeof(FieldViewModel).AssemblyQualifiedName),
				new SolrServer("template-assignee", serverUrl + settings.SolrTemplateAssigneeCollection, typeof(AssigneeViewModel).AssemblyQualifiedName),
				new SolrServer("template-document", serverUrl + settings.SolrTemplateDocumentCollection, typeof(TemplateDocumentViewModel).AssemblyQualifiedName)
			})
            {
                Mapper = SolrMapping.Get()
            };
            builder.RegisterModule(module);


			builder.RegisterType<TemplateQueryRepository>().As<ITemplateQueryRepository>();
			builder.RegisterType<TemplateDocumentQueryRepository>().As<ITemplateDocumentQueryRepository>();
			builder.RegisterType<SolrQueryBuilder>().As<IQueryBuilder<SolrMultipleCriteriaQuery>>();
		}

		public static void AddQueryHandlers(ContainerBuilder builder)
		{
			builder.RegisterType<TemplateListQueryHandler>().AsImplementedInterfaces();
			builder.RegisterType<SingleTemplateQueryHandler>().AsImplementedInterfaces();
			builder.RegisterType<SingleDocumentQueryHandler>().AsImplementedInterfaces();
			builder.RegisterType<CheckTemplateExistenceQueryHandler>().AsImplementedInterfaces();
			builder.RegisterType<TemplateShortListQueryHandler>().AsImplementedInterfaces();
		}

		public static void AddEventPublishers(this IServiceCollection services)
        {
			services.AddScoped<IDomainEventPublisher, DomainEventPublisher>();
        }

		public static void AddCustomServices(this IServiceCollection services)
		{
			services.AddScoped<IFontStyleHelper, XFontStyleHelper>();
			services.AddScoped<IFontHelper, XFontHelper>();
			services.AddScoped<IFieldAdapter, FieldAdapter>();
			services.AddScoped<ITemplateAdapter, TemplateAdapter>();
			services.AddScoped<IHelloSignService>((ctx) => {
				var settings = ctx.GetService<ISettings>();
				var api_key = settings.HelloSignApiKey;
				var client_key = settings.HelloSignClientKey;
				var helloSignTestMode = settings.HelloSignTestMode;
				var rootFolder = settings.UploadedFilesPath;
				return new HelloSignService(
					new Services.HelloSign.Models.HelloSignConfiguration(api_key, client_key, rootFolder, helloSignTestMode),
					ctx.GetRequiredService<IFontHelper>(),
					ctx.GetRequiredService<ITemplateAdapter>(),
					ctx.GetRequiredService<IEntityConverter>(),
					ctx.GetRequiredService<ITemplateDocumentQueryRepository>());
			});
			FontResolverExtension.RegisterResolver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));

			services.AddScoped<IFileService>((ctx) =>
			{
				var settings = ctx.GetService<ISettings>();
				var rootFolder = settings.UploadedFilesPath;
				return new FileService(rootFolder, ctx.GetService<ILogService<FileService>>());
			});
		}

		/// <summary>
		/// Adds repositories to DI service collection.
		/// </summary>
		/// <param name="services">DI services collection.</param>
		/// <param name="configuration">App configuration</param>
		public static void AddDatabaseServices(this IServiceCollection services, ISettings settings)
		{
			services.Configure<MongoDbSettings>(options =>
			{
				options.ConnectionString
					= settings.TemplateStorageServer;
				options.Database
					= settings.TemplateStorageDB;
			});

			services.AddSingleton<IMongoDbContext, MongoDbContext>();
			MongoDbMapping.Map();

			services.AddScoped<ITemplateRepository, TemplateRepository>();
			services.AddScoped<ITemplateDocumentRepository, TemplateDocumentRepository>();
		}

		public static void AddMappingServices(this IServiceCollection services)
		{
			var mapperConfigExpression = EntityConverter.GetDefaultMapperConfiguration(new string[]
			{
				typeof(CommonMapping).Assembly.GetName().Name
			});

			services.AddSingleton(mapperConfigExpression);
			services.AddScoped<IEntityConverter>((ctx) =>
			{
				var scope = ctx.GetService<ILifetimeScope>();
				return new EntityConverter(ctx.GetService<Action<IMapperConfigurationExpression>>(), scope.Resolve);
			});
		}

		public static void ConfigureEventBus(IServiceCollection services, IConfiguration configuration)
		{
			var connection = new ConnectionConfiguration(configuration.GetSection("Settings"));
			services.AddSingleton<IConnectionConfiguration>(connection);
			services.AddSingleton<IRabbitMQConnection>(sp =>
			{
				var logger = sp.GetRequiredService<ILogger<RabbitMQConnection>>();

				var factory = new ConnectionFactory()
				{
					HostName = connection.Host,
					UserName = connection.UserName,
					Password = connection.Password,
					VirtualHost = connection.VirtualHost,
					AutomaticRecoveryEnabled = true,
					RequestedHeartbeat = 3,
				};

				return new RabbitMQConnection(factory, logger, connection.RetryCount);
			}).RegisterEventBus(connection);
		}

		private static IServiceCollection RegisterEventBus(this IServiceCollection services, ConnectionConfiguration connection)
		{

			services.AddSingleton<IEventBus, EventBus.EventBusRabbitMQ.EventBus>(sp =>
			{
				var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQConnection>();
				var logger = sp.GetRequiredService<ILogger<EventBus.EventBusRabbitMQ.EventBus>>();
				var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();
				return new EventBus.EventBusRabbitMQ.EventBus(rabbitMQPersistentConnection, logger, sp, eventBusSubcriptionsManager);
			});
			services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
			return services;
		}
	}
}
