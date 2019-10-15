using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace PipelineFramework.Autofac.Examples
{
    [ExcludeFromCodeCoverage]
    public class ExamplePipelinePayload
    {
        public ExamplePipelinePayload()
        {
            Messages = new List<string>();
        }

        public List<string> Messages { get; }
    }
}