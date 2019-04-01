using HappyDog.BingWallpaper.Models;
using HappyDog.BingWallpaper.Services;
using System;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using Windows.ApplicationModel.Resources;
using Windows.Storage;
using Windows.System.UserProfile;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace HappyDog.BingWallpaper.Views
{
    public sealed partial class ImageDetailPage : Page, INotifyPropertyChanged
    {
        public ImageDetailPage()
        {
            InitializeComponent();
            bingService = new BingService();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        readonly BingService bingService;

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

        private async void Download_Click(object sender, RoutedEventArgs e)
        {
            await bingService.DownloadAsync(ImageInfo.Url);
            var srcLoader = ResourceLoader.GetForCurrentView();
            var dialog = new ContentDialog
            {
                Title = srcLoader.GetString("TipDialogTitle"),
                Content = srcLoader.GetString("DownloadOkDialogContent"),
                PrimaryButtonText = srcLoader.GetString("DialogOK")
            };
            await dialog.ShowAsync();
        }

        private async void SetWallPaper_Click(object sender, RoutedEventArgs e)
        {
            await bingService.DownloadAsync(ImageInfo.Url);
            await bingService.SetWallpaperByUrlAsync(ImageInfo.Url);
        }
    }
}
