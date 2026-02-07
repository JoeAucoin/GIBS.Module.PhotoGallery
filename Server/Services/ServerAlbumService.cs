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
    public class ServerAlbumService : IAlbumService
    {
        private readonly IAlbumRepository _albumRepository;
        private readonly IUserPermissions _userPermissions;
        private readonly ILogManager _logger;
        private readonly IHttpContextAccessor _accessor;
        private readonly Alias _alias;

        public ServerAlbumService(IAlbumRepository albumRepository, IUserPermissions userPermissions, ITenantManager tenantManager, ILogManager logger, IHttpContextAccessor accessor)
        {
            _albumRepository = albumRepository;
            _userPermissions = userPermissions;
            _logger = logger;
            _accessor = accessor;
            _alias = tenantManager.GetAlias();
        }

        public Task<List<Models.Album>> GetAlbumsAsync(int ModuleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, ModuleId, PermissionNames.View))
            {
                return Task.FromResult(_albumRepository.GetAlbums(ModuleId).ToList());
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized PhotoGallery Get Attempt {ModuleId}", ModuleId);
                return null;
            }
        }

        public Task<Models.Album> GetAlbumAsync(int AlbumId, int ModuleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, ModuleId, PermissionNames.View))
            {
                return Task.FromResult(_albumRepository.GetAlbum(AlbumId));
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized PhotoGallery Get Attempt {AlbumId} {ModuleId}", AlbumId, ModuleId);
                return null;
            }
        }

        public Task<Models.Album> AddAlbumAsync(Models.Album Album)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, Album.ModuleId, PermissionNames.Edit))
            {
                Album = _albumRepository.AddAlbum(Album);
                _logger.Log(LogLevel.Information, this, LogFunction.Create, "PhotoGallery Added {Album}", Album);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized PhotoGallery Add Attempt {Album}", Album);
                Album = null;
            }
            return Task.FromResult(Album);
        }

        public Task<Models.Album> UpdateAlbumAsync(Models.Album Album)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, Album.ModuleId, PermissionNames.Edit))
            {
                Album = _albumRepository.UpdateAlbum(Album);
                _logger.Log(LogLevel.Information, this, LogFunction.Update, "PhotoGallery Updated {Album}", Album);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized PhotoGallery Update Attempt {Album}", Album);
                Album = null;
            }
            return Task.FromResult(Album);
        }

        public Task DeleteAlbumAsync(int AlbumId, int ModuleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, ModuleId, PermissionNames.Edit))
            {
                _albumRepository.DeleteAlbum(AlbumId);
                _logger.Log(LogLevel.Information, this, LogFunction.Delete, "PhotoGallery Deleted {AlbumId}", AlbumId);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized PhotoGallery Delete Attempt {AlbumId} {ModuleId}", AlbumId, ModuleId);
            }
            return Task.CompletedTask;
        }
    }
}
