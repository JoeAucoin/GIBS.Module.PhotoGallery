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
    public class PhotoTagsController : ModuleControllerBase
    {
        private readonly IPhotoTagService _photoTagService;

        public PhotoTagsController(IPhotoTagService photoTagService, ILogManager logger, IHttpContextAccessor accessor) : base(logger, accessor)
        {
            _photoTagService = photoTagService;
        }

        [HttpGet]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<IEnumerable<PhotoTags>> Get(string photoid, string moduleid)
        {
            int PhotoId;
            int ModuleId;
            if (int.TryParse(photoid, out PhotoId) && int.TryParse(moduleid, out ModuleId) && IsAuthorizedEntityId(EntityNames.Module, ModuleId))
            {
                return await _photoTagService.GetPhotoTagsAsync(PhotoId, ModuleId);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized PhotoTags Get Attempt {PhotoId} {ModuleId}", photoid, moduleid);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }
        }

        [HttpGet("{id}/{moduleid}")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<PhotoTags> Get(int id, int moduleid)
        {
            PhotoTags photoTag = await _photoTagService.GetPhotoTagAsync(id, moduleid);
            if (photoTag != null)
            {
                return photoTag;
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized PhotoTag Get Attempt {PhotoTagId} {ModuleId}", id, moduleid);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }
        }

        [HttpPost]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<PhotoTags> Post([FromBody] PhotoTags photoTag, [FromQuery] int moduleid)
        {
            if (ModelState.IsValid && IsAuthorizedEntityId(EntityNames.Module, moduleid))
            {
                photoTag = await _photoTagService.AddPhotoTagAsync(photoTag, moduleid);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized PhotoTag Post Attempt {PhotoTag}", photoTag);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                photoTag = null;
            }
            return photoTag;
        }

        [HttpPut("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<PhotoTags> Put(int id, [FromBody] PhotoTags photoTag, [FromQuery] int moduleid)
        {
            if (ModelState.IsValid && photoTag.PhotoTagId == id && IsAuthorizedEntityId(EntityNames.Module, moduleid))
            {
                photoTag = await _photoTagService.UpdatePhotoTagAsync(photoTag, moduleid);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized PhotoTag Put Attempt {PhotoTag}", photoTag);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                photoTag = null;
            }
            return photoTag;
        }

        [HttpDelete("{id}/{moduleid}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task Delete(int id, int moduleid)
        {
            await _photoTagService.DeletePhotoTagAsync(id, moduleid);
        }
    }
}
