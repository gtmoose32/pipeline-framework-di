using LightInject.Interception;
using PipelineFramework.Abstractions;

namespace PipelineFramework.LightInject
{
    public interface ILightInjectPipelineComponentResolver : IPipelineComponentResolver
    {
        void AddAsyncInterceptor(IInterceptor interceptor);

        void AddInterceptor(IInterceptor interceptor);
    }
}