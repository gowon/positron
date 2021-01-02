using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Positron.Core
{
    public class BackgroundWebHostService : IHostedService
    {
        private readonly ILogger<BackgroundWebHostService> _logger;
        private readonly IWebHost _webHost;
        private readonly Action<IWebHost> _webHostInitializedCallback;

        public BackgroundWebHostService(IWebHostBuilder builder, ILogger<BackgroundWebHostService> logger, Action<IWebHost> webHostInitializedCallback = null)
        {
            _webHost = builder?.Build() ?? throw new ArgumentNullException(nameof(builder));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _webHostInitializedCallback = webHostInitializedCallback;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("Starting background web host.");
            await _webHost.StartAsync(cancellationToken);
            _logger.LogDebug("Executing background web host callback.");
            _webHostInitializedCallback?.Invoke(_webHost);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("Terminating background web host.");
            
            try
            {
                _webHost.StopAsync(cancellationToken).Wait(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Exception occurred during web host termination.");
            }
            finally
            {
                _webHost.Dispose();
            }
            
            return Task.CompletedTask;
        }
    }
}