using LightInject.Interception;
using PipelineFramework.Abstractions;
using Serilog;
using System;
using System.Diagnostics;

namespace PipelineFramework.LightInject.Logging
{
    public class PipelineComponentInterceptor : IInterceptor
    {
        protected ILogger Logger { get; }

        public PipelineComponentInterceptor(ILogger logger)
        {
            Logger = logger;
        }

        public object Invoke(IInvocationInfo invocationInfo)
        {
            var logger = EnrichLogger(invocationInfo);
            var stopwatch = new Stopwatch();
            try
            {
                stopwatch.Start();
                var result = invocationInfo.Proceed();
                stopwatch.Stop();
                logger.Information(LogMessageTemplates.SuccessMessage, stopwatch.ElapsedMilliseconds);
                return result;
            }
            catch (Exception e)
            {
                stopwatch.Stop();
                logger.Error(e, LogMessageTemplates.ExceptionMessage, stopwatch.ElapsedMilliseconds);
                throw;
            }
        }

        protected virtual ILogger EnrichLogger(IInvocationInfo invocationInfo)
        {
            return invocationInfo.Proxy.Target is IPipelineComponent component
                ? Logger.ForContext(ComponentNameLoggingLabel, component.Name)
                : Logger;
        }

        public virtual string ComponentNameLoggingLabel => "ComponentName";
    }
}