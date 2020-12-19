using System;
using System.IO;
using System.Windows;
using Windows.ApplicationModel;
using Windows.Data.Xml.Dom;
using Windows.Storage;
using Windows.UI.Notifications;
using Microsoft.Web.WebView2.Core;
using WpfHybridApp.Abstractions;
using WpfHybridApp.Services;
using static WpfHybridApp.Extensions.UwpPackageDetection;

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

            
            // DEVNOTE cannot use ApplicationData.Current.LocalFolder.Path, ref: https://github.com/microsoft/ProjectReunion/issues/101#issuecomment-705890839
            // TODO need better WPF local data location
            var userDataFolder = IsRunningAsUwp
                ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Package.Current.Id.Name, "WebView2_Cache")
                : @"C:\Temp\WebView2_Cache";

            var environment = await CoreWebView2Environment.CreateAsync(null, userDataFolder, options);

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
            Close();
        }

        private void fileSampleToastMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (!IsRunningAsUwp) return;

            var title = "The current time is";
            var timeString = $"{DateTime.Now:HH:mm:ss}";
            var thomasImage = "https://via.placeholder.com/300";

            var toastXmlString =
                $@"<toast><visual>
            <binding template='ToastGeneric'>
            <text>{title}</text>
            <text>{timeString}</text>
            <image src='{thomasImage}'/>
            </binding>
        </visual></toast>";

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(toastXmlString);

            var toastNotification = new ToastNotification(xmlDoc);

            var toastNotifier = ToastNotificationManager.CreateToastNotifier();
            toastNotifier.Show(toastNotification);
        }
    }
}