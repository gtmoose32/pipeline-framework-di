using LightInject;
using PipelineFramework.Abstractions;
using System;
using LightInject.Interception;
using PipelineFramework.LightInject.Logging;

namespace PipelineFramework.LightInject
{
    public class PipelineComponentResolver : ILightInjectPipelineComponentResolver
    {
        private readonly IServiceFactory _serviceFactory;

        public PipelineComponentResolver(IServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory ?? throw new ArgumentNullException(nameof(serviceFactory));
        }

        public T GetInstance<T>(string name) where T : IPipelineComponent
        {
            return _serviceFactory.GetInstance<T>(name);
        }

        public void AddAsyncInterceptor(IInterceptor interceptor)
        {
            if (_serviceFactory is IServiceRegistry serviceRegistry)
            {
                serviceRegistry.RegisterAsyncInterceptor(interceptor.GetType());
                serviceRegistry.AddAsyncPipelineComponentInterception();
            }
        }

        public void AddInterceptor(IInterceptor interceptor)
        {
            throw new NotImplementedException();
        }
    }
}
