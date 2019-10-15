using LightInject.Interception;
using PipelineFramework.LightInject.Interception;
using Serilog;
using System.Linq;

namespace PipelineFramework.LightInject.Examples.Customizations
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class CustomAsyncPipelineComponentInterceptor : AsyncPipelineComponentInterceptor
    {
        public CustomAsyncPipelineComponentInterceptor(ILogger logger) 
            : base(logger)
        {
        }

        protected override ILogger EnrichLogger(IInvocationInfo invocationInfo)
        {
            var logger = base.EnrichLogger(invocationInfo);
            
            var payload = invocationInfo.Arguments.Where(arg => arg is ExamplePipelinePayload)
                .Cast<ExamplePipelinePayload>()
                .FirstOrDefault();

            return payload != null
                ? logger.ForContext("TransactionId", payload.TransactionId)
                : logger;
        }
    }
}
