using GIBS.Module.PhotoGallery.Models;
using Oqtane.Services;
using Oqtane.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace GIBS.Module.PhotoGallery.Services
{
    public interface IPhotoService 
    {
        Task<List<PhotoAlbum>> GetPhotoAlbumsAsync(int ModuleId);
        Task<List<Photo>> GetPhotosAsync(int ModuleId);
        Task<Photo> GetPhotoAsync(int photoId, int ModuleId);
        Task<Photo> AddPhotoAsync(Photo photo);
        Task<Photo> UpdatePhotoAsync(Photo photo);
        Task DeletePhotoAsync(int photoId, int ModuleId);
        Task<ThumbnailResponse> CreateImageThumbnailAsync(int fileId, int width, int height, int moduleId);
        Task<ThumbnailResponse> ResizeImageAsync(int fileId, int width, int height, int moduleId);
    }
    
    public class PhotoService : ServiceBase, IPhotoService
    {
        private readonly HttpClient _httpClient;
        public PhotoService(HttpClient http, SiteState siteState) : base(http, siteState) 
        {
            _httpClient = http ?? throw new ArgumentNullException(nameof(http), "HttpClient is not initialized.");
        }

        private string Apiurl => CreateApiUrl("Photo");
        private string AlbumApiurl => CreateApiUrl("PhotoAlbum");

        public async Task<List<PhotoAlbum>> GetPhotoAlbumsAsync(int ModuleId)
        {
            return await GetJsonAsync<List<PhotoAlbum>>(CreateAuthorizationPolicyUrl($"{AlbumApiurl}?moduleid={ModuleId}", EntityNames.Module, ModuleId));
        }

        public async Task<List<Photo>> GetPhotosAsync(int ModuleId)
        {
            List<Photo> photos = await GetJsonAsync<List<Photo>>(CreateAuthorizationPolicyUrl($"{Apiurl}?moduleid={ModuleId}", EntityNames.Module, ModuleId));
            return photos.OrderBy(item => item.SortOrder).ToList();
        }

        public async Task<Photo> GetPhotoAsync(int photoId, int ModuleId)
        {
            return await GetJsonAsync<Photo>(CreateAuthorizationPolicyUrl($"{Apiurl}/{photoId}/{ModuleId}", EntityNames.Module, ModuleId));
        }

        public async Task<Photo> AddPhotoAsync(Photo photo)
        {
            return await PostJsonAsync<Photo>(CreateAuthorizationPolicyUrl($"{Apiurl}", EntityNames.Module, photo.ModuleId), photo);
        }

        public async Task<Photo> UpdatePhotoAsync(Photo photo)
        {
            return await PutJsonAsync<Photo>(CreateAuthorizationPolicyUrl($"{Apiurl}/{photo.PhotoId}", EntityNames.Module, photo.ModuleId), photo);
        }

        public async Task DeletePhotoAsync(int photoId, int ModuleId)
        {
            await DeleteAsync(CreateAuthorizationPolicyUrl($"{Apiurl}/{photoId}/{ModuleId}", EntityNames.Module, ModuleId));
        }

        public async Task<ThumbnailResponse> CreateImageThumbnailAsync(int fileId, int width, int height, int moduleId)
        {
            var request = new ResizeRequest { FileId = fileId, Width = width, Height = height, ModuleId = moduleId };
            var url = CreateAuthorizationPolicyUrl($"{Apiurl}/resize-image-thumbnail", EntityNames.Module, moduleId);

            try
            {
                var response = await _httpClient.PostAsJsonAsync(url, request);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<ThumbnailResponse>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating thumbnail: {ex.Message}");
                return null;
            }
        }

        public async Task<ThumbnailResponse> ResizeImageAsync(int fileId, int width, int height, int moduleId)
        {
            var request = new ResizeRequest { FileId = fileId, Width = width, Height = height, ModuleId = moduleId };
            var url = CreateAuthorizationPolicyUrl($"{Apiurl}/resize-image", EntityNames.Module, moduleId);

            try
            {
                var response = await _httpClient.PostAsJsonAsync(url, request);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<ThumbnailResponse>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error resizing image: {ex.Message}");
                return null;
            }
        }
    }
}
