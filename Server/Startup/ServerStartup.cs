using Microsoft.AspNetCore.Builder; 
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Oqtane.Infrastructure;
using GIBS.Module.PhotoGallery.Repository;
using GIBS.Module.PhotoGallery.Services;

namespace GIBS.Module.PhotoGallery.Startup
{
    public class ServerStartup : IServerStartup
    {
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // not implemented
        }

        public void ConfigureMvc(IMvcBuilder mvcBuilder)
        {
            // not implemented
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IPhotoGalleryService, ServerPhotoGalleryService>();
            services.AddDbContextFactory<PhotoGalleryContext>(opt => { }, ServiceLifetime.Transient);
        }
    }
}
