using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using Oqtane.Databases.Interfaces;
using Oqtane.Migrations;
using Oqtane.Migrations.EntityBuilders;

namespace GIBS.Module.PhotoGallery.Migrations.EntityBuilders
{
    public class AlbumEntityBuilder : AuditableBaseEntityBuilder<AlbumEntityBuilder>
    {
        private const string _entityTableName = "GIBSPhotoGallery_Album";
        private readonly PrimaryKey<AlbumEntityBuilder> _primaryKey = new("PK_GIBSPhotoGallery_Album", x => x.AlbumId);
        private readonly ForeignKey<AlbumEntityBuilder> _moduleForeignKey = new("FK_GIBSPhotoGallery_Album_Module", x => x.ModuleId, "Module", "ModuleId", ReferentialAction.Cascade);

        public AlbumEntityBuilder(MigrationBuilder migrationBuilder, IDatabase database) : base(migrationBuilder, database)
        {
            EntityTableName = _entityTableName;
            PrimaryKey = _primaryKey;
            ForeignKeys.Add(_moduleForeignKey);
        }

        protected override AlbumEntityBuilder BuildTable(ColumnsBuilder table)
        {
            AlbumId = AddAutoIncrementColumn(table, "AlbumId");
            ModuleId = AddIntegerColumn(table,"ModuleId");
            AlbumName = AddStringColumn(table,"AlbumName", 100);
            Description = AddStringColumn(table,"Description", 500, true);
            SortOrder = AddIntegerColumn(table,"SortOrder", false, 0);
            IsActive = AddBooleanColumn(table,"IsActive", false, true);
            ParentAlbumId = AddIntegerColumn(table,"ParentAlbumId", true);
            AddAuditableColumns(table);
            return this;
        }

        public OperationBuilder<AddColumnOperation> AlbumId { get; set; }
        public OperationBuilder<AddColumnOperation> ModuleId { get; set; }
        public OperationBuilder<AddColumnOperation> AlbumName { get; set; }
        public OperationBuilder<AddColumnOperation> Description { get; set; }
        public OperationBuilder<AddColumnOperation> SortOrder { get; set; }
        public OperationBuilder<AddColumnOperation> IsActive { get; set; }
        public OperationBuilder<AddColumnOperation> ParentAlbumId { get; set; }

    }
}
