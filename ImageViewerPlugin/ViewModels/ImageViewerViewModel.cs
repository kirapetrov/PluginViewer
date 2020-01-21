using Caliburn.Micro;
using Common;
using ImageViewerPlugin.Models;
using System.ComponentModel.Composition;

namespace ImageViewerPlugin.ViewModels
{
    [Export]
    public class ImageViewerViewModel : Screen, IPluginViewModel
    {
        private ImageInfo imageInfo;

        public string ImageLocation => imageInfo.Location;

        public int ImageWidth
        {
            get => imageInfo.Width;
            set
            {
                imageInfo.Width = value;
                NotifyOfPropertyChange(nameof(ImageWidth));
            }
        }

        public int ImageHeight
        {
            get => imageInfo.Height;
            set
            {
                imageInfo.Height = value;
                NotifyOfPropertyChange(nameof(ImageHeight));
            }
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            // т. к. в тестовом задании не оговаривалось как изображение может меняться, код в данном классе написан из предположения что "imageInfo" не может быть равен "null"
            // В реальном приложении в свойствах привязки хорошо бы делать проверки на null 
            imageInfo = new ImageInfo
            {
                // Откуда будет браться картинка для тестового приложения не оговаривалось. Как мне кажется я реализовал самый простой вариант (использовать как ресурс).
                Location = @"/ImageViewerPlugin;component/Resources/OIP.jpg",
                Width = 800,
                Height = 600
            };
        }
    }
}
