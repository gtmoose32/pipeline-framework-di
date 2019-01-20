using System.Collections.Generic;

namespace PipelineFramework.Autofac.Examples
{
    public class ExamplePipelinePayload
    {
        public ExamplePipelinePayload()
        {
            Messages = new List<string>();
        }

        public List<string> Messages { get; }
    }
}