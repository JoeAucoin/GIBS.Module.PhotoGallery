using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using Oqtane.Databases.Interfaces;
using Oqtane.Migrations;
using Oqtane.Migrations.EntityBuilders;

namespace GIBS.Module.PhotoGallery.Migrations.EntityBuilders
{
    public class PhotoTagsEntityBuilder : AuditableBaseEntityBuilder<PhotoTagsEntityBuilder>
    {
        private const string _entityTableName = "GIBSPhotoGallery_PhotoTags";
        private readonly PrimaryKey<PhotoTagsEntityBuilder> _primaryKey = new("PK_GIBSPhotoGallery_PhotoTags", x => x.PhotoTagId);
        private readonly ForeignKey<PhotoTagsEntityBuilder> _photoForeignKey = new("FK_GIBSPhotoGallery_PhotoTags_Photo", x => x.PhotoId, "GIBSPhotoGallery_Photo", "PhotoId", ReferentialAction.NoAction);
        private readonly ForeignKey<PhotoTagsEntityBuilder> _tagForeignKey = new("FK_GIBSPhotoGallery_PhotoTags_Tag", x => x.TagId, "GIBSPhotoGallery_Tags", "TagId", ReferentialAction.Cascade);

        public PhotoTagsEntityBuilder(MigrationBuilder migrationBuilder, IDatabase database) : base(migrationBuilder, database)
        {
            EntityTableName = _entityTableName;
            PrimaryKey = _primaryKey;
            ForeignKeys.Add(_photoForeignKey);
            ForeignKeys.Add(_tagForeignKey);
        }

        protected override PhotoTagsEntityBuilder BuildTable(ColumnsBuilder table)
        {
            PhotoTagId = AddAutoIncrementColumn(table, "PhotoTagId");
            PhotoId = AddIntegerColumn(table, "PhotoId");
            TagId = AddIntegerColumn(table, "TagId");
            AddAuditableColumns(table);
            return this;
        }

        public OperationBuilder<AddColumnOperation> PhotoTagId { get; set; }
        public OperationBuilder<AddColumnOperation> PhotoId { get; set; }
        public OperationBuilder<AddColumnOperation> TagId { get; set; }
    }
}
