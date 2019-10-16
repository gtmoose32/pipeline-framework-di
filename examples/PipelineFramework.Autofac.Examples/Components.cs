using PipelineFramework.Abstractions;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace PipelineFramework.Autofac.Examples
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

    [ExcludeFromCodeCoverage]
    public class Component1 : ComponentBase
    {
        
    }

    [ExcludeFromCodeCoverage]
    public class Component2 : ComponentBase
    {
        
    }

    [ExcludeFromCodeCoverage]
    public class Component3 : ComponentBase
    {
        
    }
}