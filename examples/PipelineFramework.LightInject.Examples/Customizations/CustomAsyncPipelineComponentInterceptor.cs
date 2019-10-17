using LightInject.Interception;
using PipelineFramework.LightInject.Logging;
using Serilog;
using System.Linq;

namespace PipelineFramework.LightInject.Examples.Customizations
{
    [ExcludeFromCodeCoverage]
    public class CustomAsyncPipelineComponentInterceptor : AsyncPipelineComponentInterceptor
    {
        public CustomAsyncPipelineComponentInterceptor(ILogger logger) 
            : base(logger)
        {
        }

        ///Override to add custom logger context through LogEventEnrichers
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

        //Override to alter the name of the property enricher that represents the name of the pipeline component
        public override string ComponentNameLoggingLabel => "Step";
    }
}
