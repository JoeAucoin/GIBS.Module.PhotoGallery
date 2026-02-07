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
        private readonly IAlbumService _albumService;

        public PhotoGalleryController(IAlbumService albumService, ILogManager logger, IHttpContextAccessor accessor) : base(logger, accessor)
        {
            _albumService = albumService;
        }

        // GET: api/<controller>?moduleid=x
        [HttpGet]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<IEnumerable<Models.Album>> Get(string moduleid)
        {
            int ModuleId;
            if (int.TryParse(moduleid, out ModuleId) && IsAuthorizedEntityId(EntityNames.Module, ModuleId))
            {
                return await _albumService.GetAlbumsAsync(ModuleId);
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
        public async Task<Models.Album> Get(int id, int moduleid)
        {
            Models.Album PhotoGallery = await _albumService.GetAlbumAsync(id, moduleid);
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
        public async Task<Models.Album> Post([FromBody] Models.Album PhotoGallery)
        {
            if (ModelState.IsValid && IsAuthorizedEntityId(EntityNames.Module, PhotoGallery.ModuleId))
            {
                PhotoGallery = await _albumService.AddAlbumAsync(PhotoGallery);
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
        public async Task<Models.Album> Put(int id, [FromBody] Models.Album PhotoGallery)
        {
            if (ModelState.IsValid && PhotoGallery.AlbumId == id && IsAuthorizedEntityId(EntityNames.Module, PhotoGallery.ModuleId))
            {
                PhotoGallery = await _albumService.UpdateAlbumAsync(PhotoGallery);
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
            Models.Album PhotoGallery = await _albumService.GetAlbumAsync(id, moduleid);
            if (PhotoGallery != null && IsAuthorizedEntityId(EntityNames.Module, PhotoGallery.ModuleId))
            {
                await _albumService.DeleteAlbumAsync(id, PhotoGallery.ModuleId);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized PhotoGallery Delete Attempt {PhotoGalleryId} {ModuleId}", id, moduleid);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            }
        }
    }
}
