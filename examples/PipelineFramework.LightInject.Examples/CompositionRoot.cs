using LightInject;
using PipelineFramework.Abstractions;
using PipelineFramework.Builder;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace PipelineFramework.LightInject.Examples
{
    [ExcludeFromCodeCoverage]
    public class CompositionRoot : PipelineCompositionRootBase
    {
        public override void Compose(IServiceRegistry registry)
        {
            //Inheriting from PipelineCompositionRootBase and calling base class Compose method
            //will register the LightInject container implementation of IPipelineComponentResolver automatically.
            base.Compose(registry);

            //Register components
            registry.RegisterAsyncPipelineComponentsFromAssembly(Assembly.GetExecutingAssembly());

            //Register configuration
            registry.RegisterInstance(
                typeof(IDictionary<string, IDictionary<string, string>>), new Dictionary<string, IDictionary<string, string>>());

            //Register pipeline with builder
            registry.RegisterSingleton(factory =>
                PipelineBuilder<ExamplePipelinePayload>
                    .Async()
                    .WithComponent<Component1>()
                    .WithComponent<Component2>()
                    .WithComponent<Component3>()
                    .WithComponentResolver(factory.GetInstance<IPipelineComponentResolver>())
                    .WithSettings(factory.GetInstance<IDictionary<string, IDictionary<string, string>>>())
                    .Build(),
                Examples.PipelineNames.PipelineName);

            registry.RegisterSingleton(factory =>
                PipelineBuilder<ExamplePipelinePayload>
                    .Async()
                    .WithComponent<Component1>()
                    .WithComponent<Component2>()
                    .WithComponent<ExceptionComponent>()
                    .WithComponentResolver(factory.GetInstance<IPipelineComponentResolver>())
                    .WithNoSettings()
                    .Build(),
                Examples.PipelineNames.ExceptionPipelineName);
        }
    }
}
