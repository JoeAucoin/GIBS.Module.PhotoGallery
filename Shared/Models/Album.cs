using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oqtane.Models;

namespace GIBS.Module.PhotoGallery.Models
{
    [Table("GIBSPhotoGallery_Album")]
    public class Album : ModelBase
    {
        [Key]
        public int AlbumId { get; set; }
        public int ModuleId { get; set; }
        public string AlbumName { get; set; }
        public string Description { get; set; }
        public int SortOrder { get; set; } = 0;
        public bool IsActive { get; set; } = true;
        public int? ParentAlbumId { get; set; }

    }
}
