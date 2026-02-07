using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Oqtane.Shared;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using GIBS.Module.PhotoGallery.Services;
using Oqtane.Controllers;
using System.Net;
using System.Threading.Tasks;
using GIBS.Module.PhotoGallery.Models;

namespace GIBS.Module.PhotoGallery.Controllers
{
    [Route(ControllerRoutes.ApiRoute)]
    public class PhotoAlbumController : ModuleControllerBase
    {
        private readonly IPhotoService _photoService;

        public PhotoAlbumController(IPhotoService photoService, ILogManager logger, IHttpContextAccessor accessor) : base(logger, accessor)
        {
            _photoService = photoService;
        }

        [HttpGet]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<IEnumerable<PhotoAlbum>> Get(string moduleid)
        {
            int ModuleId;
            if (int.TryParse(moduleid, out ModuleId) && IsAuthorizedEntityId(EntityNames.Module, ModuleId))
            {
                return await _photoService.GetPhotoAlbumsAsync(ModuleId);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized PhotoAlbum Get Attempt {ModuleId}", moduleid);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }
        }
    }
}
