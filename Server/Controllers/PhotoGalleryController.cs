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

namespace GIBS.Module.PhotoGallery.Controllers
{
    [Route(ControllerRoutes.ApiRoute)]
    public class PhotoGalleryController : ModuleControllerBase
    {
        private readonly IPhotoGalleryService _PhotoGalleryService;

        public PhotoGalleryController(IPhotoGalleryService PhotoGalleryService, ILogManager logger, IHttpContextAccessor accessor) : base(logger, accessor)
        {
            _PhotoGalleryService = PhotoGalleryService;
        }

        // GET: api/<controller>?moduleid=x
        [HttpGet]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<IEnumerable<Models.PhotoGallery>> Get(string moduleid)
        {
            int ModuleId;
            if (int.TryParse(moduleid, out ModuleId) && IsAuthorizedEntityId(EntityNames.Module, ModuleId))
            {
                return await _PhotoGalleryService.GetPhotoGallerysAsync(ModuleId);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized PhotoGallery Get Attempt {ModuleId}", moduleid);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }
        }

        // GET api/<controller>/5
        [HttpGet("{id}/{moduleid}")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<Models.PhotoGallery> Get(int id, int moduleid)
        {
            Models.PhotoGallery PhotoGallery = await _PhotoGalleryService.GetPhotoGalleryAsync(id, moduleid);
            if (PhotoGallery != null && IsAuthorizedEntityId(EntityNames.Module, PhotoGallery.ModuleId))
            {
                return PhotoGallery;
            }
            else
            { 
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized PhotoGallery Get Attempt {PhotoGalleryId} {ModuleId}", id, moduleid);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }
        }

        // POST api/<controller>
        [HttpPost]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<Models.PhotoGallery> Post([FromBody] Models.PhotoGallery PhotoGallery)
        {
            if (ModelState.IsValid && IsAuthorizedEntityId(EntityNames.Module, PhotoGallery.ModuleId))
            {
                PhotoGallery = await _PhotoGalleryService.AddPhotoGalleryAsync(PhotoGallery);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized PhotoGallery Post Attempt {PhotoGallery}", PhotoGallery);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                PhotoGallery = null;
            }
            return PhotoGallery;
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<Models.PhotoGallery> Put(int id, [FromBody] Models.PhotoGallery PhotoGallery)
        {
            if (ModelState.IsValid && PhotoGallery.PhotoGalleryId == id && IsAuthorizedEntityId(EntityNames.Module, PhotoGallery.ModuleId))
            {
                PhotoGallery = await _PhotoGalleryService.UpdatePhotoGalleryAsync(PhotoGallery);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized PhotoGallery Put Attempt {PhotoGallery}", PhotoGallery);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                PhotoGallery = null;
            }
            return PhotoGallery;
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}/{moduleid}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task Delete(int id, int moduleid)
        {
            Models.PhotoGallery PhotoGallery = await _PhotoGalleryService.GetPhotoGalleryAsync(id, moduleid);
            if (PhotoGallery != null && IsAuthorizedEntityId(EntityNames.Module, PhotoGallery.ModuleId))
            {
                await _PhotoGalleryService.DeletePhotoGalleryAsync(id, PhotoGallery.ModuleId);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized PhotoGallery Delete Attempt {PhotoGalleryId} {ModuleId}", id, moduleid);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            }
        }
    }
}
