using LightInject;
using PipelineFramework.Abstractions;
using PipelineFramework.Builder;
using PipelineFramework.Tests.SharedInfrastructure;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace PipelineFramework.LightInject.Tests.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public class TestCompositionRoot : PipelineCompositionRootBase
    {
        public override void Compose(IServiceRegistry registry)
        {
            base.Compose(registry);

            registry.Register<IPipelineComponent<TestPayload>, FooComponent>(typeof(FooComponent).Name);
            registry.Register<IPipelineComponent<TestPayload>, BarComponent>(typeof(BarComponent).Name);

            registry.Register<IDictionary<string, IDictionary<string, string>>>(
                factory => new Dictionary<string, IDictionary<string, string>>());

            registry.Register(factory =>
                PipelineBuilder<TestPayload>
                    .NonAsync()
                    .WithComponent<FooComponent>()
                    .WithComponent<BarComponent>()
                    .WithComponentResolver(factory.GetInstance<IPipelineComponentResolver>())
                    .WithSettings(factory.GetInstance<IDictionary<string, IDictionary<string, string>>>())
                    .Build());
        }
    }
}
