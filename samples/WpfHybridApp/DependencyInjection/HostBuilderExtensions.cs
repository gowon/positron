using System;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Positron.Core;
using SampleStaticServer;
using WpfHybridApp.Abstractions;
using WpfHybridApp.Services;

namespace WpfHybridApp.DependencyInjection
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder ConfigureHost(this IHostBuilder builder)
        {
            return builder.ConfigureAppConfiguration((context, configurationBuilder) =>
                {
                    var args = Environment.GetCommandLineArgs();
                    configurationBuilder
                        // Add configuration from the appsettings.json file.
                        .AddJsonFile("app.json", true, false)
                        // Add configuration specific to the Development, Staging or Production environments. This config can
                        // be stored on the machine being deployed to or if you are using Azure, in the cloud. These settings
                        // override the ones in all of the above config files. See
                        // http://docs.asp.net/en/latest/security/app-secrets.html
                        .AddEnvironmentVariables()
                        // Add command line options. These take the highest priority.
                        .AddCommandLine(args);
                })
                .ConfigureServices((context, services) =>
                {
                    // add services

                    services.AddSingleton<ApplicationOptions>();

                    services.AddBackgroundWebHost(webHostBuilder => webHostBuilder.ConfigureWebHost(),
                        (provider, host) =>
                        {
                            // get the port from the web host and update the application options
                            var options = provider.GetRequiredService<ApplicationOptions>();
                            var addressFeature = host.ServerFeatures.Get<IServerAddressesFeature>();
                            var port = Regex.Match(addressFeature.Addresses.First(),
                                @"(https?:\/\/.*):(\d*)").Groups[2].Value;
                            options.HostPort = int.Parse(port);
                        });

                    services.AddHostedService<BackgroundWebHostService>();

                    services.AddScoped<ISampleService, SampleService>();

                    services.AddSingleton<MainWindow>();
                })
                .ConfigureLogging(loggingBuilder =>
                {
                    loggingBuilder
                        .AddDebug()
                        .SetMinimumLevel(LogLevel.Debug);
                });
        }

        public static IWebHostBuilder ConfigureWebHost(this IWebHostBuilder builder)
        {
            return builder.UseKestrel(options =>
                {
                    //options.Listen(IPAddress.Loopback, 0);
                    options.Listen(IPAddress.Loopback, 0,
                        listenOptions => listenOptions.UseHttps());
                })
                .UseStartup<Startup>();
        }
    }
}