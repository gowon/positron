using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Positron.Core
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBackgroundWebHost(this IServiceCollection services,
            Func<IServiceProvider, IWebHostBuilder, IWebHostBuilder> webHostBuilder,
            Action<IServiceProvider, IWebHost> webHostInitializedCallback = null)
        {
            webHostBuilder = webHostBuilder ?? throw new ArgumentNullException(nameof(webHostBuilder));

            return services.AddHostedService(provider =>
            {
                var logger = provider.GetRequiredService<ILogger<BackgroundWebHostService>>();
                var builder = WebHost.CreateDefaultBuilder();
                
                return new BackgroundWebHostService(webHostBuilder.Invoke(provider, builder), logger,
                    host => webHostInitializedCallback?.Invoke(provider, host));
            });
        }

        public static IServiceCollection AddBackgroundWebHost(this IServiceCollection services,
            Func<IServiceProvider, IWebHostBuilder, IWebHostBuilder> webHostBuilder,
            Action<IWebHost> webHostInitializedCallback = null)
        {
            return services.AddBackgroundWebHost(webHostBuilder,
                (provider, host) => webHostInitializedCallback?.Invoke(host));
        }

        public static IServiceCollection AddBackgroundWebHost(this IServiceCollection services,
            Func<IWebHostBuilder, IWebHostBuilder> webHostBuilder,
            Action<IServiceProvider, IWebHost> webHostInitializedCallback = null)
        {
            return services.AddBackgroundWebHost((provider, builder) => webHostBuilder.Invoke(builder),
                webHostInitializedCallback);
        }

        public static IServiceCollection AddBackgroundWebHost(this IServiceCollection services,
            Func<IWebHostBuilder, IWebHostBuilder> webHostBuilder,
            Action<IWebHost> webHostInitializedCallback = null)
        {
            return services.AddBackgroundWebHost((provider, builder) => webHostBuilder.Invoke(builder),
                (provider, host) => webHostInitializedCallback?.Invoke(host));
        }
    }
}