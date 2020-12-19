using System;
using System.Windows;
using System.Windows.Input;
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
            await WebView.EnsureCoreWebView2Async(environment);

            WebView.CoreWebView2.AddHostObjectToScript("sample", _sampleService);

            // Add a script to run when a page is loading
            await WebView.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync(
                // This script just posts a message with the window's URL
                "console.log(window.document.URL);"
            );

            WebView.Source = _options.HostAddress;
        }

        private void fileDevToolsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            WebView.CoreWebView2.OpenDevToolsWindow();
        }

        private void fileExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Close this window
            Close();
        }
    }
}