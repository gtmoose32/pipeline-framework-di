using PipelineFramework.Abstractions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace PipelineFramework.LightInject.Examples
{
    [ExcludeFromCodeCoverage]
    public abstract class ComponentBase : AsyncPipelineComponentBase<ExamplePipelinePayload>
    {
        public override Task<ExamplePipelinePayload> ExecuteAsync(ExamplePipelinePayload payload, CancellationToken cancellationToken)
        {
            payload.Messages.Add($"Component {Name} has been executed.");

            return Task.FromResult(payload);
        }
    }

    //Note:  ExecuteAsync method must be overriden in the implementations even if they only need to call the base class implementation.  
    //       This is due to a issue with LightInject interception not working if the intercepted method isn't present on the class being used 
    //       despite a base class implementation being present.
    [ExcludeFromCodeCoverage]
    public class Component1 : ComponentBase
    {
        // ReSharper disable once RedundantOverriddenMember
        public override Task<ExamplePipelinePayload> ExecuteAsync(ExamplePipelinePayload payload, CancellationToken cancellationToken)
            => base.ExecuteAsync(payload, cancellationToken);
    }

    [ExcludeFromCodeCoverage]
    public class Component2 : ComponentBase
    {
        // ReSharper disable once RedundantOverriddenMember
        public override Task<ExamplePipelinePayload> ExecuteAsync(ExamplePipelinePayload payload, CancellationToken cancellationToken)
            => base.ExecuteAsync(payload, cancellationToken);
    }

    [ExcludeFromCodeCoverage]
    public class Component3 : ComponentBase
    {
        // ReSharper disable once RedundantOverriddenMember
        public override Task<ExamplePipelinePayload> ExecuteAsync(ExamplePipelinePayload payload, CancellationToken cancellationToken)
            => base.ExecuteAsync(payload, cancellationToken);
    }

    [ExcludeFromCodeCoverage]
    public class ExceptionComponent : AsyncPipelineComponentBase<ExamplePipelinePayload>
    {
        public override Task<ExamplePipelinePayload> ExecuteAsync(ExamplePipelinePayload payload, CancellationToken cancellationToken)
            => throw new InvalidOperationException();
    }
}