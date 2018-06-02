using HappyDog.BingWallpaper.Models;
using System;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using Windows.Storage;
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

        private async void Download_Click(object sender, RoutedEventArgs e)
        {
            string name = Path.GetFileName(ImageInfo.Url);
            var file = await ApplicationData.Current.LocalFolder.TryGetItemAsync(name);
            if (file == null)
            {
                var newFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(name, CreationCollisionOption.OpenIfExists);
                using (var client = new HttpClient())
                {
                    byte[] buffer = await client.GetByteArrayAsync(ImageInfo.Url);
                    await FileIO.WriteBytesAsync(newFile, buffer);
                }
            }
            var dialog = new ContentDialog
            {
                Title = "提示",
                Content = "图片下载成功",
                PrimaryButtonText = "确定"
            };
            await dialog.ShowAsync();
        }
    }
}
