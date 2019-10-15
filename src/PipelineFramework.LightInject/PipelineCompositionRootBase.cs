using LightInject;
using PipelineFramework.Abstractions;

namespace PipelineFramework.LightInject
{
    public abstract class PipelineCompositionRootBase : ICompositionRoot
    {
        public virtual void Compose(IServiceRegistry registry)
        {
            var container = registry as IServiceFactory;

            registry.RegisterSingleton<IPipelineComponentResolver>(factory => new PipelineComponentResolver(container));
        }
    }
}
