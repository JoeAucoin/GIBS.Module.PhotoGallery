using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Oqtane.Modules;
using Oqtane.Repository;
using Oqtane.Infrastructure;
using Oqtane.Repository.Databases.Interfaces;

namespace GIBS.Module.PhotoGallery.Repository
{
    public class PhotoGalleryContext : DBContextBase, ITransientService, IMultiDatabase
    {
        public virtual DbSet<Models.Album> PhotoGallery { get; set; }
        public virtual DbSet<Models.Photo> Photos { get; set; }
        public virtual DbSet<Models.Tags> Tags { get; set; }
        public virtual DbSet<Models.PhotoTags> PhotoTags { get; set; }

        public PhotoGalleryContext(IDBContextDependencies DBContextDependencies) : base(DBContextDependencies)
        {
            // ContextBase handles multi-tenant database connections
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Models.Album>().ToTable(ActiveDatabase.RewriteName("GIBSPhotoGallery_Album"));
            builder.Entity<Models.Photo>().ToTable(ActiveDatabase.RewriteName("GIBSPhotoGallery_Photo"));
            builder.Entity<Models.Tags>().ToTable(ActiveDatabase.RewriteName("GIBSPhotoGallery_Tags"));
            builder.Entity<Models.PhotoTags>().ToTable(ActiveDatabase.RewriteName("GIBSPhotoGallery_PhotoTags"));
        }
    }
}
