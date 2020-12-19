using System;
using System.Net;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Positron.Core;
using SampleMvcApplication;
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
                }).ConfigureServices((context, services) =>
                {
                    // add services

                    services.AddSingleton<ApplicationOptions>();

                    services.AddSingleton<Func<IWebHost>>(provider =>
                    {
                        return () => WebHost.CreateDefaultBuilder().ConfigureWebHost().Build();
                    });

                    services.AddSingleton<WebHostPortCallback>(provider =>
                    {
                        var options = provider.GetRequiredService<ApplicationOptions>();
                        return port => options.HostPort = port;
                    });

                    services.AddHostedService<BackgroundWebHostService>();

                    services.AddScoped<ISampleService, SampleService>();

                    services.AddSingleton<MainWindow>();
                })
                .ConfigureLogging(loggingBuilder =>
                {
                    loggingBuilder.ClearProviders();
                    loggingBuilder.AddDebug();
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