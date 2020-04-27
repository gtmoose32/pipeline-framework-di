using FluentAssertions;
using LightInject;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PipelineFramework.Abstractions;
using PipelineFramework.LightInject.Interception;
using PipelineFramework.Tests.SharedInfrastructure;
using Serilog;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace PipelineFramework.LightInject.Tests.Interception
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    //[Ignore]
    public class PipelineComponentAsyncInterceptorBaseTests
    {
        private IServiceContainer _container;
        private ILogger _logger;

        [TestInitialize]
        public void Init()
        {
            _container = new ServiceContainer();
            _logger = Substitute.For<ILogger>();
            _logger.ForContext(Arg.Any<string>(), Arg.Any<string>())
                .Returns(_logger);
            _container.RegisterInstance(_logger);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _container?.Dispose();
        }

        [TestMethod]
        public async Task InterceptorInvokedForMatchingPayloadType()
        {
            // Arrange
            _container.AddPayloadScopedPipelineAsyncInterceptor<CustomPayloadInterceptor, InterceptorTestPayload>();
            _container.Register<IAsyncPipelineComponent<InterceptorTestPayload>, PayloadInterceptorAsyncTestComponent>();
            
            var component = _container.GetInstance<IAsyncPipelineComponent<InterceptorTestPayload>>();

            // Act
            var result = await component.ExecuteAsync(new InterceptorTestPayload(), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Count.Should().Be(1);

            _logger.Received().Information("Before");
            _logger.Received().Information("After");
        }

        [TestMethod]
        public async Task InterceptorNotInvokedForUnmatchedPayloadType()
        {
            // Arrange
            _container.AddPayloadScopedPipelineAsyncInterceptor<CustomPayloadInterceptor, InterceptorTestPayload>();
            _container.Register<IAsyncPipelineComponent<TestPayload>, AsyncTestComponent>();

            var component = _container.GetInstance<IAsyncPipelineComponent<TestPayload>>();

            // Act
            var result = await component.ExecuteAsync(new TestPayload(), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Count.Should().Be(1);
            _logger.DidNotReceiveWithAnyArgs().Information(null);
        }

        public class CustomPayloadInterceptor : PayloadScopedPipelineComponentAsyncInterceptorBase<InterceptorTestPayload>
        {
            private readonly ILogger _logger;

            public CustomPayloadInterceptor(ILogger logger)
            {
                _logger = logger;
            }

            protected override void BeforeExecute(InterceptorTestPayload payload)
            {
                _logger.Information("Before");
            }

            protected override void AfterExecute(InterceptorTestPayload payload)
            {
                _logger.Information("After");
            }
        }

        public class InterceptorTestPayload : TestPayload { }

        public class PayloadInterceptorAsyncTestComponent : AsyncPipelineComponentBase<InterceptorTestPayload>
        {
            public override Task<InterceptorTestPayload> ExecuteAsync(InterceptorTestPayload payload, CancellationToken cancellationToken)
            {
                payload.Count++;
                return Task.FromResult(payload);
            }
        }
    }
}