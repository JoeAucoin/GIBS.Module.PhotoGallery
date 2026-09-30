using Oqtane.Models;
using Oqtane.Modules;

namespace GIBS.Module.PhotoGallery
{
    public class ModuleInfo : IModule
    {
        public ModuleDefinition ModuleDefinition => new ModuleDefinition
        {
            Name = "PhotoGallery",
            Description = "Oqtane Photo Gallery",
            Version = "1.0.2",
            ServerManagerType = "GIBS.Module.PhotoGallery.Manager.PhotoGalleryManager, GIBS.Module.PhotoGallery.Server.Oqtane",
            ReleaseVersions = "1.0.0,1.0.1,1.0.2",
            Dependencies = "GIBS.Module.PhotoGallery.Shared.Oqtane",
            PackageName = "GIBS.Module.PhotoGallery" 
        };
    }
}
