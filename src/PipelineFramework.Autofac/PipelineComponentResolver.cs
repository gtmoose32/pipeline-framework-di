using Autofac;
using PipelineFramework.Abstractions;
using PipelineFramework.Exceptions;
using System;

namespace PipelineFramework.Autofac
{
    public class PipelineComponentResolver : IPipelineComponentResolver
    {
        private readonly IComponentContext _componentContext;

        public PipelineComponentResolver(IComponentContext componentContext)
        {
            _componentContext = componentContext ?? throw new ArgumentNullException(nameof(componentContext));
        }

        public T GetInstance<T>(string name) where T : class, IPipelineComponent
        {

            if (_componentContext.TryResolveNamed(name, typeof(T), out var instance)) return (T)instance;
        
            throw new PipelineComponentNotFoundException($"Pipeline component named, '{name}' could not be found.");
        }
    }
}
