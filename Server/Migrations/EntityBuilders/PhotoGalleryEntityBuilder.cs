using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using Oqtane.Databases.Interfaces;
using Oqtane.Migrations;
using Oqtane.Migrations.EntityBuilders;

namespace GIBS.Module.PhotoGallery.Migrations.EntityBuilders
{
    public class PhotoGalleryEntityBuilder : AuditableBaseEntityBuilder<PhotoGalleryEntityBuilder>
    {
        private const string _entityTableName = "GIBSPhotoGallery";
        private readonly PrimaryKey<PhotoGalleryEntityBuilder> _primaryKey = new("PK_GIBSPhotoGallery", x => x.PhotoGalleryId);
        private readonly ForeignKey<PhotoGalleryEntityBuilder> _moduleForeignKey = new("FK_GIBSPhotoGallery_Module", x => x.ModuleId, "Module", "ModuleId", ReferentialAction.Cascade);

        public PhotoGalleryEntityBuilder(MigrationBuilder migrationBuilder, IDatabase database) : base(migrationBuilder, database)
        {
            EntityTableName = _entityTableName;
            PrimaryKey = _primaryKey;
            ForeignKeys.Add(_moduleForeignKey);
        }

        protected override PhotoGalleryEntityBuilder BuildTable(ColumnsBuilder table)
        {
            PhotoGalleryId = AddAutoIncrementColumn(table,"PhotoGalleryId");
            ModuleId = AddIntegerColumn(table,"ModuleId");
            Name = AddMaxStringColumn(table,"Name");
            AddAuditableColumns(table);
            return this;
        }

        public OperationBuilder<AddColumnOperation> PhotoGalleryId { get; set; }
        public OperationBuilder<AddColumnOperation> ModuleId { get; set; }
        public OperationBuilder<AddColumnOperation> Name { get; set; }
    }
}
