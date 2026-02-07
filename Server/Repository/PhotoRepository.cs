using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Oqtane.Modules;

namespace GIBS.Module.PhotoGallery.Repository
{
    public interface IPhotoRepository
    {
        IEnumerable<Models.Photo> GetPhotos(int ModuleId);
        Models.Photo GetPhoto(int PhotoId);
        Models.Photo GetPhoto(int PhotoId, bool tracking);
        Models.Photo AddPhoto(Models.Photo Photo);
        Models.Photo UpdatePhoto(Models.Photo Photo);
        void DeletePhoto(int PhotoId);
    }

    public class PhotoRepository : IPhotoRepository, ITransientService
    {
        private readonly IDbContextFactory<PhotoGalleryContext> _factory;

        public PhotoRepository(IDbContextFactory<PhotoGalleryContext> factory)
        {
            _factory = factory;
        }

        public IEnumerable<Models.Photo> GetPhotos(int ModuleId)
        {
            using var db = _factory.CreateDbContext();
            return db.Photos.Where(item => item.ModuleId == ModuleId).ToList();
        }

        public Models.Photo GetPhoto(int PhotoId)
        {
            return GetPhoto(PhotoId, true);
        }

        public Models.Photo GetPhoto(int PhotoId, bool tracking)
        {
            using var db = _factory.CreateDbContext();
            if (tracking)
            {
                return db.Photos.Find(PhotoId);
            }
            else
            {
                return db.Photos.AsNoTracking().FirstOrDefault(item => item.PhotoId == PhotoId);
            }
        }

        public Models.Photo AddPhoto(Models.Photo Photo)
        {
            using var db = _factory.CreateDbContext();
            db.Photos.Add(Photo);
            db.SaveChanges();
            return Photo;
        }

        public Models.Photo UpdatePhoto(Models.Photo Photo)
        {
            using var db = _factory.CreateDbContext();
            db.Entry(Photo).State = EntityState.Modified;
            db.SaveChanges();
            return Photo;
        }

        public void DeletePhoto(int PhotoId)
        {
            using var db = _factory.CreateDbContext();
            Models.Photo Photo = db.Photos.Find(PhotoId);
            db.Photos.Remove(Photo);
            db.SaveChanges();
        }
    }
}
