using Oqtane.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GIBS.Module.PhotoGallery.Models
{
    [NotMapped]
    public class PhotoAlbum : ModelBase
    {
        public int AlbumId { get; set; }
        public string AlbumName { get; set; }
        public string FilePath { get; set; } 
        public string ThumbnailPath { get; set; }
        public string Description { get; set; }
        public int ItemCount { get; set; }
    }
}