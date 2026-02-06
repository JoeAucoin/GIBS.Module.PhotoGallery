using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Oqtane.Modules;
using Oqtane.Models;
using Oqtane.Infrastructure;
using Oqtane.Interfaces;
using Oqtane.Enums;
using Oqtane.Repository;
using GIBS.Module.PhotoGallery.Repository;
using System.Threading.Tasks;

namespace GIBS.Module.PhotoGallery.Manager
{
    public class PhotoGalleryManager : MigratableModuleBase, IInstallable, IPortable, ISearchable
    {
        private readonly IPhotoGalleryRepository _PhotoGalleryRepository;
        private readonly IDBContextDependencies _DBContextDependencies;

        public PhotoGalleryManager(IPhotoGalleryRepository PhotoGalleryRepository, IDBContextDependencies DBContextDependencies)
        {
            _PhotoGalleryRepository = PhotoGalleryRepository;
            _DBContextDependencies = DBContextDependencies;
        }

        public bool Install(Tenant tenant, string version)
        {
            return Migrate(new PhotoGalleryContext(_DBContextDependencies), tenant, MigrationType.Up);
        }

        public bool Uninstall(Tenant tenant)
        {
            return Migrate(new PhotoGalleryContext(_DBContextDependencies), tenant, MigrationType.Down);
        }

        public string ExportModule(Oqtane.Models.Module module)
        {
            string content = "";
            List<Models.PhotoGallery> PhotoGallerys = _PhotoGalleryRepository.GetPhotoGallerys(module.ModuleId).ToList();
            if (PhotoGallerys != null)
            {
                content = JsonSerializer.Serialize(PhotoGallerys);
            }
            return content;
        }

        public void ImportModule(Oqtane.Models.Module module, string content, string version)
        {
            List<Models.PhotoGallery> PhotoGallerys = null;
            if (!string.IsNullOrEmpty(content))
            {
                PhotoGallerys = JsonSerializer.Deserialize<List<Models.PhotoGallery>>(content);
            }
            if (PhotoGallerys != null)
            {
                foreach(var PhotoGallery in PhotoGallerys)
                {
                    _PhotoGalleryRepository.AddPhotoGallery(new Models.PhotoGallery { ModuleId = module.ModuleId, Name = PhotoGallery.Name });
                }
            }
        }

        public Task<List<SearchContent>> GetSearchContentsAsync(PageModule pageModule, DateTime lastIndexedOn)
        {
           var searchContentList = new List<SearchContent>();

           foreach (var PhotoGallery in _PhotoGalleryRepository.GetPhotoGallerys(pageModule.ModuleId))
           {
               if (PhotoGallery.ModifiedOn >= lastIndexedOn)
               {
                   searchContentList.Add(new SearchContent
                   {
                       EntityName = "GIBSPhotoGallery",
                       EntityId = PhotoGallery.PhotoGalleryId.ToString(),
                       Title = PhotoGallery.Name,
                       Body = PhotoGallery.Name,
                       ContentModifiedBy = PhotoGallery.ModifiedBy,
                       ContentModifiedOn = PhotoGallery.ModifiedOn
                   });
               }
           }

           return Task.FromResult(searchContentList);
        }
    }
}
