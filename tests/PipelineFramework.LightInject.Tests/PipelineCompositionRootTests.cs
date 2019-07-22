using FluentAssertions;
using LightInject;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PipelineFramework.Abstractions;
using PipelineFramework.LightInject.Tests.Infrastructure;
using PipelineFramework.Tests.SharedInfrastructure;
using System.Diagnostics.CodeAnalysis;

namespace PipelineFramework.LightInject.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class PipelineCompositionRootTests
    {
        private readonly IServiceContainer _container;

        public PipelineCompositionRootTests()
        {
            _container = new ServiceContainer();
            _container.RegisterFrom<TestCompositionRoot>();
        }

        [TestMethod]
        public void PipelineCompositionRoot_GetPipeline()
        {
            var result = _container.GetInstance<IPipeline<TestPayload>>();

            result.Should().NotBeNull();
        }
    }
}
