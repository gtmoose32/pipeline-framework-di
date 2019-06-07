using PipelineFramework.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace PipelineFramework.LightInject.Examples
{
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
    public class Component1 : ComponentBase
    {
        // ReSharper disable once RedundantOverriddenMember
        public override Task<ExamplePipelinePayload> ExecuteAsync(ExamplePipelinePayload payload, CancellationToken cancellationToken)
        {
            return base.ExecuteAsync(payload, cancellationToken);
        }
    }

    public class Component2 : ComponentBase
    {
        // ReSharper disable once RedundantOverriddenMember
        public override Task<ExamplePipelinePayload> ExecuteAsync(ExamplePipelinePayload payload, CancellationToken cancellationToken)
        {
            return base.ExecuteAsync(payload, cancellationToken);
        } 
    }

    public class Component3 : ComponentBase
    {
        // ReSharper disable once RedundantOverriddenMember
        public override Task<ExamplePipelinePayload> ExecuteAsync(ExamplePipelinePayload payload, CancellationToken cancellationToken)
        {
            return base.ExecuteAsync(payload, cancellationToken);
        }
    }

    public class ExceptionComponent : AsyncPipelineComponentBase<ExamplePipelinePayload>
    {
        public override Task<ExamplePipelinePayload> ExecuteAsync(ExamplePipelinePayload payload, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}