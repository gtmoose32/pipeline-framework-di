using LightInject;
using PipelineFramework.Abstractions;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

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

            registry  //Register components
                .Register<IAsyncPipelineComponent<ExamplePipelinePayload>, Component1>(typeof(Component1).Name)
                .Register<IAsyncPipelineComponent<ExamplePipelinePayload>, Component3>(typeof(Component2).Name)
                .Register<IAsyncPipelineComponent<ExamplePipelinePayload>, Component3>(typeof(Component3).Name);

            //Register configuration
            registry.Register<IDictionary<string, IDictionary<string, string>>>(factory =>
                    new Dictionary<string, IDictionary<string, string>>());

            //Register pipeline with builder
            registry.Register(factory =>
                PipelineBuilder<ExamplePipelinePayload>
                    .Async()
                    .WithComponent<Component1>()
                    .WithComponent<Component2>()
                    .WithComponent<Component3>()
                    .WithComponentResolver(factory.GetInstance<IPipelineComponentResolver>())
                    .WithSettings(factory.GetInstance<IDictionary<string, IDictionary<string, string>>>())
                    .Build(),
                Examples.PipelineNames.PipelineName,
                new PerContainerLifetime());

            registry.Register(factory =>
                    PipelineBuilder<ExamplePipelinePayload>
                        .Async()
                        .WithComponent<Component1>()
                        .WithComponent<Component2>()
                        .WithComponent<ExceptionComponent>()
                        .WithComponentResolver(factory.GetInstance<IPipelineComponentResolver>())
                        .WithSettings(factory.GetInstance<IDictionary<string, IDictionary<string, string>>>())
                        .Build(),
                Examples.PipelineNames.ExceptionPipelineName,
                new PerContainerLifetime());
        }
    }
}
