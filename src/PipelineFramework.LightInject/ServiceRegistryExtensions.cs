using LightInject;
using LightInject.Interception;
using PipelineFramework.Abstractions;
using PipelineFramework.LightInject.Interception;
using PipelineFramework.LightInject.Logging;
using System;
using System.Linq;
using System.Reflection;

namespace PipelineFramework.LightInject
{
    /// <summary>
    /// Provides a set of extension methods to facilitate pipeline component LightInject container registration
    /// </summary>
    public static class ServiceRegistryExtensions
    {
        /// <summary>
        /// Registers the specified <see cref="TComponent"/> as an <see cref="IAsyncPipelineComponent{TPayload}"/> with the LightInject container.
        /// /// </summary>
        /// <typeparam name="TComponent">The component type to be registered.</typeparam>
        /// <typeparam name="TPayload">The payload type the pipeline should use.</typeparam>
        /// <param name="serviceRegistry">Registry used to register <see cref="IAsyncPipelineComponent{TPayload}"/></param>
        /// <param name="componentName">Optional name that will be used to register the component.  If not specified, the type name of <see cref="TComponent"/> will be used instead.</param>
        /// <returns><see cref="IServiceRegistry"/></returns>
        public static IServiceRegistry RegisterAsyncPipelineComponent<TComponent, TPayload>(
            this IServiceRegistry serviceRegistry, string componentName = null)
            where TComponent : IAsyncPipelineComponent<TPayload>
            => serviceRegistry.Register<IAsyncPipelineComponent<TPayload>, TComponent>(componentName ?? typeof(TComponent).Name);

        /// <summary>
        /// Registers the specified <see cref="TComponent"/> as an <see cref="IPipelineComponent{TPayload}"/> with the LightInject container.
        /// /// </summary>
        /// <typeparam name="TComponent">The component type to be registered.</typeparam>
        /// <typeparam name="TPayload">The payload type the pipeline should use.</typeparam>
        /// <param name="serviceRegistry">Registry used to register <see cref="IPipelineComponent{TPayload}"/></param>
        /// <param name="componentName">Optional name that will be used to register the component.  If not specified, the type name of <see cref="TComponent"/> will be used instead.</param>
        /// <returns><see cref="IServiceRegistry"/></returns>
        public static IServiceRegistry RegisterPipelineComponent<TComponent, TPayload>(
            this IServiceRegistry serviceRegistry, string componentName = null)
            where TComponent : IPipelineComponent<TPayload>
            => serviceRegistry.Register<IPipelineComponent<TPayload>, TComponent>(componentName ?? typeof(TComponent).Name);

        /// <summary>
        /// Scans the specified <see cref="Assembly"/> for any types that implement <see cref="IAsyncPipelineComponent{T}"/> and automatically registers those matching types with the LightInject container.
        /// </summary>
        /// <param name="serviceRegistry">Registry used to register any types located implementing <see cref="IAsyncPipelineComponent{T}"/></param>
        /// <param name="assembly">The specified <see cref="Assembly"/> to scan.</param>
        /// <returns><see cref="IServiceRegistry"/></returns>
        public static IServiceRegistry RegisterAsyncPipelineComponentsFromAssembly(this IServiceRegistry serviceRegistry, Assembly assembly)
            => RegisterComponentsFromAssembly(serviceRegistry, assembly, true);

        /// <summary>
        /// Scans the specified <see cref="Assembly"/> for any types that implement <see cref="IPipelineComponent{T}"/> and automatically registers those matching types with the LightInject container.
        /// </summary>
        /// <param name="serviceRegistry">Registry used to register any types located implementing <see cref="IPipelineComponent{T}"/></param>
        /// <param name="assembly">The specified <see cref="Assembly"/> to scan.</param>
        /// <returns><see cref="IServiceRegistry"/></returns>
        public static IServiceRegistry RegisterPipelineComponentsFromAssembly(this IServiceRegistry serviceRegistry, Assembly assembly)
            => RegisterComponentsFromAssembly(serviceRegistry, assembly);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TInterceptor"></typeparam>
        /// <param name="serviceRegistry"></param>
        public static void AddAsyncPipelineComponentLogging<TInterceptor>(this IServiceRegistry serviceRegistry)
            where TInterceptor : AsyncInterceptor
        {
            serviceRegistry.Register<TInterceptor>();

            serviceRegistry.Intercept(
                sr => sr.ServiceType.IsAsyncPipelineComponentInterface(),
                AsyncInterceptorProxyDefinition<TInterceptor>());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceRegistry"></param>
        public static void AddAsyncPipelineComponentLogging(this IServiceRegistry serviceRegistry)
        {
            serviceRegistry.Register<AsyncPipelineComponentInterceptor>();

            serviceRegistry.Intercept(
                sr => sr.ServiceType.IsAsyncPipelineComponentInterface(),
                AsyncInterceptorProxyDefinition<AsyncPipelineComponentInterceptor>());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TInterceptor"></typeparam>
        /// <param name="serviceRegistry"></param>
        public static void AddPipelineComponentLogging<TInterceptor>(this IServiceRegistry serviceRegistry)
            where TInterceptor : IInterceptor
        {
            serviceRegistry.Register<TInterceptor>();

            serviceRegistry.Intercept(
                sr => sr.ServiceType.IsPipelineComponentInterface(),
                InterceptorProxyDefinition<TInterceptor>());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceRegistry"></param>
        public static void AddPipelineComponentLogging(this IServiceRegistry serviceRegistry)
        {
            serviceRegistry.Register<PipelineComponentInterceptor>();

            serviceRegistry.Intercept(
                sr => sr.ServiceType.IsPipelineComponentInterface(),
                InterceptorProxyDefinition<PipelineComponentInterceptor>());
        }

        /// <summary>
        /// Add an interceptor that will only be invoked for <see cref="IAsyncPipelineComponent{TPayload}"/> of a specified payload type.
        /// </summary>
        /// <typeparam name="TInterceptor">The type of the interceptor</typeparam>
        /// <typeparam name="TPayload">The type of payload that must be matched</typeparam>
        /// <param name="serviceRegistry"></param>
        public static void AddPayloadScopedPipelineAsyncInterceptor<TInterceptor, TPayload>(this IServiceRegistry serviceRegistry) 
            where TInterceptor : PayloadScopedPipelineComponentAsyncInterceptorBase<TPayload> where TPayload : class
        {
            serviceRegistry.Register<TInterceptor>();

            serviceRegistry.Intercept(
                sr => sr.ServiceType.IsAsyncPipelineComponentInterface<TPayload>(),
                AsyncInterceptorProxyDefinition<TInterceptor>());
        }

        #region Private Extensions
        private static bool IsPipelineComponentInterface(this Type type) 
            => type.IsInterface && 
               type.IsGenericType &&
                type.GetGenericTypeDefinition() == typeof(IPipelineComponent<>);

        private static bool IsAsyncPipelineComponentInterface(this Type type)
            => type.IsInterface &&
               type.IsGenericType &&
               type.GetGenericTypeDefinition() == typeof(IAsyncPipelineComponent<>);

        private static bool IsAsyncPipelineComponentInterface<TPayload>(this Type type)
            => type.IsInterface &&
               type.IsGenericType &&
               type.GetGenericTypeDefinition() == typeof(IAsyncPipelineComponent<>) &&
               type.GenericTypeArguments[0] == typeof(TPayload);

        private static IServiceRegistry RegisterComponentsFromAssembly(this IServiceRegistry serviceRegistry, Assembly assembly, bool useAsyncComponents = false)
        {
            Func<Type, bool> isComponent;
            if (useAsyncComponents) isComponent = IsAsyncPipelineComponent;
            else isComponent = IsPipelineComponent;

            var components = from t in assembly.GetTypes()
                             let interfaces = t.GetInterfaces()
                             where !t.IsAbstract &&
                                   !t.IsInterface &&
                                   interfaces.Any(isComponent)
                             select new
                             {
                                 ImplementingType = t,
                                 PipelineComponentInterfaces = interfaces.Where(isComponent)
                             };

            foreach (var component in components)
            {
                foreach (var i in component.PipelineComponentInterfaces)
                {
                    serviceRegistry.Register(i, component.ImplementingType, component.ImplementingType.Name);
                }
            }

            return serviceRegistry;

            //Local functions
            bool IsAsyncPipelineComponent(Type i) => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IAsyncPipelineComponent<>);
            bool IsPipelineComponent(Type i) => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IPipelineComponent<>);
        }

        private static Action<IServiceFactory, ProxyDefinition> InterceptorProxyDefinition<TInterceptor>()
            where TInterceptor : IInterceptor
            => (factory, proxy) => proxy.Implement(factory.GetInterceptor<TInterceptor>, mi => mi.Name == "Execute");

        private static Action<IServiceFactory, ProxyDefinition> AsyncInterceptorProxyDefinition<TInterceptor>()
            where TInterceptor : AsyncInterceptor
            => (factory, proxy) => proxy.Implement(factory.GetInterceptor<TInterceptor>, mi => mi.Name == "ExecuteAsync");

        #endregion
    }
}
