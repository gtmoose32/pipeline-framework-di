using LightInject;
using PipelineFramework.Abstractions;
using System;
using System.Linq;
using System.Reflection;

namespace PipelineFramework.LightInject
{
    public static class ServiceRegistryExtensions
    {
        public static IServiceRegistry RegisterAsyncPipelineComponent<TComponent, TPayload>(
            this IServiceRegistry serviceRegistry, string componentName = null)
            where TComponent : IAsyncPipelineComponent<TPayload>
            => serviceRegistry.Register<IAsyncPipelineComponent<TPayload>, TComponent>(componentName ?? typeof(TComponent).Name);

        public static IServiceRegistry RegisterPipelineComponent<TComponent, TPayload>(
            this IServiceRegistry serviceRegistry, string componentName = null)
            where TComponent : IPipelineComponent<TPayload>
            => serviceRegistry.Register<IPipelineComponent<TPayload>, TComponent>(componentName ?? typeof(TComponent).Name);

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