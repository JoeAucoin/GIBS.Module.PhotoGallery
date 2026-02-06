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
        public virtual DbSet<Models.PhotoGallery> PhotoGallery { get; set; }

        public PhotoGalleryContext(IDBContextDependencies DBContextDependencies) : base(DBContextDependencies)
        {
            // ContextBase handles multi-tenant database connections
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Models.PhotoGallery>().ToTable(ActiveDatabase.RewriteName("GIBSPhotoGallery"));
        }
    }
}
