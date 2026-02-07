using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using Oqtane.Databases.Interfaces;
using Oqtane.Migrations;
using Oqtane.Migrations.EntityBuilders;

namespace GIBS.Module.PhotoGallery.Migrations.EntityBuilders
{
    public class PhotoEntityBuilder : AuditableBaseEntityBuilder<PhotoEntityBuilder>
    {
        private const string _entityTableName = "GIBSPhotoGallery_Photo";
        private readonly PrimaryKey<PhotoEntityBuilder> _primaryKey = new("PK_GIBSPhotoGallery_Photo", x => x.PhotoId);
        private readonly ForeignKey<PhotoEntityBuilder> _moduleForeignKey = new("FK_GIBSPhotoGallery_Photo_Module", x => x.ModuleId, "Module", "ModuleId", ReferentialAction.Cascade);
        private readonly ForeignKey<PhotoEntityBuilder> _albumForeignKey = new("FK_GIBSPhotoGallery_Photo_Album", x => x.AlbumId, "GIBSPhotoGallery_Album", "AlbumId", ReferentialAction.Restrict);
        public PhotoEntityBuilder(MigrationBuilder migrationBuilder, IDatabase database) : base(migrationBuilder, database)
        {
            EntityTableName = _entityTableName;
            PrimaryKey = _primaryKey;
            ForeignKeys.Add(_moduleForeignKey);
            ForeignKeys.Add(_albumForeignKey);
        }
        protected override PhotoEntityBuilder BuildTable(ColumnsBuilder table)
        {
            PhotoId = AddAutoIncrementColumn(table, "PhotoId");
            ModuleId = AddIntegerColumn(table, "ModuleId");
            AlbumId = AddIntegerColumn(table, "AlbumId", true);
            Title = AddStringColumn(table, "Title", 200);
            Description = AddStringColumn(table, "Description", 2000, true);
            MediaType = AddStringColumn(table, "MediaType", 20);
            FileId = AddIntegerColumn(table, "FileId");
            FilePath = AddStringColumn(table, "FilePath", 500);
            ThumbnailFileId = AddIntegerColumn(table, "ThumbnailFileId", true);
            ThumbnailPath = AddStringColumn(table, "ThumbnailPath", 500, true);
            SortOrder = AddIntegerColumn(table, "SortOrder", false, 0);
            IsActive = AddBooleanColumn(table, "IsActive", false, true);
            ViewCount = AddIntegerColumn(table, "ViewCount", false, 0);
            AddAuditableColumns(table);
            return this;
        }
        public OperationBuilder<AddColumnOperation> PhotoId { get; set; }
        public OperationBuilder<AddColumnOperation> ModuleId { get; set; }
        public OperationBuilder<AddColumnOperation> AlbumId { get; set; }
        public OperationBuilder<AddColumnOperation> Title { get; set; }
        public OperationBuilder<AddColumnOperation> Description { get; set; }
        public OperationBuilder<AddColumnOperation> MediaType { get; set; }
        public OperationBuilder<AddColumnOperation> FileId { get; set; }
        public OperationBuilder<AddColumnOperation> FilePath { get; set; }
        public OperationBuilder<AddColumnOperation> ThumbnailFileId { get; set; }
        public OperationBuilder<AddColumnOperation> ThumbnailPath { get; set; }
        public OperationBuilder<AddColumnOperation> SortOrder { get; set; }
        public OperationBuilder<AddColumnOperation> IsActive { get; set; }
        public OperationBuilder<AddColumnOperation> ViewCount { get; set; }
    }
}
