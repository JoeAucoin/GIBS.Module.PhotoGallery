using Oqtane.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace GIBS.Module.PhotoGallery.Models
{
    public class ThumbnailResponse
    {
        public int FileId { get; set; }
        public string Url { get; set; }
    }
}
