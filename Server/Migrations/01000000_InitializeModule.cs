using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Oqtane.Databases.Interfaces;
using Oqtane.Migrations;
using GIBS.Module.PhotoGallery.Migrations.EntityBuilders;
using GIBS.Module.PhotoGallery.Repository;

namespace GIBS.Module.PhotoGallery.Migrations
{
    [DbContext(typeof(PhotoGalleryContext))]
    [Migration("GIBS.Module.PhotoGallery.01.00.00.00")]
    public class InitializeModule : MultiDatabaseMigration
    {
        public InitializeModule(IDatabase database) : base(database)
        {
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var albumEntityBuilder = new AlbumEntityBuilder(migrationBuilder, ActiveDatabase);
            albumEntityBuilder.Create();

            var photoEntityBuilder = new PhotoEntityBuilder(migrationBuilder, ActiveDatabase); 
            photoEntityBuilder.Create();

            var tagsEntityBuilder = new TagsEntityBuilder(migrationBuilder, ActiveDatabase);
            tagsEntityBuilder.Create();

            var photoTagsEntityBuilder = new PhotoTagsEntityBuilder(migrationBuilder, ActiveDatabase);
            photoTagsEntityBuilder.Create();
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var photoTagsEntityBuilder = new PhotoTagsEntityBuilder(migrationBuilder, ActiveDatabase);
            photoTagsEntityBuilder.Drop();

            var tagsEntityBuilder = new TagsEntityBuilder(migrationBuilder, ActiveDatabase);
            tagsEntityBuilder.Drop();

            var photoEntityBuilder = new PhotoEntityBuilder(migrationBuilder, ActiveDatabase);
            photoEntityBuilder.Drop();

            var albumEntityBuilder = new AlbumEntityBuilder(migrationBuilder, ActiveDatabase);
            albumEntityBuilder.Drop();
        }
    }
}
