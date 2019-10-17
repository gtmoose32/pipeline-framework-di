using LightInject;
using PipelineFramework.Abstractions;
using PipelineFramework.Exceptions;
using PipelineFramework.LightInject.Examples.Customizations;
using Serilog;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace PipelineFramework.LightInject.Examples
{
    [ExcludeFromCodeCoverage]
    public static class Examples
    {
        public static class PipelineNames
        {
            public const string ExceptionPipelineName = "ExceptionPipeline";

            public const string PipelineName = "Pipeline";
        }

        public static async Task RunNormalCompositionRootExampleAsync()
        {
            using (var container = new ServiceContainer())
            {
                Console.WriteLine("************** COMPOSITION ROOT RUN BEGIN **************\n");

                container.RegisterFrom<CompositionRoot>();

                var pipeline = container.GetInstance<IAsyncPipeline<ExamplePipelinePayload>>(PipelineNames.PipelineName);
                var result = await pipeline.ExecuteAsync(new ExamplePipelinePayload(), CancellationToken.None);

                Console.WriteLine($"Pipeline has completed execution and returned '{result.Messages.Count}' component messages.");
                Console.WriteLine("\n************** COMPOSITION ROOT RUN END **************\n\n");
            }
        }

        public static async Task RunCompositionRootWithDefaultLoggingExampleAsync()
        {
            using (var container = new ServiceContainer())
            {
                Console.WriteLine("************** COMPOSITION ROOT WITH DEFAULT LOGGING RUN BEGIN **************\n");

                container.RegisterFrom<CompositionRoot>();
                container.RegisterInstance(typeof(ILogger), new LoggerConfiguration().WriteTo.Console().CreateLogger());
                container.AddAsyncPipelineComponentLogging();

                var pipeline = container.GetInstance<IAsyncPipeline<ExamplePipelinePayload>>(PipelineNames.ExceptionPipelineName);
                
                var payload = new ExamplePipelinePayload();
                try
                {
                    payload = await pipeline.ExecuteAsync(payload, CancellationToken.None);
                }
                catch (PipelineExecutionException exception)
                {
                    container.GetInstance<ILogger>()
                        .Error(
                            exception, 
                            "An exception was thrown by pipeline component named '{ComponentName}'", 
                            exception.ThrowingComponent.Name);
                }

                Console.WriteLine($"Pipeline has completed execution and returned '{payload.Messages.Count}' component messages.");
                Console.WriteLine("\n************** COMPOSITION ROOT WITH DEFAULT LOGGING RUN END **************\n\n");
            }
        }

        public static async Task RunCompositionRootWithCustomLoggingExampleAsync()
        {
            using (var container = new ServiceContainer())
            {
                Console.WriteLine("************** COMPOSITION ROOT WITH CUSTOM LOGGING RUN BEGIN **************");
                Console.WriteLine("Note: Serilog.Console sink doesn't display enriched properties.");
                Console.WriteLine("Debugging code allows you to see the logger enriched for this sample.\n");
                
                container.RegisterFrom<CompositionRoot>();
                container.RegisterInstance(new LoggerConfiguration().WriteTo.Console().CreateLogger());
                container.AddAsyncPipelineComponentLogging<CustomAsyncPipelineComponentInterceptor>();

                var pipeline = container.GetInstance<IAsyncPipeline<ExamplePipelinePayload>>(PipelineNames.PipelineName);
                var result = await pipeline.ExecuteAsync(new ExamplePipelinePayload(), CancellationToken.None);

                Console.WriteLine($"Pipeline has completed execution and returned '{result.Messages.Count}' component messages.");
                Console.WriteLine("\n************** COMPOSITION ROOT WITH CUSTOM LOGGING RUN END **************\n\n");
            }
        }
    }
}
