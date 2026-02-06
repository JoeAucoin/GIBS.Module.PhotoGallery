using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Oqtane.Services;
using GIBS.Module.PhotoGallery.Services;

namespace GIBS.Module.PhotoGallery.Startup
{
    public class ClientStartup : IClientStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            if (!services.Any(s => s.ServiceType == typeof(IPhotoGalleryService)))
            {
                services.AddScoped<IPhotoGalleryService, PhotoGalleryService>();
            }
        }
    }
}
