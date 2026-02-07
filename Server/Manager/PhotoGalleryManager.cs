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
        private readonly IAlbumRepository _albumRepository;
        private readonly IDBContextDependencies _DBContextDependencies;

        public PhotoGalleryManager(IAlbumRepository albumRepository, IDBContextDependencies DBContextDependencies)
        {
            _albumRepository = albumRepository;
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
            List<Models.Album> PhotoGallerys = _albumRepository.GetAlbums(module.ModuleId).ToList();
            if (PhotoGallerys != null)
            {
                content = JsonSerializer.Serialize(PhotoGallerys);
            }
            return content;
        }

        public void ImportModule(Oqtane.Models.Module module, string content, string version)
        {
            List<Models.Album> PhotoGallerys = null;
            if (!string.IsNullOrEmpty(content))
            {
                PhotoGallerys = JsonSerializer.Deserialize<List<Models.Album>>(content);
            }
            if (PhotoGallerys != null)
            {
                foreach(var PhotoGallery in PhotoGallerys)
                {
                    _albumRepository.AddAlbum(new Models.Album { ModuleId = module.ModuleId, AlbumName = PhotoGallery.AlbumName });
                }
            }
        }

        public Task<List<SearchContent>> GetSearchContentsAsync(PageModule pageModule, DateTime lastIndexedOn)
        {
           var searchContentList = new List<SearchContent>();

           foreach (var PhotoGallery in _albumRepository.GetAlbums(pageModule.ModuleId))
           {
               if (PhotoGallery.ModifiedOn >= lastIndexedOn)
               {
                   searchContentList.Add(new SearchContent
                   {
                       EntityName = "GIBSPhotoGallery",
                       EntityId = PhotoGallery.AlbumId.ToString(),
                       Title = PhotoGallery.AlbumName,
                       Body = PhotoGallery.AlbumName,
                       ContentModifiedBy = PhotoGallery.ModifiedBy,
                       ContentModifiedOn = PhotoGallery.ModifiedOn
                   });
               }
           }

           return Task.FromResult(searchContentList);
        }
    }
}
