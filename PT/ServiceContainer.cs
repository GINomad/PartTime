using Autofac;
using Lease.Core.Template.Domain.Events;
using Lease.Core.Template.Handlers.Event;
using Lease.Core.TemplateDocument.Domain.Events;
using Lease.Core.TemplateDocument.Handlers.Event;
using MediatR;
using Merlin.BuildingBlocks;
using Merlin.BuildingBlocks.Abstractions;

namespace Lease.Infrastructure.DependencyResolution
{
    public class ServiceContainer: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            RegisterMediator(builder);
        }

        private void RegisterMediator(ContainerBuilder builder)
        {
            builder.RegisterType<Mediator>().As<IMediator>().InstancePerLifetimeScope();
            builder.Register<ServiceFactory>(context =>
            {
                var c = context.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });

            builder.RegisterGeneric(typeof(DomainEventProcessor<>)).AsImplementedInterfaces();
            builder.RegisterType<CommandPublisher>().As<ICommandPublisher>();
            builder.RegisterType<QueryPublisher>().As<IQueryPublisher>();
            builder.RegisterType<TemplateCreatedEventHandler>().As<IDomainEventHandler<TemplateCreated_Event>>().InstancePerLifetimeScope();
            builder.RegisterType<TemplateUpdatedEventHandler>().As<IDomainEventHandler<TemplateUpdated_Event>>().InstancePerLifetimeScope();
            builder.RegisterType<TemplateDocumentCreatedEventHandler>().As<IDomainEventHandler<TemplateDocumentCreated_Event>>().InstancePerLifetimeScope();
            builder.RegisterType<TemplateDocumentUpdatedEventHandler>().As<IDomainEventHandler<TemplateDocumentUpdated_Event>>().InstancePerLifetimeScope();
            builder.RegisterType<TemplateDeletedEventHandler>().As<IDomainEventHandler<TemplateDeleted_Event>>().InstancePerLifetimeScope();
        }
    }
}
