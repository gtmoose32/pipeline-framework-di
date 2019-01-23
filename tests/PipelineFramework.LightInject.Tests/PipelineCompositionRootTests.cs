using System;
using FluentAssertions;
using LightInject;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PipelineFramework.Abstractions;
using PipelineFramework.Tests.SharedInfrastructure;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using NSubstitute;
using PipelineFramework.LightInject.Interception;
using PipelineFramework.LightInject.Tests.Infrastructure;
using Serilog;

namespace PipelineFramework.LightInject.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class PipelineCompositionRootTests
    {
        private readonly IServiceContainer _container;

        public PipelineCompositionRootTests()
        {
            _container = new ServiceContainer(new ContainerOptions { EnablePropertyInjection = false });
            _container.RegisterFrom<TestCompositionRoot>();
        }

        [TestMethod]
        public void PipelineCompositionRoot_GetPipeline()
        {
            var result = _container.GetInstance<IPipeline<TestPayload>>();

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<Pipeline<TestPayload>>();
        }
    }
}
