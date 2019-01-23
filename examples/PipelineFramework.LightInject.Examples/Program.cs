using System;
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
            await Examples.RunNormalCompositionRootExampleAsync();

            await Examples.RunCompositionRootWithDefaultLoggingExampleAsync();

            await Examples.RunCompositionRootWithCustomLoggingExampleAsync();

            Console.Read();
        }
    }
}
