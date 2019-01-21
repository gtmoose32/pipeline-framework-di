using LightInject;
using PipelineFramework.Abstractions;
using PipelineFramework.LightInject.Interception;
using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;
using PipelineFramework.LightInject.Examples.Customizations;

namespace PipelineFramework.LightInject.Examples
{
    class Program
    {
        /// <summary>
        /// This program demonstrates how to use LightInject composition roots to compose, instantiate and execute pipelines.
        /// </summary>
        /// <returns></returns>
        static async Task Main()
        {
            using (var container = new ServiceContainer())
            {
                Console.WriteLine("************** COMPOSITION ROOT RUN BEGIN **************\n");

                container.RegisterFrom<CompositionRoot>();

                var pipeline = container.GetInstance<IAsyncPipeline<ExamplePipelinePayload>>();
                var result = await pipeline.ExecuteAsync(new ExamplePipelinePayload(), CancellationToken.None);

                result.Messages.ForEach(Console.WriteLine);

                Console.WriteLine("************** COMPOSITION ROOT RUN END **************\n\n");
            }

            using (var container = new ServiceContainer())
            {
                Console.WriteLine("************** COMPOSITION ROOT WITH DEFAULT LOGGING RUN BEGIN **************\n");

                container.RegisterFrom<CompositionRoot>();
                container.Register<ILogger>(factory => new LoggerConfiguration().WriteTo.Console().CreateLogger(), new PerContainerLifetime());
                container.RegisterFrom<DefaultLoggingCompositionRoot>();

                var pipeline = container.GetInstance<IAsyncPipeline<ExamplePipelinePayload>>();

                var result = await pipeline.ExecuteAsync(new ExamplePipelinePayload(), CancellationToken.None);

                result.Messages.ForEach(Console.WriteLine);

                Console.WriteLine("************** COMPOSITION ROOT WITH DEFAULT LOGGING RUN BEGIN **************\n\n");
            }

            using (var container = new ServiceContainer())
            {
                Console.WriteLine("************** COMPOSITION ROOT WITH CUSTOM LOGGING RUN BEGIN **************\n");

                container.RegisterFrom<CompositionRoot>();
                container.Register<ILogger>(factory => new LoggerConfiguration().WriteTo.Console().CreateLogger(), new PerContainerLifetime());
                container.RegisterFrom<CustomLoggingCompositionRoot>();

                var pipeline = container.GetInstance<IAsyncPipeline<ExamplePipelinePayload>>();

                var result = await pipeline.ExecuteAsync(new ExamplePipelinePayload(), CancellationToken.None);

                result.Messages.ForEach(Console.WriteLine);

                Console.WriteLine("************** COMPOSITION ROOT WITH CUSTOM LOGGING RUN BEGIN **************\n\n");
            }

            Console.Read();
        }
    }
}
