namespace GIBS.Module.PhotoGallery.Models
{
    public class ResizeRequest
    {
        public int FileId { get; set; }
        public int PhotoId { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int ModuleId { get; set; }
    }
}
