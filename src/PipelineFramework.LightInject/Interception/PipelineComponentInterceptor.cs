using LightInject.Interception;
using Serilog;
using System;
using System.Diagnostics;

namespace PipelineFramework.LightInject.Interception
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
                logger.Information(LoggingMessages.SuccessMessage, stopwatch.ElapsedMilliseconds);
                return result;
            }
            catch (Exception e)
            {
                stopwatch.Stop();
                logger.Error(e, LoggingMessages.ExceptionMessage, stopwatch.ElapsedMilliseconds);
                throw;
            }
        }

        protected virtual ILogger EnrichLogger(IInvocationInfo invocationInfo) => Logger;
    }
}