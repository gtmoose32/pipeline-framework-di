using LightInject;
using LightInject.Interception;
using PipelineFramework.Abstractions;
using PipelineFramework.LightInject.Interception;
using System;

namespace PipelineFramework.LightInject
{
    public class DefaultLoggingCompositionRoot : ICompositionRoot
    {
        protected const string AsyncInterceptorServiceName = "AsyncInterceptorService";
        protected const string InterceptorServiceName = "InterceptorService";

        public void Compose(IServiceRegistry serviceRegistry)
        {
            RegisterInterceptor(serviceRegistry);
            RegisterAsyncInterceptor(serviceRegistry);

            serviceRegistry.Intercept(sr => IsAsyncPipelineComponent(sr.ServiceType),
                (factory, proxy) => proxy.Implement(() => GetAsyncInterceptor(factory), mi => mi.Name == "ExecuteAsync"));

            serviceRegistry.Intercept(sr => IsPipelineComponent(sr.ServiceType),
                    (factory, definition) => definition.Implement(() => GetInterceptor(factory), mi => mi.Name == "Execute"));
        }

        protected virtual void RegisterInterceptor(IServiceRegistry serviceRegistry)
            => serviceRegistry.Register<IInterceptor, PipelineComponentInterceptor>(InterceptorServiceName);

        protected virtual void RegisterAsyncInterceptor(IServiceRegistry serviceRegistry)
            => serviceRegistry.Register<IInterceptor, AsyncPipelineComponentInterceptor>(AsyncInterceptorServiceName);

        #region Private Methods
        private static IInterceptor GetAsyncInterceptor(IServiceFactory factory)
            => factory.GetInstance<IInterceptor>(AsyncInterceptorServiceName);

        private static IInterceptor GetInterceptor(IServiceFactory factory)
            => factory.GetInstance<IInterceptor>(InterceptorServiceName);

        private static bool IsPipelineComponent(Type type) =>
            type.IsInterface && type.IsGenericType &&
            type.GetGenericTypeDefinition() == typeof(IPipelineComponent<>);

        private static bool IsAsyncPipelineComponent(Type type) =>
            type.IsInterface && type.IsGenericType &&
            type.GetGenericTypeDefinition() == typeof(IAsyncPipelineComponent<>);
        #endregion
    }
}
