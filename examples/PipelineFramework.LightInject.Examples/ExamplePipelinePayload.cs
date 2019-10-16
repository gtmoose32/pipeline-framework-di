using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace PipelineFramework.LightInject.Examples
{
    [ExcludeFromCodeCoverage]
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