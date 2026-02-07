using Oqtane.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace GIBS.Module.PhotoGallery.Models
{
    [Table("GIBSPhotoGallery_PhotoTags")]
    public class PhotoTags : ModelBase
    {
        [Key]
        public int PhotoTagId { get; set; }
        public int PhotoId { get; set; }
        public int TagId { get; set; }
    }
}
