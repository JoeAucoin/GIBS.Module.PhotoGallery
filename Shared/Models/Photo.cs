using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oqtane.Models;

namespace GIBS.Module.PhotoGallery.Models
{
    [Table("GIBSPhotoGallery_Photo")]
    public class Photo : ModelBase
    {
        [Key]
        public int PhotoId { get; set; }
        public int ModuleId { get; set; }
        public int? AlbumId { get; set; }
        [Required]
        [StringLength(200)]
        public string Title { get; set; }
        [StringLength(2000)]
        public string Description { get; set; }
        [Required]
        [StringLength(20)]
        public string MediaType { get; set; }
        public int FileId { get; set; }
        [Required]
        [StringLength(500)]
        public string FilePath { get; set; }
        public int? ThumbnailFileId { get; set; }
        [StringLength(500)]
        public string ThumbnailPath { get; set; }
        public int SortOrder { get; set; } = 0;
        public bool IsActive { get; set; } = true;
        public int ViewCount { get; set; } = 0;

        [NotMapped]
        public string AlbumName { get; set; } // Not mapped, for display only
        [NotMapped]
        public string Tags { get; set; }
    }
}
