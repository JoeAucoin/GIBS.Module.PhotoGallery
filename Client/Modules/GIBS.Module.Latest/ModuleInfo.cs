using Oqtane.Models;
using Oqtane.Modules;

namespace GIBS.Module.PhotoGallery.Latest
{
    public class ModuleInfo : IModule
    {
        public ModuleDefinition ModuleDefinition => new ModuleDefinition
        {
            Name = "PhotoGallery Latest",
            Description = "Widgets For Displaying Latest Items in Oqtane Photo Gallery",
            Version = "1.0.1",
            ServerManagerType = "GIBS.Module.PhotoGallery.Manager.PhotoGalleryManager, GIBS.Module.PhotoGallery.Server.Oqtane",
            ReleaseVersions = "1.0.0,1.0.1",
            Dependencies = "GIBS.Module.PhotoGallery.Shared.Oqtane",
            PackageName = "GIBS.Module.PhotoGallery" 
        };
    }
}
