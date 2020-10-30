using System;
using System.Collections.Generic;

namespace PT.Infrastructure.Abstractions
{
    public interface IServiceProviderEx: IServiceProvider
    {
        TService GetService<TService>();
        new object GetService(Type serviceType);
        IEnumerable<TService> GetServices<TService>();
    }
}
