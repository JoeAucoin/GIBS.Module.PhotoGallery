using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Oqtane.Services;
using Oqtane.Shared;

namespace GIBS.Module.PhotoGallery.Services
{
    public interface IPhotoGalleryService 
    {
        Task<List<Models.PhotoGallery>> GetPhotoGallerysAsync(int ModuleId);

        Task<Models.PhotoGallery> GetPhotoGalleryAsync(int PhotoGalleryId, int ModuleId);

        Task<Models.PhotoGallery> AddPhotoGalleryAsync(Models.PhotoGallery PhotoGallery);

        Task<Models.PhotoGallery> UpdatePhotoGalleryAsync(Models.PhotoGallery PhotoGallery);

        Task DeletePhotoGalleryAsync(int PhotoGalleryId, int ModuleId);
    }

    public class PhotoGalleryService : ServiceBase, IPhotoGalleryService
    {
        public PhotoGalleryService(HttpClient http, SiteState siteState) : base(http, siteState) { }

        private string Apiurl => CreateApiUrl("PhotoGallery");

        public async Task<List<Models.PhotoGallery>> GetPhotoGallerysAsync(int ModuleId)
        {
            List<Models.PhotoGallery> PhotoGallerys = await GetJsonAsync<List<Models.PhotoGallery>>(CreateAuthorizationPolicyUrl($"{Apiurl}?moduleid={ModuleId}", EntityNames.Module, ModuleId), Enumerable.Empty<Models.PhotoGallery>().ToList());
            return PhotoGallerys.OrderBy(item => item.Name).ToList();
        }

        public async Task<Models.PhotoGallery> GetPhotoGalleryAsync(int PhotoGalleryId, int ModuleId)
        {
            return await GetJsonAsync<Models.PhotoGallery>(CreateAuthorizationPolicyUrl($"{Apiurl}/{PhotoGalleryId}/{ModuleId}", EntityNames.Module, ModuleId));
        }

        public async Task<Models.PhotoGallery> AddPhotoGalleryAsync(Models.PhotoGallery PhotoGallery)
        {
            return await PostJsonAsync<Models.PhotoGallery>(CreateAuthorizationPolicyUrl($"{Apiurl}", EntityNames.Module, PhotoGallery.ModuleId), PhotoGallery);
        }

        public async Task<Models.PhotoGallery> UpdatePhotoGalleryAsync(Models.PhotoGallery PhotoGallery)
        {
            return await PutJsonAsync<Models.PhotoGallery>(CreateAuthorizationPolicyUrl($"{Apiurl}/{PhotoGallery.PhotoGalleryId}", EntityNames.Module, PhotoGallery.ModuleId), PhotoGallery);
        }

        public async Task DeletePhotoGalleryAsync(int PhotoGalleryId, int ModuleId)
        {
            await DeleteAsync(CreateAuthorizationPolicyUrl($"{Apiurl}/{PhotoGalleryId}/{ModuleId}", EntityNames.Module, ModuleId));
        }
    }
}
