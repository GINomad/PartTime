using Autofac;
using Autofac.Extensions.DependencyInjection;
using Lease.Infrastructure.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Lease.Infrastructure.DependencyResolution
{
    public static class ServiceProvider
    {
        private static AutofacServiceProviderWrapper _provider;

        public static IServiceProviderEx Initialize(IServiceCollection services, IConfiguration config, params string[] servicesContainerAssemblyNames)
        {
            _provider = new AutofacServiceProviderWrapper(services, config, servicesContainerAssemblyNames);
            return _provider;
        }

        public static IServiceProviderEx Current
        {
            get
            {
                if (_provider == null)
                {
                    throw new InvalidOperationException("To use service provider call Initialize method first");
                }

                return _provider;
            }
        }
    }

    internal class AutofacServiceProviderWrapper : IServiceProviderEx
    {
        private readonly AutofacServiceProvider _autofac;

        public AutofacServiceProviderWrapper(IServiceCollection services, IConfiguration config, params string[] servicesContainerAssemblyNames)
        {
            var servicesContainerBuilder = new ContainerBuilder();
            DependencyResolver.Initialize(services, config, servicesContainerBuilder);
            var servicesContainerAssemblies = servicesContainerAssemblyNames.Select(assemblyName => Assembly.Load(assemblyName)).ToArray();
            servicesContainerBuilder.RegisterAssemblyModules(servicesContainerAssemblies);
            if (services != null)
            {
                servicesContainerBuilder.Populate(services);
            }

            var container = servicesContainerBuilder.Build();
            
            _autofac = new AutofacServiceProvider(container);
        }

        public object GetService(Type serviceType)
        {
            var perRequestDIContainer = _autofac.GetService<IHttpContextAccessor>()?.HttpContext?.RequestServices;
            return perRequestDIContainer != null
                ? perRequestDIContainer.GetService(serviceType)
                : _autofac.GetService(serviceType);
        }

        public TService GetService<TService>()
        {
            return (TService)this.GetService(typeof(TService));
        }

        public object GetServices(Type serviceType)
        {
            var perRequestDIContainer = _autofac.GetService<IHttpContextAccessor>()?.HttpContext?.RequestServices;
            return perRequestDIContainer != null
                ? perRequestDIContainer.GetServices(serviceType)
                : _autofac.GetServices(serviceType);
        }

        public IEnumerable<TService> GetServices<TService>()
        {
            return (IEnumerable<TService>)this.GetServices(typeof(TService));
        }
    }
}
