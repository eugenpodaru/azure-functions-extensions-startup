namespace Devlight.Azure.Functions.Extensions.Startup.Listeners
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Host.Executors;
    using Microsoft.Azure.WebJobs.Host.Listeners;
    using Microsoft.Extensions.Options;

    [Singleton(Mode = SingletonMode.Listener)]
    internal sealed class StartupTriggerListener : IListener
    {
        private readonly ITriggeredFunctionExecutor _executor;
        private readonly IOptions<StartupTriggerOptions> _options;
        private readonly CancellationTokenSource _cancellationTokenSource;

        private bool _disposed;

        public StartupTriggerListener(ITriggeredFunctionExecutor executor, IOptions<StartupTriggerOptions> options)
        {
            _executor = executor ?? throw new ArgumentNullException(nameof(executor));
            _options = options ?? throw new ArgumentNullException(nameof(options));

            _cancellationTokenSource = new CancellationTokenSource();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            await InvokeJobFunction();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            _cancellationTokenSource.Cancel();

            return Task.FromResult(true);
        }

        public void Cancel()
        {
            ThrowIfDisposed();

            _cancellationTokenSource.Cancel();
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource.Dispose();

                _disposed = true;
            }
        }

        /// <summary>
        /// Invokes the job function.
        /// </summary>
        internal async Task InvokeJobFunction()
        {
            var token = _cancellationTokenSource.Token;
            var startupInfo = StartupInfo.Create(_options.Value?.Title, _options.Value?.Version);
            var input = new TriggeredFunctionData
            {
                TriggerValue = startupInfo
            };

            try
            {
                var result = await _executor.TryExecuteAsync(input, token);
                if (!result.Succeeded)
                {
                    token.ThrowIfCancellationRequested();
                }
            }
            catch
            {
                // We don't want any function errors to stop the execution
                // schedule. Errors will be logged to Dashboard already.
            }
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(null);
            }
        }
    }
}
