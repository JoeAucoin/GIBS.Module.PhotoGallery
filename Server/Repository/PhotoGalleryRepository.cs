using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using Oqtane.Modules;

namespace GIBS.Module.PhotoGallery.Repository
{
    public interface IPhotoGalleryRepository
    {
        IEnumerable<Models.PhotoGallery> GetPhotoGallerys(int ModuleId);
        Models.PhotoGallery GetPhotoGallery(int PhotoGalleryId);
        Models.PhotoGallery GetPhotoGallery(int PhotoGalleryId, bool tracking);
        Models.PhotoGallery AddPhotoGallery(Models.PhotoGallery PhotoGallery);
        Models.PhotoGallery UpdatePhotoGallery(Models.PhotoGallery PhotoGallery);
        void DeletePhotoGallery(int PhotoGalleryId);
    }

    public class PhotoGalleryRepository : IPhotoGalleryRepository, ITransientService
    {
        private readonly IDbContextFactory<PhotoGalleryContext> _factory;

        public PhotoGalleryRepository(IDbContextFactory<PhotoGalleryContext> factory)
        {
            _factory = factory;
        }

        public IEnumerable<Models.PhotoGallery> GetPhotoGallerys(int ModuleId)
        {
            using var db = _factory.CreateDbContext();
            return db.PhotoGallery.Where(item => item.ModuleId == ModuleId).ToList();
        }

        public Models.PhotoGallery GetPhotoGallery(int PhotoGalleryId)
        {
            return GetPhotoGallery(PhotoGalleryId, true);
        }

        public Models.PhotoGallery GetPhotoGallery(int PhotoGalleryId, bool tracking)
        {
            using var db = _factory.CreateDbContext();
            if (tracking)
            {
                return db.PhotoGallery.Find(PhotoGalleryId);
            }
            else
            {
                return db.PhotoGallery.AsNoTracking().FirstOrDefault(item => item.PhotoGalleryId == PhotoGalleryId);
            }
        }

        public Models.PhotoGallery AddPhotoGallery(Models.PhotoGallery PhotoGallery)
        {
            using var db = _factory.CreateDbContext();
            db.PhotoGallery.Add(PhotoGallery);
            db.SaveChanges();
            return PhotoGallery;
        }

        public Models.PhotoGallery UpdatePhotoGallery(Models.PhotoGallery PhotoGallery)
        {
            using var db = _factory.CreateDbContext();
            db.Entry(PhotoGallery).State = EntityState.Modified;
            db.SaveChanges();
            return PhotoGallery;
        }

        public void DeletePhotoGallery(int PhotoGalleryId)
        {
            using var db = _factory.CreateDbContext();
            Models.PhotoGallery PhotoGallery = db.PhotoGallery.Find(PhotoGalleryId);
            db.PhotoGallery.Remove(PhotoGallery);
            db.SaveChanges();
        }
    }
}
