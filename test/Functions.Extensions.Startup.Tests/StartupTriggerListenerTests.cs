namespace Devlight.Azure.Functions.Extensions.Startup.Tests
{
    using Listeners;
    using Microsoft.Azure.WebJobs.Host.Executors;
    using Microsoft.Extensions.Options;
    using Moq;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;

    public class StartupTriggerListenerTests
    {
        private IOptions<StartupTriggerOptions> _options;
        private StartupTriggerListener _listener;
        private Mock<ITriggeredFunctionExecutor> _mockTriggerExecutor;
        private TriggeredFunctionData _triggeredFunctionData;

        public StartupTriggerListenerTests()
        {
            CreateTestListener();
        }

        [Fact]
        public async Task StartAsync_InvokesJobFunctionImmediately()
        {
            CancellationToken cancellationToken = CancellationToken.None;
            await _listener.StartAsync(cancellationToken);

            _mockTriggerExecutor.Verify(p => p.TryExecuteAsync(It.IsAny<TriggeredFunctionData>(), It.IsAny<CancellationToken>()), Times.Once());

            _listener.Dispose();
        }

        private void CreateTestListener(Action functionAction = null)
        {
            _options = Options.Create(new StartupTriggerOptions());
            _mockTriggerExecutor = new Mock<ITriggeredFunctionExecutor>(MockBehavior.Strict);
            _mockTriggerExecutor.Setup(p => p.TryExecuteAsync(It.IsAny<TriggeredFunctionData>(), It.IsAny<CancellationToken>()))
                .Callback<TriggeredFunctionData, CancellationToken>((mockFunctionData, mockToken) =>
                {
                    _triggeredFunctionData = mockFunctionData;
                    functionAction?.Invoke();
                })
                .Returns(Task.FromResult(new FunctionResult(true)));
            _listener = new StartupTriggerListener(_mockTriggerExecutor.Object, _options);
        }
    }
}
