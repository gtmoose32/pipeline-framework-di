using LightInject;
using LightInject.Interception;
using PipelineFramework.Abstractions;
using System;

namespace PipelineFramework.LightInject.Logging
{
    public static class LoggingExtensions
    {
        public static IPipelineComponentResolver WithAsyncPipelineComponentLogging<TInterceptor>(this IPipelineComponentResolver resolver, TInterceptor interceptor)
            where TInterceptor : IInterceptor
        {
            if (resolver is ILightInjectPipelineComponentResolver r)
                r.AddAsyncInterceptor(interceptor);

            return resolver;
        }

        public static IPipelineComponentResolver WithPipelineComponentLogging<TInterceptor>(this IPipelineComponentResolver resolver, TInterceptor interceptor)
            where TInterceptor : IInterceptor
        {
            if (resolver is ILightInjectPipelineComponentResolver r)
                r.AddInterceptor(interceptor);

            return resolver;
        }

        public static IInterceptor GetAsyncInterceptor(this IServiceFactory factory) => 
            factory.GetInstance<IInterceptor>(ServiceNames.AsyncInterceptorServiceName);

        public static IInterceptor GetInterceptor(this IServiceFactory factory) => 
            factory.GetInstance<IInterceptor>(ServiceNames.InterceptorServiceName);

        public static bool IsPipelineComponent(this Type type) =>
            type.IsInterface && type.IsGenericType &&
            type.GetGenericTypeDefinition() == typeof(IPipelineComponent<>);

        public static bool IsAsyncPipelineComponent(this Type type) =>
            type.IsInterface && type.IsGenericType &&
            type.GetGenericTypeDefinition() == typeof(IAsyncPipelineComponent<>);

        public static void AddAsyncPipelineComponentInterception(this IServiceRegistry serviceRegistry) => 
            serviceRegistry.Intercept(sr => 
                sr.ServiceType.IsAsyncPipelineComponent(),
                (factory, proxy) => proxy.Implement(factory.GetAsyncInterceptor, mi => mi.Name == "ExecuteAsync"));

        public static void AddPipelineComponentInterception(this IServiceRegistry serviceRegistry) =>
            serviceRegistry.Intercept(sr => 
                sr.ServiceType.IsPipelineComponent(),
                (factory, definition) => definition.Implement(factory.GetInterceptor, mi => mi.Name == "Execute"));

        public static void RegisterInterceptor(this IServiceRegistry serviceRegistry, Type interceptorType) => 
            serviceRegistry.Register(typeof(IInterceptor), interceptorType, ServiceNames.InterceptorServiceName);

        public static void RegisterAsyncInterceptor(this IServiceRegistry serviceRegistry, Type interceptorType) => 
            serviceRegistry.Register(typeof(IInterceptor), interceptorType, ServiceNames.AsyncInterceptorServiceName);
    }
}