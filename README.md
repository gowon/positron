# Positron

Proof-of-concept Electron alternative using only .NET technologies.

## Components

### Blazor WebAssembly

### ASP.NET Core Static File Server

WebAssembly applications are static files that do not need any complex servers or server-side features. Kestrel can be used as a static file server to accomplish this.

#### Include WebAssembly artifacts as embedded resources

The Blazor WebAssembly application published artifacts will be added as embedded resources in the ASP.NET Core project. To preserve folder heirarchy, a `ManifestEmbeddedFileProvider` will be used instead of the `EmbeddedFileProvider`.

```xml
<PropertyGroup>
    <GenerateEmbeddedFilesManifest>true</GenerateEmbeddedFilesManifest>
</PropertyGroup>
```

> Use the [Microsoft.Extensions.FileProviders.Embedded](https://www.nuget.org/packages/Microsoft.Extensions.FileProviders.Embedded/) NuGet package.

#### Disabling executable generation

Disable generation of the executable for ASP.NET Core projects by adding the following to the csproj:

```xml
<PropertyGroup>
  <UseAppHost>false</UseAppHost>
</PropertyGroup>
```

> See <https://docs.microsoft.com/en-us/dotnet/core/project-sdk/msbuild-props#useapphost>

### .NET 5 WPF Application

#### Using the Generic Host with WPF

```csharp
public partial class App : Application
{
    private readonly IHost _host;

    public App()
    {
        _host = Host.CreateDefaultBuilder()
            .ConfigureHost()
            .Build();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await _host.StartAsync();

        var mainWindow = _host.Services.GetRequiredService<MainWindow>();
        mainWindow.Show();

        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        using (_host)
        {
            await _host.StopAsync(TimeSpan.FromSeconds(5));
        }

        base.OnExit(e);
    }
}
```

#### Self-hosting Kestrel as a background service

The `BackgroundWebHostService` implements the [IHostedService](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-5.0&tabs=visual-studio#ihostedservice-interface) interface for background services. This class manages the IWebHost implementation in the background; the lifetime of the web host should completely overlap the lifetime of the WPF application.

#### WebView2 Browser Component

#### Javascript Interop

1. Javascript Post message is bound to a CQRS request object
2. Request is handled by Mediatr
3. A JSON response is asynchronously posted back to the DOM

- A Message discovery API will return all request objects in JSON schema format
- <https://github.com/korzio/djvi> can be used to create schema instances

### UWP and WinRT

#### Desktop Bridge

#### Detecting WinRT environment

- <https://www.thomasclaudiushuber.com/2019/04/26/calling-windows-10-apis-from-your-wpf-application/>
- <https://github.com/thomasclaudiushuber/Wpf-Calling-Win10-WinRT-Toast-Api>
- <https://stackoverflow.com/a/65044923/7644876>

## References

- <https://github.com/zeppaman/jSOS>
- <https://marcominerva.wordpress.com/2019/11/07/update-on-using-hostbuilder-dependency-injection-and-service-provider-with-net-core-3-0-wpf-applications/>
- <https://github.com/SirRufo/HostedWpf>
- <https://github.com/MicrosoftEdge/WebView2Samples/tree/master/SampleApps/WebView2WpfBrowser>
- <https://github.com/MicrosoftEdge/WebView2Feedback/issues/295>
- <https://github.com/githubcatw/SurfView>
- <https://laurentkempe.com/2019/09/03/WPF-and-dotnet-Generic-Host-with-dotnet-Core-3-0/>
- <https://github.com/dapplo/Dapplo.Microsoft.Extensions.Hosting>
- <https://github.com/mortenbrudvik/KioskBrowser>
- <https://docs.microsoft.com/en-us/dotnet/api/microsoft.web.webview2.core.corewebview2.addhostobjecttoscript?view=WebView2-dotnet-1.0.664.37>
- <https://www.thomasclaudiushuber.com/2020/02/18/hosting-blazor-app-in-winui-3-with-webview-2-and-call-blazor-component-method-from-winui/>
