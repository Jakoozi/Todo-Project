using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApi
{
    public class ContainerJobActivator : JobActivator
    {
        private IServiceScopeFactory _container;

        public ContainerJobActivator(IServiceScopeFactory container)
        {
            _container = container;
        }

        public override object ActivateJob(Type type) => _container.CreateScope().ServiceProvider.GetRequiredService(type);
    }
}
