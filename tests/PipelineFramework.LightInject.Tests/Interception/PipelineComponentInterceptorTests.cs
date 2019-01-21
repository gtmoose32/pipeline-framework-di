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

namespace PipelineFramework.LightInject.Tests.Interception
{
    [TestClass]
    public class PipelineComponentInterceptorTests
    {
        private IServiceContainer _container;

        [TestInitialize]
        public void Init()
        {
            _container = new ServiceContainer(new ContainerOptions { EnablePropertyInjection = false });
        }

        [TestMethod]
        public void Interceptor_Test()
        {
            var logger = Substitute.For<ILogger>();
            _container.RegisterInstance(logger);
            _container.RegisterFrom<DefaultLoggingCompositionRoot>();
            _container.Register<IPipelineComponent<TestPayload>, FooComponent>();

            var component = _container.GetInstance<IPipelineComponent<TestPayload>>();
            var result = component.Execute(new TestPayload(), CancellationToken.None);

            result.Should().NotBeNull();
            logger.Received().Information(Arg.Is(LoggingMessages.SuccessMessage), Arg.Any<long>());
        }

        [TestMethod]
        public void Interceptor_ComponentException_Test()
        {
            var logger = Substitute.For<ILogger>();
            _container.RegisterInstance(logger);
            _container.RegisterFrom<DefaultLoggingCompositionRoot>();
            _container.Register<IPipelineComponent<TestPayload>, BarExceptionComponent>();

            var component = _container.GetInstance<IPipelineComponent<TestPayload>>();
            Action act = () => component.Execute(new TestPayload(), CancellationToken.None);

            act.Should().ThrowExactly<NotImplementedException>();
            logger.Received().Error(Arg.Any<Exception>(), Arg.Is(LoggingMessages.ExceptionMessage), Arg.Any<long>());
        }
    }
}
