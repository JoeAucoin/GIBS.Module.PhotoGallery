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
    public class TagsController : ModuleControllerBase
    {
        private readonly ITagService _tagService;

        public TagsController(ITagService tagService, ILogManager logger, IHttpContextAccessor accessor) : base(logger, accessor)
        {
            _tagService = tagService;
        }

        [HttpGet]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<IEnumerable<Tags>> Get(string moduleid)
        {
            int ModuleId;
            if (int.TryParse(moduleid, out ModuleId) && IsAuthorizedEntityId(EntityNames.Module, ModuleId))
            {
                return await _tagService.GetTagsAsync(ModuleId);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Tags Get Attempt {ModuleId}", moduleid);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }
        }

        [HttpGet("{id}/{moduleid}")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<Tags> Get(int id, int moduleid)
        {
            Tags tag = await _tagService.GetTagAsync(id, moduleid);
            if (tag != null && IsAuthorizedEntityId(EntityNames.Module, tag.ModuleId))
            {
                return tag;
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Tags Get Attempt {TagId} {ModuleId}", id, moduleid);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }
        }

        [HttpPost]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<Tags> Post([FromBody] Tags tag)
        {
            if (ModelState.IsValid && IsAuthorizedEntityId(EntityNames.Module, tag.ModuleId))
            {
                tag = await _tagService.AddTagAsync(tag);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Tag Post Attempt {Tag}", tag);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                tag = null;
            }
            return tag;
        }

        [HttpPut("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<Tags> Put(int id, [FromBody] Tags tag)
        {
            if (ModelState.IsValid && tag.TagId == id && IsAuthorizedEntityId(EntityNames.Module, tag.ModuleId))
            {
                tag = await _tagService.UpdateTagAsync(tag);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Tag Put Attempt {Tag}", tag);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                tag = null;
            }
            return tag;
        }

        [HttpDelete("{id}/{moduleid}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task Delete(int id, int moduleid)
        {
            Tags tag = await _tagService.GetTagAsync(id, moduleid);
            if (tag != null && IsAuthorizedEntityId(EntityNames.Module, tag.ModuleId))
            {
                await _tagService.DeleteTagAsync(id, tag.ModuleId);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Tag Delete Attempt {TagId} {ModuleId}", id, moduleid);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            }
        }
    }
}
