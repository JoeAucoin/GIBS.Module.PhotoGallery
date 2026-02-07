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
    public class PhotoController : ModuleControllerBase
    {
        private readonly IPhotoService _photoService;

        public PhotoController(IPhotoService photoService, ILogManager logger, IHttpContextAccessor accessor) : base(logger, accessor)
        {
            _photoService = photoService;
        }

        [HttpGet]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<IEnumerable<Photo>> Get(string moduleid)
        {
            int ModuleId;
            if (int.TryParse(moduleid, out ModuleId) && IsAuthorizedEntityId(EntityNames.Module, ModuleId))
            {
                return await _photoService.GetPhotosAsync(ModuleId);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Photo Get Attempt {ModuleId}", moduleid);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }
        }

        [HttpGet("{id}/{moduleid}")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<Photo> Get(int id, int moduleid)
        {
            Photo photo = await _photoService.GetPhotoAsync(id, moduleid);
            if (photo != null && IsAuthorizedEntityId(EntityNames.Module, photo.ModuleId))
            {
                return photo;
            }
            else
            { 
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Photo Get Attempt {PhotoId} {ModuleId}", id, moduleid);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }
        }

        [HttpPost]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<Photo> Post([FromBody] Photo photo)
        {
            if (ModelState.IsValid && IsAuthorizedEntityId(EntityNames.Module, photo.ModuleId))
            {
                photo = await _photoService.AddPhotoAsync(photo);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Photo Post Attempt {Photo}", photo);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                photo = null;
            }
            return photo;
        }

        [HttpPut("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<Photo> Put(int id, [FromBody] Photo photo)
        {
            if (ModelState.IsValid && photo.PhotoId == id && IsAuthorizedEntityId(EntityNames.Module, photo.ModuleId))
            {
                photo = await _photoService.UpdatePhotoAsync(photo);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Photo Put Attempt {Photo}", photo);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                photo = null;
            }
            return photo;
        }

        [HttpDelete("{id}/{moduleid}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task Delete(int id, int moduleid)
        {
            Photo photo = await _photoService.GetPhotoAsync(id, moduleid);
            if (photo != null && IsAuthorizedEntityId(EntityNames.Module, photo.ModuleId))
            {
                await _photoService.DeletePhotoAsync(id, photo.ModuleId);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Photo Delete Attempt {PhotoId} {ModuleId}", id, moduleid);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            }
        }

        [HttpPost("resize-image-thumbnail")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<ThumbnailResponse> CreateImageThumbnail([FromBody] ResizeRequest request)
        {
            if (IsAuthorizedEntityId(EntityNames.Module, request.ModuleId))
            {
                return await _photoService.CreateImageThumbnailAsync(request.FileId, request.Width, request.Height, request.ModuleId);
            }

            _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Photo Thumbnail Attempt {ModuleId}", request.ModuleId);
            HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            return null;
        }

        [HttpPost("resize-image")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<ThumbnailResponse> ResizeImage([FromBody] ResizeRequest request)
        {
            if (IsAuthorizedEntityId(EntityNames.Module, request.ModuleId))
            {
                return await _photoService.ResizeImageAsync(request.FileId, request.Width, request.Height, request.ModuleId);
            }

            _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Photo Resize Attempt {ModuleId}", request.ModuleId);
            HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            return null;
        }
    }
}
