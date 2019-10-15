using LightInject;
using PipelineFramework.Abstractions;
using PipelineFramework.LightInject.Examples.Customizations;
using PipelineFramework.LightInject.Logging;
using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PipelineFramework.LightInject.Examples
{
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
                container.Register<ILogger>(factory => new LoggerConfiguration().WriteTo.Console().CreateLogger(), new PerContainerLifetime());
                container.RegisterFrom<DefaultLoggingCompositionRoot>();

                var pipeline = container.GetInstance<IAsyncPipeline<ExamplePipelinePayload>>(PipelineNames.PipelineName);
                var result = await pipeline.ExecuteAsync(new ExamplePipelinePayload(), CancellationToken.None);

                Console.WriteLine($"Pipeline has completed execution and returned '{result.Messages.Count}' component messages.");
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
                container.Register<ILogger>(factory => new LoggerConfiguration().WriteTo.Console().CreateLogger(), new PerContainerLifetime());
                container.RegisterFrom<CustomLoggingCompositionRoot>();

                var pipeline = container.GetInstance<IAsyncPipeline<ExamplePipelinePayload>>(PipelineNames.PipelineName);
                var result = await pipeline.ExecuteAsync(new ExamplePipelinePayload(), CancellationToken.None);

                Console.WriteLine($"Pipeline has completed execution and returned '{result.Messages.Count}' component messages.");
                Console.WriteLine("\n************** COMPOSITION ROOT WITH CUSTOM LOGGING RUN END **************\n\n");
            }
        }
    }
}
