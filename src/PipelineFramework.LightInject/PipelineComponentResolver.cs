using LightInject;
using PipelineFramework.Abstractions;
using PipelineFramework.Exceptions;
using System;

namespace PipelineFramework.LightInject
{
    public class PipelineComponentResolver : IPipelineComponentResolver
    {
        private readonly IServiceFactory _serviceFactory;

        public PipelineComponentResolver(IServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory ?? throw new ArgumentNullException(nameof(serviceFactory));
        }

        public T GetInstance<T>(string name) where T : class, IPipelineComponent
            => _serviceFactory.TryGetInstance<T>(name) ?? 
               throw new PipelineComponentNotFoundException($"Pipeline component named, '{name}' could not be found.");
    }
}
