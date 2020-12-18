using System;
using System.Windows;
using Microsoft.Web.WebView2.Core;
using WpfHybridApp.Abstractions;
using WpfHybridApp.Services;

namespace WpfHybridApp
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ApplicationOptions _options;
        private readonly ISampleService _sampleService;

        public MainWindow(ApplicationOptions options, ISampleService sampleService)
        {
            InitializeComponent();

            _options = options ?? throw new NullReferenceException(nameof(options));
            _sampleService = sampleService ?? throw new NullReferenceException(nameof(options));

            InitializeAsync();
        }

        // Initialize the WebView.
        private async void InitializeAsync()
        {
            var options = new CoreWebView2EnvironmentOptions
            {
                AdditionalBrowserArguments = "--allow-insecure-localhost"
            };

            var environment = await CoreWebView2Environment.CreateAsync(options: options);

            // Initialize the WebView
            await webView.EnsureCoreWebView2Async(environment);

            webView.CoreWebView2.AddHostObjectToScript("sample", _sampleService);

            // Add a script to run when a page is loading
            await webView.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync(
                // This script just posts a message with the window's URL
                "console.log(window.document.URL);"
            );

            webView.Source = _options.HostAddress;
        }
    }
}