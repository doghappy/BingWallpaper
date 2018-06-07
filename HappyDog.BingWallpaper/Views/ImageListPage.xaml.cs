using HappyDog.BingWallpaper.Common;
using HappyDog.BingWallpaper.Models;
using HappyDog.BingWallpaper.Services;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace HappyDog.BingWallpaper.Views
{
    public sealed partial class ImageListPage : Page, INotifyPropertyChanged
    {
        public ImageListPage()
        {
            InitializeComponent();
            IsLoading = true;
            bingServices = new BingServices();
            Images = new IncrementalCollection<ImageInfo>(LoadAsync);
        }

        readonly BingServices bingServices;
        public event PropertyChangedEventHandler PropertyChanged;

        public IncrementalCollection<ImageInfo> Images { get; }

        private bool isLoading;
        public bool IsLoading
        {
            get => isLoading;
            set
            {
                isLoading = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsLoading)));
            }
        }


        int loadCount = 0;
        private async Task<IEnumerable<ImageInfo>> LoadAsync()
        {
            if (loadCount < 2)
            {
                loadCount++;
                var data = await bingServices.LoadAsync();
                IsLoading = false;
                return data;
            }
            else
            {
                Images.HasMoreItems = false;
                return new List<ImageInfo>();
            }
        }

        private void AdaptiveGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            Frame.Navigate(typeof(ImageDetailPage), e.ClickedItem);
        }
    }
}
