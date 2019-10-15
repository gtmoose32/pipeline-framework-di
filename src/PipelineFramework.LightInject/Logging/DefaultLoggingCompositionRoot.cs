using LightInject;
using LightInject.Interception;
using PipelineFramework.LightInject.Logging.Interceptors;
using System;

namespace PipelineFramework.LightInject.Logging
{
    public class DefaultLoggingCompositionRoot : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            serviceRegistry.RegisterInterceptor(GetPipelineComponentInterceptorType());
            serviceRegistry.RegisterAsyncInterceptor(GetAsyncPipelineComponentInterceptorType());

            serviceRegistry.AddAsyncPipelineComponentInterception();
            serviceRegistry.AddPipelineComponentInterception();
        }

        protected virtual Type GetPipelineComponentInterceptorType() => typeof(PipelineComponentInterceptor);

        protected virtual Type GetAsyncPipelineComponentInterceptorType() => typeof(AsyncPipelineComponentInterceptor);

        #region Private Methods
        private void RegisterInterceptor(IServiceRegistry serviceRegistry)
            => serviceRegistry.Register(typeof(IInterceptor), GetPipelineComponentInterceptorType(), ServiceNames.InterceptorServiceName);

        private void RegisterAsyncInterceptor(IServiceRegistry serviceRegistry)
        {
            var asyncInterceptorType = typeof(AsyncInterceptor);
            var interceptorType = GetAsyncPipelineComponentInterceptorType();
            if (!(asyncInterceptorType.IsAssignableFrom(interceptorType)))
                throw new InvalidOperationException(
                    $"AsyncPipelineComponentTypeInterceptor type must be a type that inherits from '{asyncInterceptorType.FullName}'.");

            serviceRegistry.Register(typeof(IInterceptor), interceptorType, ServiceNames.AsyncInterceptorServiceName);
        }
        #endregion
    }
}
