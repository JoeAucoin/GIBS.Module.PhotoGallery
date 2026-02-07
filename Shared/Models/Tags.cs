using Oqtane.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GIBS.Module.PhotoGallery.Models
{
    [Table("GIBSPhotoGallery_Tags")]
    public class Tags : ModelBase
    {
        [Key]
        public int TagId { get; set; }
        public int ModuleId { get; set; }
        [Required]
        [StringLength(100)]
        public string TagName { get; set; }

    }
}