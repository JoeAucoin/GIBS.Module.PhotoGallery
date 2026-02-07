using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using Oqtane.Models;
using Oqtane.Security;
using Oqtane.Shared;
using GIBS.Module.PhotoGallery.Repository;

namespace GIBS.Module.PhotoGallery.Services
{
    public class ServerTagService : ITagService
    {
        private readonly ITagRepository _tagRepository;
        private readonly IUserPermissions _userPermissions;
        private readonly ILogManager _logger;
        private readonly IHttpContextAccessor _accessor;
        private readonly Alias _alias;

        public ServerTagService(ITagRepository tagRepository, IUserPermissions userPermissions, ITenantManager tenantManager, ILogManager logger, IHttpContextAccessor accessor)
        {
            _tagRepository = tagRepository;
            _userPermissions = userPermissions;
            _logger = logger;
            _accessor = accessor;
            _alias = tenantManager.GetAlias();
        }

        public Task<List<Models.Tags>> GetTagsAsync(int ModuleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, ModuleId, PermissionNames.View))
            {
                return Task.FromResult(_tagRepository.GetTags(ModuleId).ToList());
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Tags Get Attempt {ModuleId}", ModuleId);
                return null;
            }
        }

        public Task<Models.Tags> GetTagAsync(int tagId, int ModuleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, ModuleId, PermissionNames.View))
            {
                return Task.FromResult(_tagRepository.GetTag(tagId));
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Tags Get Attempt {TagId} {ModuleId}", tagId, ModuleId);
                return null;
            }
        }

        public Task<Models.Tags> AddTagAsync(Models.Tags tag)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, tag.ModuleId, PermissionNames.Edit))
            {
                tag = _tagRepository.AddTag(tag);
                _logger.Log(LogLevel.Information, this, LogFunction.Create, "Tag Added {Tag}", tag);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Tag Add Attempt {Tag}", tag);
                tag = null;
            }
            return Task.FromResult(tag);
        }

        public Task<Models.Tags> UpdateTagAsync(Models.Tags tag)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, tag.ModuleId, PermissionNames.Edit))
            {
                tag = _tagRepository.UpdateTag(tag);
                _logger.Log(LogLevel.Information, this, LogFunction.Update, "Tag Updated {Tag}", tag);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Tag Update Attempt {Tag}", tag);
                tag = null;
            }
            return Task.FromResult(tag);
        }

        public Task DeleteTagAsync(int tagId, int ModuleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, ModuleId, PermissionNames.Edit))
            {
                _tagRepository.DeleteTag(tagId);
                _logger.Log(LogLevel.Information, this, LogFunction.Delete, "Tag Deleted {TagId}", tagId);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Tag Delete Attempt {TagId} {ModuleId}", tagId, ModuleId);
            }
            return Task.CompletedTask;
        }
    }
}
