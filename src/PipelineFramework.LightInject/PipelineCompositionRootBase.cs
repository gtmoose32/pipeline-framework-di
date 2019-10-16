using LightInject;
using PipelineFramework.Abstractions;

namespace PipelineFramework.LightInject
{
    public abstract class PipelineCompositionRootBase : ICompositionRoot
    {
        public virtual void Compose(IServiceRegistry registry)
            => registry.RegisterSingleton<IPipelineComponentResolver>(factory => new PipelineComponentResolver(factory));
        
    }
}
