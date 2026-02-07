using GIBS.Module.PhotoGallery.Models;
using Oqtane.Services;
using Oqtane.Shared;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace GIBS.Module.PhotoGallery.Services
{
    public interface IPhotoTagService
    {
        Task<List<PhotoTags>> GetPhotoTagsAsync(int photoId, int moduleId);
        Task<PhotoTags> GetPhotoTagAsync(int photoTagId, int moduleId);
        Task<PhotoTags> AddPhotoTagAsync(PhotoTags photoTag, int moduleId);
        Task<PhotoTags> UpdatePhotoTagAsync(PhotoTags photoTag, int moduleId);
        Task DeletePhotoTagAsync(int photoTagId, int moduleId);
    }

    public class PhotoTagService : ServiceBase, IPhotoTagService
    {
        public PhotoTagService(HttpClient http, SiteState siteState) : base(http, siteState) { }

        private string Apiurl => CreateApiUrl("PhotoTags");

        public async Task<List<PhotoTags>> GetPhotoTagsAsync(int photoId, int moduleId)
        {
            return await GetJsonAsync<List<PhotoTags>>(CreateAuthorizationPolicyUrl($"{Apiurl}?photoid={photoId}&moduleid={moduleId}", EntityNames.Module, moduleId));
        }

        public async Task<PhotoTags> GetPhotoTagAsync(int photoTagId, int moduleId)
        {
            return await GetJsonAsync<PhotoTags>(CreateAuthorizationPolicyUrl($"{Apiurl}/{photoTagId}/{moduleId}", EntityNames.Module, moduleId));
        }

        public async Task<PhotoTags> AddPhotoTagAsync(PhotoTags photoTag, int moduleId)
        {
            return await PostJsonAsync<PhotoTags>(CreateAuthorizationPolicyUrl($"{Apiurl}", EntityNames.Module, moduleId), photoTag);
        }

        public async Task<PhotoTags> UpdatePhotoTagAsync(PhotoTags photoTag, int moduleId)
        {
            return await PutJsonAsync<PhotoTags>(CreateAuthorizationPolicyUrl($"{Apiurl}/{photoTag.PhotoTagId}", EntityNames.Module, moduleId), photoTag);
        }

        public async Task DeletePhotoTagAsync(int photoTagId, int moduleId)
        {
            await DeleteAsync(CreateAuthorizationPolicyUrl($"{Apiurl}/{photoTagId}/{moduleId}", EntityNames.Module, moduleId));
        }
    }
}
