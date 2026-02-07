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
    public class ServerPhotoTagService : IPhotoTagService
    {
        private readonly IPhotoTagRepository _photoTagRepository;
        private readonly IPhotoRepository _photoRepository;
        private readonly IUserPermissions _userPermissions;
        private readonly ILogManager _logger;
        private readonly IHttpContextAccessor _accessor;
        private readonly Alias _alias;

        public ServerPhotoTagService(IPhotoTagRepository photoTagRepository, IPhotoRepository photoRepository, IUserPermissions userPermissions, ITenantManager tenantManager, ILogManager logger, IHttpContextAccessor accessor)
        {
            _photoTagRepository = photoTagRepository;
            _photoRepository = photoRepository;
            _userPermissions = userPermissions;
            _logger = logger;
            _accessor = accessor;
            _alias = tenantManager.GetAlias();
        }

        public Task<List<Models.PhotoTags>> GetPhotoTagsAsync(int photoId, int moduleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, moduleId, PermissionNames.View))
            {
                return Task.FromResult(_photoTagRepository.GetPhotoTags(photoId).ToList());
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized PhotoTags Get Attempt {PhotoId} {ModuleId}", photoId, moduleId);
                return null;
            }
        }

        public Task<Models.PhotoTags> GetPhotoTagAsync(int photoTagId, int moduleId)
        {
            if (!_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, moduleId, PermissionNames.View))
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized PhotoTag Get Attempt {PhotoTagId} {ModuleId}", photoTagId, moduleId);
                return null;
            }

            var photoTag = _photoTagRepository.GetPhotoTag(photoTagId);
            if (photoTag == null)
            {
                return Task.FromResult<Models.PhotoTags>(null);
            }

            var photo = _photoRepository.GetPhoto(photoTag.PhotoId);
            if (photo == null || photo.ModuleId != moduleId)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized PhotoTag Get Attempt {PhotoTagId} {ModuleId}", photoTagId, moduleId);
                return Task.FromResult<Models.PhotoTags>(null);
            }

            return Task.FromResult(photoTag);
        }

        public Task<Models.PhotoTags> AddPhotoTagAsync(Models.PhotoTags photoTag, int moduleId)
        {
            if (!_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, moduleId, PermissionNames.Edit))
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized PhotoTag Add Attempt {PhotoTag}", photoTag);
                return Task.FromResult<Models.PhotoTags>(null);
            }

            var photo = _photoRepository.GetPhoto(photoTag.PhotoId);
            if (photo == null || photo.ModuleId != moduleId)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized PhotoTag Add Attempt {PhotoTag}", photoTag);
                return Task.FromResult<Models.PhotoTags>(null);
            }

            photoTag = _photoTagRepository.AddPhotoTag(photoTag);
            _logger.Log(LogLevel.Information, this, LogFunction.Create, "PhotoTag Added {PhotoTag}", photoTag);
            return Task.FromResult(photoTag);
        }

        public Task<Models.PhotoTags> UpdatePhotoTagAsync(Models.PhotoTags photoTag, int moduleId)
        {
            if (!_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, moduleId, PermissionNames.Edit))
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized PhotoTag Update Attempt {PhotoTag}", photoTag);
                return Task.FromResult<Models.PhotoTags>(null);
            }

            var photo = _photoRepository.GetPhoto(photoTag.PhotoId);
            if (photo == null || photo.ModuleId != moduleId)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized PhotoTag Update Attempt {PhotoTag}", photoTag);
                return Task.FromResult<Models.PhotoTags>(null);
            }

            photoTag = _photoTagRepository.UpdatePhotoTag(photoTag);
            _logger.Log(LogLevel.Information, this, LogFunction.Update, "PhotoTag Updated {PhotoTag}", photoTag);
            return Task.FromResult(photoTag);
        }

        public Task DeletePhotoTagAsync(int photoTagId, int moduleId)
        {
            if (!_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, moduleId, PermissionNames.Edit))
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized PhotoTag Delete Attempt {PhotoTagId} {ModuleId}", photoTagId, moduleId);
                return Task.CompletedTask;
            }

            var photoTag = _photoTagRepository.GetPhotoTag(photoTagId);
            if (photoTag == null)
            {
                return Task.CompletedTask;
            }

            var photo = _photoRepository.GetPhoto(photoTag.PhotoId);
            if (photo == null || photo.ModuleId != moduleId)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized PhotoTag Delete Attempt {PhotoTagId} {ModuleId}", photoTagId, moduleId);
                return Task.CompletedTask;
            }

            _photoTagRepository.DeletePhotoTag(photoTagId);
            _logger.Log(LogLevel.Information, this, LogFunction.Delete, "PhotoTag Deleted {PhotoTagId}", photoTagId);
            return Task.CompletedTask;
        }
    }
}
