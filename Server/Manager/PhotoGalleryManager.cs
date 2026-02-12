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
    public class PhotoGalleryManager : MigratableModuleBase, IInstallable, IPortable, ISearchable, ISitemap
    {
        private readonly IAlbumRepository _albumRepository;
        private readonly IPhotoRepository _photoRepository; // Keep only the underscored version
        private readonly IDBContextDependencies _DBContextDependencies;

        public PhotoGalleryManager(IAlbumRepository albumRepository, IPhotoRepository photoRepository, IDBContextDependencies DBContextDependencies)
        {
            _albumRepository = albumRepository;
            _photoRepository = photoRepository; // Now correctly assigned from parameters
            _DBContextDependencies = DBContextDependencies;
        }

        public List<Sitemap> GetUrls(string alias, string path, Oqtane.Models.Module module)
        {
            var sitemapUrls = new List<Sitemap>();
            var albums = _albumRepository.GetAlbums(module.ModuleId);
            var photos = _photoRepository.GetPhotos(module.ModuleId);

            // 1. Fetch your custom module data (e.g., from a repository)
            // 2. Loop through your items and create Sitemap objects
            // 3. Example of adding a dynamic detail page:

            foreach (var album in albums)
            {

                sitemapUrls.Add(new Sitemap
                {
                    Url = $"{alias}/{path}?album={album.AlbumId}", // Construct the full URL
                    ModifiedOn = DateTime.UtcNow
                });
            }

            foreach (var photo in photos)
            {
                sitemapUrls.Add(new Sitemap
                {
                    Url = $"{alias}/{path}?album={photo.AlbumId}&photo={photo.PhotoId}", // Construct the full URL
                    ModifiedOn = DateTime.UtcNow
                });
            }
            return sitemapUrls;
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
