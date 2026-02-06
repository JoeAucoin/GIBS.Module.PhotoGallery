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
    public class ServerPhotoGalleryService : IPhotoGalleryService
    {
        private readonly IPhotoGalleryRepository _PhotoGalleryRepository;
        private readonly IUserPermissions _userPermissions;
        private readonly ILogManager _logger;
        private readonly IHttpContextAccessor _accessor;
        private readonly Alias _alias;

        public ServerPhotoGalleryService(IPhotoGalleryRepository PhotoGalleryRepository, IUserPermissions userPermissions, ITenantManager tenantManager, ILogManager logger, IHttpContextAccessor accessor)
        {
            _PhotoGalleryRepository = PhotoGalleryRepository;
            _userPermissions = userPermissions;
            _logger = logger;
            _accessor = accessor;
            _alias = tenantManager.GetAlias();
        }

        public Task<List<Models.PhotoGallery>> GetPhotoGallerysAsync(int ModuleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, ModuleId, PermissionNames.View))
            {
                return Task.FromResult(_PhotoGalleryRepository.GetPhotoGallerys(ModuleId).ToList());
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized PhotoGallery Get Attempt {ModuleId}", ModuleId);
                return null;
            }
        }

        public Task<Models.PhotoGallery> GetPhotoGalleryAsync(int PhotoGalleryId, int ModuleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, ModuleId, PermissionNames.View))
            {
                return Task.FromResult(_PhotoGalleryRepository.GetPhotoGallery(PhotoGalleryId));
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized PhotoGallery Get Attempt {PhotoGalleryId} {ModuleId}", PhotoGalleryId, ModuleId);
                return null;
            }
        }

        public Task<Models.PhotoGallery> AddPhotoGalleryAsync(Models.PhotoGallery PhotoGallery)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, PhotoGallery.ModuleId, PermissionNames.Edit))
            {
                PhotoGallery = _PhotoGalleryRepository.AddPhotoGallery(PhotoGallery);
                _logger.Log(LogLevel.Information, this, LogFunction.Create, "PhotoGallery Added {PhotoGallery}", PhotoGallery);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized PhotoGallery Add Attempt {PhotoGallery}", PhotoGallery);
                PhotoGallery = null;
            }
            return Task.FromResult(PhotoGallery);
        }

        public Task<Models.PhotoGallery> UpdatePhotoGalleryAsync(Models.PhotoGallery PhotoGallery)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, PhotoGallery.ModuleId, PermissionNames.Edit))
            {
                PhotoGallery = _PhotoGalleryRepository.UpdatePhotoGallery(PhotoGallery);
                _logger.Log(LogLevel.Information, this, LogFunction.Update, "PhotoGallery Updated {PhotoGallery}", PhotoGallery);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized PhotoGallery Update Attempt {PhotoGallery}", PhotoGallery);
                PhotoGallery = null;
            }
            return Task.FromResult(PhotoGallery);
        }

        public Task DeletePhotoGalleryAsync(int PhotoGalleryId, int ModuleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, ModuleId, PermissionNames.Edit))
            {
                _PhotoGalleryRepository.DeletePhotoGallery(PhotoGalleryId);
                _logger.Log(LogLevel.Information, this, LogFunction.Delete, "PhotoGallery Deleted {PhotoGalleryId}", PhotoGalleryId);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized PhotoGallery Delete Attempt {PhotoGalleryId} {ModuleId}", PhotoGalleryId, ModuleId);
            }
            return Task.CompletedTask;
        }
    }
}
