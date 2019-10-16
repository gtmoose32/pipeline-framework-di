using LightInject;
using PipelineFramework.Abstractions;
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
        {
            var components = from t in assembly.GetTypes()
                let interfaces = t.GetInterfaces()
                where !t.IsAbstract &&
                      !t.IsInterface &&
                      interfaces.Any(IsAsyncPipelineComponent)
                select new
                {
                    ImplementingType = t,
                    AyncPipelineComponentInterfaces = interfaces.Where(IsAsyncPipelineComponent)
                };

            foreach (var component in components)
            {
                foreach (var i in component.AyncPipelineComponentInterfaces)
                {
                    serviceRegistry.Register(i, component.ImplementingType, component.ImplementingType.Name);
                }
            }

            return serviceRegistry;

            //Local function
            bool IsAsyncPipelineComponent(Type i) => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IAsyncPipelineComponent<>);
        }

        /// <summary>
        /// Scans the specified <see cref="Assembly"/> for any types that implement <see cref="IPipelineComponent{T}"/> and automatically registers those matching types with the LightInject container.
        /// </summary>
        /// <param name="serviceRegistry">Registry used to register any types located implementing <see cref="IPipelineComponent{T}"/></param>
        /// <param name="assembly">The specified <see cref="Assembly"/> to scan.</param>
        /// <returns><see cref="IServiceRegistry"/></returns>
        public static IServiceRegistry RegisterPipelineComponentsFromAssembly(this IServiceRegistry serviceRegistry, Assembly assembly)
        {
            var components = from t in assembly.GetTypes()
                let interfaces = t.GetInterfaces()
                where !t.IsAbstract &&
                      !t.IsInterface &&
                      interfaces.Any(IsPipelineComponent)
                select new
                {
                    ImplementingType = t,
                    PipelineComponentInterfaces = interfaces.Where(IsPipelineComponent)
                };

            foreach (var component in components)
            {
                foreach (var i in component.PipelineComponentInterfaces)
                {
                    serviceRegistry.Register(i, component.ImplementingType, component.ImplementingType.Name);
                }
            }

            return serviceRegistry;

            //Local function
            bool IsPipelineComponent(Type i) => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IPipelineComponent<>);
        }
    }
}