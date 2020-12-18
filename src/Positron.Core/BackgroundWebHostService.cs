using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Hosting;

namespace Positron.Core
{
    public delegate void WebHostPortCallback(int port);

    public class BackgroundWebHostService : IHostedService
    {
        private readonly IWebHost _webHost;
        private readonly WebHostPortCallback _webHostPortCallback;

        public BackgroundWebHostService(Func<IWebHost> webHostFactory, WebHostPortCallback webHostPortCallback = null)
        {
            _webHost = webHostFactory?.Invoke() ?? throw new ArgumentNullException(nameof(webHostFactory));
            _webHostPortCallback = webHostPortCallback;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _webHost.StartAsync(cancellationToken);

            var addressFeature = _webHost.ServerFeatures.Get<IServerAddressesFeature>();
            var port = Regex.Match(addressFeature.Addresses.First(),
                @"(https?:\/\/.*):(\d*)").Groups[2].Value;

            _webHostPortCallback.Invoke(int.Parse(port));
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            using (_webHost)
            {
                _webHost.StopAsync(cancellationToken).Wait(cancellationToken);
            }

            return Task.CompletedTask;
        }
    }
}