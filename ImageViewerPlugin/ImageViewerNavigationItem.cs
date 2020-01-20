using System.ComponentModel.Composition;
using Common;
using ImageViewerPlugin.ViewModels;

namespace ImageViewerPlugin
{
    [Export(typeof(PluginNavigationItem))]
    public class ImageViewerNavigationItem : PluginNavigationItem
    {
        [ImportingConstructor]
        public ImageViewerNavigationItem(ImageViewerViewModel imageViewerViewModel)
        {
            MainViewModel = imageViewerViewModel;
            Name = "Image Viewer";
        }
    }
}
