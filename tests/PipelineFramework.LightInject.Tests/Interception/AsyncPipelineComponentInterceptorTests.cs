using FluentAssertions;
using LightInject;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PipelineFramework.Abstractions;
using PipelineFramework.LightInject.Interception;
using PipelineFramework.Tests.SharedInfrastructure;
using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PipelineFramework.LightInject.Tests.Interception
{
    [TestClass]
    public class AsyncPipelineComponentInterceptorTests
    {
        private IServiceContainer _container;

        [TestInitialize]
        public void Init()
        {
            _container = new ServiceContainer(new ContainerOptions { EnablePropertyInjection = false });
        }

        [TestMethod]
        public async Task AsyncInterceptor_Test()
        {
            var logger = Substitute.For<ILogger>();
            _container.RegisterInstance(logger);
            _container.RegisterFrom<DefaultLoggingCompositionRoot>();
            _container.Register<IAsyncPipelineComponent<TestPayload>, AsyncTestComponent>();

            var component = _container.GetInstance<IAsyncPipelineComponent<TestPayload>>();
            var result = await component.ExecuteAsync(new TestPayload(), CancellationToken.None);

            result.Should().NotBeNull();
            logger.Received().Information(Arg.Is(LoggingMessages.SuccessMessage), Arg.Any<long>());
        }

        [TestMethod]
        public void AsyncInterceptor_ComponentException_Test()
        {
            var logger = Substitute.For<ILogger>();
            _container.RegisterInstance(logger);
            _container.RegisterFrom<DefaultLoggingCompositionRoot>();
            _container.Register<IAsyncPipelineComponent<TestPayload>, BarExceptionComponent>();

            var component = _container.GetInstance<IAsyncPipelineComponent<TestPayload>>();
            Func<Task> act = () => component.ExecuteAsync(new TestPayload(), CancellationToken.None);

            act.Should().ThrowExactly<NotImplementedException>();
            logger.Received().Error(Arg.Any<Exception>(), Arg.Is(LoggingMessages.ExceptionMessage), Arg.Any<long>());
        }
    }
}
