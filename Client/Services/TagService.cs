using GIBS.Module.PhotoGallery.Models;
using Oqtane.Services;
using Oqtane.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace GIBS.Module.PhotoGallery.Services
{
    public interface ITagService
    {
        Task<List<Tags>> GetTagsAsync(int ModuleId);
        Task<Tags> GetTagAsync(int tagId, int ModuleId);
        Task<Tags> AddTagAsync(Tags tag);
        Task<Tags> UpdateTagAsync(Tags tag);
        Task DeleteTagAsync(int tagId, int ModuleId);
    }

    public class TagService : ServiceBase, ITagService
    {
        public TagService(HttpClient http, SiteState siteState) : base(http, siteState) { }

        private string Apiurl => CreateApiUrl("Tags");

        public async Task<List<Tags>> GetTagsAsync(int ModuleId)
        {
            List<Tags> tags = await GetJsonAsync<List<Tags>>(CreateAuthorizationPolicyUrl($"{Apiurl}?moduleid={ModuleId}", EntityNames.Module, ModuleId), Enumerable.Empty<Tags>().ToList());
            return tags.OrderBy(item => item.TagName).ToList();
        }

        public async Task<Tags> GetTagAsync(int tagId, int ModuleId)
        {
            return await GetJsonAsync<Tags>(CreateAuthorizationPolicyUrl($"{Apiurl}/{tagId}/{ModuleId}", EntityNames.Module, ModuleId));
        }

        public async Task<Tags> AddTagAsync(Tags tag)
        {
            return await PostJsonAsync<Tags>(CreateAuthorizationPolicyUrl($"{Apiurl}", EntityNames.Module, tag.ModuleId), tag);
        }

        public async Task<Tags> UpdateTagAsync(Tags tag)
        {
            return await PutJsonAsync<Tags>(CreateAuthorizationPolicyUrl($"{Apiurl}/{tag.TagId}", EntityNames.Module, tag.ModuleId), tag);
        }

        public async Task DeleteTagAsync(int tagId, int ModuleId)
        {
            await DeleteAsync(CreateAuthorizationPolicyUrl($"{Apiurl}/{tagId}/{ModuleId}", EntityNames.Module, ModuleId));
        }
    }
}
