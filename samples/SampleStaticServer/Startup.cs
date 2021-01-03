using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace SampleStaticServer
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            var fileProvider = new ManifestEmbeddedFileProvider(GetType().Assembly, "dist");

            app.UseDefaultFiles(new DefaultFilesOptions
            {
                FileProvider = fileProvider
            });

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = fileProvider,
                ServeUnknownFileTypes = true
            });
        }
    }
}