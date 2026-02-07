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
            if (!services.Any(s => s.ServiceType == typeof(IAlbumService)))
            {
                services.AddScoped<IAlbumService, AlbumService>();
            }

            if (!services.Any(s => s.ServiceType == typeof(IPhotoService)))
            {
                services.AddScoped<IPhotoService, PhotoService>();
            }

            if (!services.Any(s => s.ServiceType == typeof(ITagService)))
            {
                services.AddScoped<ITagService, TagService>();
            }

            if (!services.Any(s => s.ServiceType == typeof(IPhotoTagService)))
            {
                services.AddScoped<IPhotoTagService, PhotoTagService>();
            }
        }
    }
}
