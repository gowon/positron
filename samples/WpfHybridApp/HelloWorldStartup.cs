using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace WpfHybridApp
{
    public class HelloWorldStartup
    {
        public void Configure(IApplicationBuilder app)
        {
            app.Run(async context => { await context.Response.WriteAsync("Hello, World!"); });
        }
    }
}