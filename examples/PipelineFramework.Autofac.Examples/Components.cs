using PipelineFramework.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace PipelineFramework.Autofac.Examples
{
    public abstract class ComponentBase : AsyncPipelineComponentBase<ExamplePipelinePayload>
    {
        public override Task<ExamplePipelinePayload> ExecuteAsync(ExamplePipelinePayload payload, CancellationToken cancellationToken)
        {
            payload.Messages.Add($"Component {Name} has been executed.");

            return Task.FromResult(payload);
        }
    }

    public class Component1 : ComponentBase
    {
        
    }

    public class Component2 : ComponentBase
    {
        
    }

    public class Component3 : ComponentBase
    {
        
    }
}