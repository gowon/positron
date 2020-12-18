using System;

namespace WpfHybridApp.Abstractions
{
    public class ApplicationOptions
    {
        public int HostPort { get; set; }

        public Uri HostAddress => new Uri($"https://localhost:{HostPort}", UriKind.Absolute);
    }
}