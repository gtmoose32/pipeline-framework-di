using LightInject;
using PipelineFramework.Abstractions;
using System;
using System.Threading;
using System.Threading.Tasks;

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
            var container = new ServiceContainer();
            container.RegisterFrom<CompositionRoot>();

            var pipeline = container.GetInstance<IAsyncPipeline<ExamplePipelinePayload>>();

            var result = await pipeline.ExecuteAsync(new ExamplePipelinePayload(), CancellationToken.None);

            result.Messages.ForEach(Console.WriteLine);

            Console.Read();

        }
    }
}
