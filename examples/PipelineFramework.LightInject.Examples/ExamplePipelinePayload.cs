using System;
using System.Collections.Generic;

namespace PipelineFramework.LightInject.Examples
{
    public class ExamplePipelinePayload
    {
        public ExamplePipelinePayload()
        {
            Messages = new List<string>();
            TransactionId = Guid.NewGuid();
        }

        public List<string> Messages { get; }

        public Guid TransactionId { get; }
    }
}