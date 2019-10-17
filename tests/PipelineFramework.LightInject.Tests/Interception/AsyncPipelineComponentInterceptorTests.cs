using FluentAssertions;
using LightInject;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PipelineFramework.Abstractions;
using PipelineFramework.LightInject.Logging;
using PipelineFramework.Tests.SharedInfrastructure;
using Serilog;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using LightInject.Interception;

namespace PipelineFramework.LightInject.Tests.Interception
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class AsyncPipelineComponentInterceptorTests
    {
        private IServiceContainer _container;
        private ILogger _logger;

        [TestInitialize]
        public void Init()
        {
            _logger = Substitute.For<ILogger>();
            _logger.ForContext(Arg.Any<string>(), Arg.Any<string>())
                .Returns(_logger);

            _container = new ServiceContainer();
            _container.RegisterInstance(_logger);
        }

        [TestMethod]
        public async Task AsyncInterceptor_Test()
        {
            const string name = nameof(AsyncTestComponent);
            _container.AddAsyncPipelineComponentLogging();
            _container.Register<IAsyncPipelineComponent<TestPayload>, AsyncTestComponent>();

            var component = _container.GetInstance<IAsyncPipelineComponent<TestPayload>>();
            component.Initialize(name, null);
            var result = await component.ExecuteAsync(new TestPayload(), CancellationToken.None);

            result.Should().NotBeNull();
            
            _logger.Received().ForContext(Arg.Is("ComponentName"), Arg.Is(name));
            _logger.Received().Information(Arg.Is(LogMessageTemplates.SuccessMessage), Arg.Any<long>());
        }

        [TestMethod]
        public async Task AsyncInterceptor_CustomInterceptor_Test()
        {
            const string name = nameof(AsyncTestComponent);
            _container.AddAsyncPipelineComponentLogging<CustomAsyncInterceptor>();
            _container.Register<IAsyncPipelineComponent<TestPayload>, AsyncTestComponent>();

            var component = _container.GetInstance<IAsyncPipelineComponent<TestPayload>>();
            component.Initialize(name, null);
            var result = await component.ExecuteAsync(new TestPayload(), CancellationToken.None);

            result.Should().NotBeNull();

            _logger.Received().ForContext(Arg.Is("ComponentName"), Arg.Is(name));
            _logger.Received(2).ForContext(Arg.Any<string>(), Arg.Any<string>());
            _logger.Received().Information(Arg.Is(LogMessageTemplates.SuccessMessage), Arg.Any<long>());
        }

        [TestMethod]
        public void AsyncInterceptor_ComponentException_Test()
        {
            const string name = nameof(BarExceptionComponent);
            _container.AddAsyncPipelineComponentLogging();
            _container.Register<IAsyncPipelineComponent<TestPayload>, BarExceptionComponent>();

            var component = _container.GetInstance<IAsyncPipelineComponent<TestPayload>>();
            component.Initialize(name, null);
            Func<Task> act = () => component.ExecuteAsync(new TestPayload(), CancellationToken.None);

            act.Should().ThrowExactly<NotImplementedException>();
            
            _logger.Received().ForContext(Arg.Is("ComponentName"), Arg.Is(name));
            _logger.Received().Error(Arg.Any<Exception>(), Arg.Is(LogMessageTemplates.ExceptionMessage), Arg.Any<long>());
        }
    }

    [ExcludeFromCodeCoverage]
    public class CustomAsyncInterceptor : AsyncPipelineComponentInterceptor
    {
        public CustomAsyncInterceptor(ILogger logger) 
            : base(logger)
        {
        }

        protected override ILogger EnrichLogger(IInvocationInfo invocationInfo)
        {
            var logger = base.EnrichLogger(invocationInfo);

            return logger.ForContext("Test", "me");
        }
    }
}
