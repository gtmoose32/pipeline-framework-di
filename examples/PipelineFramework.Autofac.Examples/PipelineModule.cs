using Autofac;
using PipelineFramework.Abstractions;
using System;
using System.Collections.Generic;

namespace PipelineFramework.Autofac.Examples
{
    public class PipelineModule : PipelineModuleBase
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<Component1>()
                .Named<IAsyncPipelineComponent<ExamplePipelinePayload>>(typeof(Component1).Name);
            builder.RegisterType<Component2>()
                .Named<IAsyncPipelineComponent<ExamplePipelinePayload>>(typeof(Component2).Name);
            builder.RegisterType<Component3>()
                .Named<IAsyncPipelineComponent<ExamplePipelinePayload>>(typeof(Component3).Name);

            builder.RegisterInstance(
                new[]
                {
                    typeof(Component1),
                    typeof(Component2),
                    typeof(Component3)
                })
                .As<IEnumerable<Type>>();

            builder.RegisterInstance(new Dictionary<string, IDictionary<string, string>>())
                .As<IDictionary<string, IDictionary<string, string>>>();

            builder.Register(context =>
                    new AsyncPipeline<ExamplePipelinePayload>(
                        context.Resolve<IPipelineComponentResolver>(),
                        context.Resolve<IEnumerable<Type>>(),
                        context.Resolve<IDictionary<string, IDictionary<string, string>>>()))
                .As<IAsyncPipeline<ExamplePipelinePayload>>();
        }
    }
}
