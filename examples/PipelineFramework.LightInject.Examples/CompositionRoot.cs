using LightInject;
using PipelineFramework.Abstractions;
using System;
using System.Collections.Generic;

namespace PipelineFramework.LightInject.Examples
{
    public class CompositionRoot : PipelineCompositionRootBase
    {
        public override void Compose(IServiceRegistry registry)
        {
            //Inheriting from PipelineCompositionRootBase and calling base class Compose method
            //will register the LightInject container implementation of IPipelineComponentResolver automatically.
            base.Compose(registry);

            registry
                .Register<IAsyncPipelineComponent<ExamplePipelinePayload>, Component1>(typeof(Component1).Name)
                .Register<IAsyncPipelineComponent<ExamplePipelinePayload>, Component3>(typeof(Component2).Name)
                .Register<IAsyncPipelineComponent<ExamplePipelinePayload>, Component3>(typeof(Component3).Name)
                .Register<IEnumerable<Type>>(factory =>
                    new[]
                    {
                        typeof(Component1),
                        typeof(Component2),
                        typeof(Component3)
                    })
                .Register<IDictionary<string, IDictionary<string, string>>>(factory => 
                    new Dictionary<string, IDictionary<string, string>>())
                .Register<IAsyncPipeline<ExamplePipelinePayload>>(factory =>
                    new AsyncPipeline<ExamplePipelinePayload>(
                        factory.GetInstance<IPipelineComponentResolver>(),
                        factory.GetInstance<IEnumerable<Type>>(),
                        factory.GetInstance<IDictionary<string, IDictionary<string, string>>>()));
        }
    }
}
