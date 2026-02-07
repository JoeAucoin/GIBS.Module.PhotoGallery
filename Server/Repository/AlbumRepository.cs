using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using Oqtane.Modules;

namespace GIBS.Module.PhotoGallery.Repository
{
    public interface IAlbumRepository
    {
        IEnumerable<Models.Album> GetAlbums(int ModuleId);
        Models.Album GetAlbum(int AlbumId);
        Models.Album GetAlbum(int AlbumId, bool tracking);
        Models.Album AddAlbum(Models.Album Album);
        Models.Album UpdateAlbum(Models.Album Album);
        void DeleteAlbum(int AlbumId);
    }

    public class AlbumRepository : IAlbumRepository, ITransientService
    {
        private readonly IDbContextFactory<PhotoGalleryContext> _factory;

        public AlbumRepository(IDbContextFactory<PhotoGalleryContext> factory)
        {
            _factory = factory;
        }

        public IEnumerable<Models.Album> GetAlbums(int ModuleId)
        {
            using var db = _factory.CreateDbContext();
            return db.PhotoGallery.Where(item => item.ModuleId == ModuleId).ToList();
        }

        public Models.Album GetAlbum(int AlbumId)
        {
            return GetAlbum(AlbumId, true);
        }

        public Models.Album GetAlbum(int AlbumId, bool tracking)
        {
            using var db = _factory.CreateDbContext();
            if (tracking)
            {
                return db.PhotoGallery.Find(AlbumId);
            }
            else
            {
                return db.PhotoGallery.AsNoTracking().FirstOrDefault(item => item.AlbumId == AlbumId);
            }
        }

        public Models.Album AddAlbum(Models.Album Album)
        {
            using var db = _factory.CreateDbContext();
            db.PhotoGallery.Add(Album);
            db.SaveChanges();
            return Album;
        }

        public Models.Album UpdateAlbum(Models.Album Album)
        {
            using var db = _factory.CreateDbContext();
            db.Entry(Album).State = EntityState.Modified;
            db.SaveChanges();
            return Album;
        }

        public void DeleteAlbum(int AlbumId)
        {
            using var db = _factory.CreateDbContext();
            Models.Album Album = db.PhotoGallery.Find(AlbumId);
            db.PhotoGallery.Remove(Album);
            db.SaveChanges();
        }
    }
}
