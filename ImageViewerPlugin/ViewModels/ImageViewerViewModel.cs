﻿using Caliburn.Micro;
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

            imageInfo = new ImageInfo
            {
                Location = @"/ImageViewerPlugin;component/Resources/OIP.jpg",
                Width = 800,
                Height = 600
            };
        }
    }
}