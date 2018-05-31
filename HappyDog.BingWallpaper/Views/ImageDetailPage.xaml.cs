﻿using HappyDog.BingWallpaper.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace HappyDog.BingWallpaper.Views
{
    public sealed partial class ImageDetailPage : Page, INotifyPropertyChanged
    {
        public ImageDetailPage()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private ImageInfo imageInfo;
        public ImageInfo ImageInfo
        {
            get => imageInfo;
            set
            {
                imageInfo = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ImageInfo)));
            }
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ImageInfo = e.Parameter as ImageInfo;
        }
    }
}
