using LightInject;
using LightInject.Interception;

namespace PipelineFramework.LightInject
{
    public static class ServiceFactoryExtensions
    {
        public static IInterceptor GetInterceptor<TInterceptor>(this IServiceFactory factory) where TInterceptor : IInterceptor
            => factory.GetInstance<TInterceptor>();
    }
}