using Autofac;
using PipelineFramework.Abstractions;
using PipelineFramework.Builder;
using PipelineFramework.Tests.SharedInfrastructure;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace PipelineFramework.Autofac.Tests.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public class TestPipelineModule : PipelineModuleBase
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<FooComponent>().Named<IPipelineComponent<TestPayload>>(typeof(FooComponent).Name);
            builder.RegisterType<BarComponent>().Named<IPipelineComponent<TestPayload>>(typeof(BarComponent).Name);

            builder.RegisterType<FooComponent>().Named<IAsyncPipelineComponent<TestPayload>>(typeof(FooComponent).Name);
            builder.RegisterType<BarComponent>().Named<IAsyncPipelineComponent<TestPayload>>(typeof(BarComponent).Name);

            builder.RegisterInstance(new Dictionary<string, IDictionary<string, string>>())
                .As<IDictionary<string, IDictionary<string, string>>>();

            builder.Register(context => 
                    PipelineBuilder<TestPayload>
                        .NonAsync()
                        .WithComponent<FooComponent>()
                        .WithComponent<BarComponent>()
                        .WithComponentResolver(context.Resolve<IPipelineComponentResolver>())
                        .WithSettings(context.Resolve<IDictionary<string, IDictionary<string, string>>>())
                        .Build())
                .As<IPipeline<TestPayload>>();

            builder.Register(context =>
                PipelineBuilder<TestPayload>
                    .Async()
                    .WithComponent<FooComponent>()
                    .WithComponent<BarComponent>()
                    .WithComponentResolver(context.Resolve<IPipelineComponentResolver>())
                    .WithSettings(context.Resolve<IDictionary<string, IDictionary<string, string>>>())
                    .Build())
                .As<IAsyncPipeline<TestPayload>>();
        }
    }
}
