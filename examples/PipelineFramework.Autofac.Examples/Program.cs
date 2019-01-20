using Autofac;
using PipelineFramework.Abstractions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PipelineFramework.Autofac.Examples
{
    class Program
    {
        static async Task Main()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<PipelineModule>();
            var container = builder.Build();

            var pipeline = container.Resolve<IAsyncPipeline<ExamplePipelinePayload>>();

            var result = await pipeline.ExecuteAsync(new ExamplePipelinePayload(), CancellationToken.None);

            result.Messages.ForEach(Console.WriteLine);

            Console.Read();
        }
    }
}
