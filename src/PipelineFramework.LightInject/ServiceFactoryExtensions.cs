using LightInject;
using LightInject.Interception;

namespace PipelineFramework.LightInject
{
    public static class ServiceFactoryExtensions
    {
        public static IInterceptor GetAsyncInterceptor(this IServiceFactory factory) 
            => factory.GetInstance<AsyncInterceptor>();

        public static IInterceptor GetInterceptor(this IServiceFactory factory) 
            => factory.GetInstance<IInterceptor>();
    }
}