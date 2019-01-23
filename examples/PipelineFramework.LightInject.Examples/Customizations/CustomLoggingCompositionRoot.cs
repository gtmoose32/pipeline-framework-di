using LightInject;
using LightInject.Interception;
using PipelineFramework.LightInject.Interception;

namespace PipelineFramework.LightInject.Examples.Customizations
{
    public class CustomLoggingCompositionRoot : DefaultLoggingCompositionRoot
    {
        protected override void RegisterAsyncInterceptor(IServiceRegistry serviceRegistry) =>
            serviceRegistry.Register<IInterceptor, CustomAsyncPipelineComponentInterceptor>(AsyncInterceptorServiceName);
        
    }
}
