using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Oqtane.Modules;

namespace GIBS.Module.PhotoGallery.Repository
{
    public interface IPhotoTagRepository
    {
        IEnumerable<Models.PhotoTags> GetPhotoTags(int PhotoId);
        Models.PhotoTags GetPhotoTag(int PhotoTagId);
        Models.PhotoTags GetPhotoTag(int PhotoTagId, bool tracking);
        Models.PhotoTags AddPhotoTag(Models.PhotoTags PhotoTag);
        Models.PhotoTags UpdatePhotoTag(Models.PhotoTags PhotoTag);
        void DeletePhotoTag(int PhotoTagId);
    }

    public class PhotoTagRepository : IPhotoTagRepository, ITransientService
    {
        private readonly IDbContextFactory<PhotoGalleryContext> _factory;

        public PhotoTagRepository(IDbContextFactory<PhotoGalleryContext> factory)
        {
            _factory = factory;
        }

        public IEnumerable<Models.PhotoTags> GetPhotoTags(int PhotoId)
        {
            using var db = _factory.CreateDbContext();
            return db.PhotoTags.Where(item => item.PhotoId == PhotoId).ToList();
        }

        public Models.PhotoTags GetPhotoTag(int PhotoTagId)
        {
            return GetPhotoTag(PhotoTagId, true);
        }

        public Models.PhotoTags GetPhotoTag(int PhotoTagId, bool tracking)
        {
            using var db = _factory.CreateDbContext();
            if (tracking)
            {
                return db.PhotoTags.Find(PhotoTagId);
            }
            else
            {
                return db.PhotoTags.AsNoTracking().FirstOrDefault(item => item.PhotoTagId == PhotoTagId);
            }
        }

        public Models.PhotoTags AddPhotoTag(Models.PhotoTags PhotoTag)
        {
            using var db = _factory.CreateDbContext();
            db.PhotoTags.Add(PhotoTag);
            db.SaveChanges();
            return PhotoTag;
        }

        public Models.PhotoTags UpdatePhotoTag(Models.PhotoTags PhotoTag)
        {
            using var db = _factory.CreateDbContext();
            db.Entry(PhotoTag).State = EntityState.Modified;
            db.SaveChanges();
            return PhotoTag;
        }

        public void DeletePhotoTag(int PhotoTagId)
        {
            using var db = _factory.CreateDbContext();
            Models.PhotoTags PhotoTag = db.PhotoTags.Find(PhotoTagId);
            db.PhotoTags.Remove(PhotoTag);
            db.SaveChanges();
        }
    }
}
