using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Oqtane.Services;
using Oqtane.Shared;

namespace GIBS.Module.PhotoGallery.Services
{
    public interface IAlbumService 
    {
        Task<List<Models.Album>> GetAlbumsAsync(int ModuleId);

        Task<Models.Album> GetAlbumAsync(int AlbumId, int ModuleId);
        Task<Models.Album> AddAlbumAsync(Models.Album Album);

        Task<Models.Album> UpdateAlbumAsync(Models.Album Album);

        Task DeleteAlbumAsync(int AlbumId, int ModuleId);
    }

    public class AlbumService : ServiceBase, IAlbumService
    {
        public AlbumService(HttpClient http, SiteState siteState) : base(http, siteState) { }

        private string Apiurl => CreateApiUrl("PhotoGallery");

        public async Task<List<Models.Album>> GetAlbumsAsync(int ModuleId)
        {
            List<Models.Album> Albums = await GetJsonAsync<List<Models.Album>>(CreateAuthorizationPolicyUrl($"{Apiurl}?moduleid={ModuleId}", EntityNames.Module, ModuleId), Enumerable.Empty<Models.Album>().ToList());
            return Albums.OrderBy(item => item.AlbumName).ToList();
        }

        public async Task<Models.Album> GetAlbumAsync(int AlbumId, int ModuleId)
        {
            return await GetJsonAsync<Models.Album>(CreateAuthorizationPolicyUrl($"{Apiurl}/{AlbumId}/{ModuleId}", EntityNames.Module, ModuleId));
        }

        public async Task<Models.Album> AddAlbumAsync(Models.Album Album)
        {
            return await PostJsonAsync<Models.Album>(CreateAuthorizationPolicyUrl($"{Apiurl}", EntityNames.Module, Album.ModuleId), Album);
        }

        public async Task<Models.Album> UpdateAlbumAsync(Models.Album Album)
        {
            return await PutJsonAsync<Models.Album>(CreateAuthorizationPolicyUrl($"{Apiurl}/{Album.AlbumId}", EntityNames.Module, Album.ModuleId), Album);
        }

        public async Task DeleteAlbumAsync(int AlbumId, int ModuleId)
        {
            await DeleteAsync(CreateAuthorizationPolicyUrl($"{Apiurl}/{AlbumId}/{ModuleId}", EntityNames.Module, ModuleId));
        }
    }
}
