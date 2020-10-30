using Autofac;
using PT.BuildingBlocks.Abstractions;
using PT.Core.Client.Services;
using PT.Core.Client.Services.Abstractions;
using PT.Identity.Abstractions;
using PT.Identity.Services;

namespace PT.Infrastructure.DependencyResolution
{
    public class AutofacModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ClientService>().As<IClientService>();
            builder.RegisterType<ApplicationUserService>().As<IApplicationUserService>();
            builder.RegisterType<IdentityService>().As<IIdentityService>();
            builder.RegisterGeneric(typeof(Repository<>))
                .As(typeof(IRepository<>))
                .InstancePerLifetimeScope();
        }
    }
}
