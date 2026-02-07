using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Oqtane.Modules;

namespace GIBS.Module.PhotoGallery.Repository
{
    public interface ITagRepository
    {
        IEnumerable<Models.Tags> GetTags(int ModuleId);
        Models.Tags GetTag(int TagId);
        Models.Tags GetTag(int TagId, bool tracking);
        Models.Tags AddTag(Models.Tags Tag);
        Models.Tags UpdateTag(Models.Tags Tag);
        void DeleteTag(int TagId);
    }

    public class TagRepository : ITagRepository, ITransientService
    {
        private readonly IDbContextFactory<PhotoGalleryContext> _factory;

        public TagRepository(IDbContextFactory<PhotoGalleryContext> factory)
        {
            _factory = factory;
        }

        public IEnumerable<Models.Tags> GetTags(int ModuleId)
        {
            using var db = _factory.CreateDbContext();
            return db.Tags.Where(item => item.ModuleId == ModuleId).ToList();
        }

        public Models.Tags GetTag(int TagId)
        {
            return GetTag(TagId, true);
        }

        public Models.Tags GetTag(int TagId, bool tracking)
        {
            using var db = _factory.CreateDbContext();
            if (tracking)
            {
                return db.Tags.Find(TagId);
            }
            else
            {
                return db.Tags.AsNoTracking().FirstOrDefault(item => item.TagId == TagId);
            }
        }

        public Models.Tags AddTag(Models.Tags Tag)
        {
            using var db = _factory.CreateDbContext();
            db.Tags.Add(Tag);
            db.SaveChanges();
            return Tag;
        }

        public Models.Tags UpdateTag(Models.Tags Tag)
        {
            using var db = _factory.CreateDbContext();
            db.Entry(Tag).State = EntityState.Modified;
            db.SaveChanges();
            return Tag;
        }

        public void DeleteTag(int TagId)
        {
            using var db = _factory.CreateDbContext();
            Models.Tags Tag = db.Tags.Find(TagId);
            db.Tags.Remove(Tag);
            db.SaveChanges();
        }
    }
}
