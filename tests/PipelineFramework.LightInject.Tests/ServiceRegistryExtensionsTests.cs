using FluentAssertions;
using LightInject;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PipelineFramework.Abstractions;
using PipelineFramework.Tests.SharedInfrastructure;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace PipelineFramework.LightInject.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class ServiceRegistryExtensionsTests
    {
        private IServiceContainer _sut;

        [TestInitialize]
        public void Init()
        {
            _sut = new ServiceContainer(new ContainerOptions {EnablePropertyInjection = false});
        }


        [TestMethod]
        public void RegisterAsyncPipelineComponent_Test()
        {
            //Arrange
            _sut.RegisterAsyncPipelineComponent<AsyncTestComponent, TestPayload>();

            //Act
            var result = _sut.GetInstance<IAsyncPipelineComponent<TestPayload>>(nameof(AsyncTestComponent));

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<AsyncTestComponent>();
        }

        [TestMethod]
        public void RegisterPipelineComponent_Test()
        {
            //Arrange
            _sut.RegisterPipelineComponent<FooComponent, TestPayload>();

            //Act
            var result = _sut.GetInstance<IPipelineComponent<TestPayload>>(nameof(FooComponent));

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<FooComponent>();
        }

        [TestMethod]
        public void RegisterAsyncPipelineComponentsFromAssembly_Test()
        {
            //Arrange
            _sut.RegisterAsyncPipelineComponentsFromAssembly(Assembly.GetExecutingAssembly());

            //Act
            var results = _sut.GetAllInstances(typeof(IAsyncPipelineComponent<TestPayload>)).ToArray();

            //Assert
            results.Should().NotBeNullOrEmpty();
            results.Length.Should().Be(8);
        }

        [TestMethod]
        public void RegisterPipelineComponentsFromAssembly_Test()
        {
            //Arrange
            _sut.RegisterPipelineComponentsFromAssembly(Assembly.GetExecutingAssembly());

            //Act
            var results = _sut.GetAllInstances(typeof(IPipelineComponent<TestPayload>)).ToArray();

            //Assert
            results.Should().NotBeNullOrEmpty();
            results.Length.Should().Be(7);
        }
    }
}