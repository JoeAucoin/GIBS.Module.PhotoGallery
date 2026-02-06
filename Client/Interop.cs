using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace GIBS.Module.PhotoGallery
{
    public class Interop
    {
        private readonly IJSRuntime _jsRuntime;

        public Interop(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }
    }
}
