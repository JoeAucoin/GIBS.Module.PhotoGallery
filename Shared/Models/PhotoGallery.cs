using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oqtane.Models;

namespace GIBS.Module.PhotoGallery.Models
{
    [Table("GIBSPhotoGallery")]
    public class PhotoGallery : ModelBase
    {
        [Key]
        public int PhotoGalleryId { get; set; }
        public int ModuleId { get; set; }
        public string Name { get; set; }
    }
}
