using HappyDog.BingWallpaper.Common;
using HappyDog.BingWallpaper.Models;
using HappyDog.BingWallpaper.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace HappyDog.BingWallpaper.Views
{
    public sealed partial class ImageListPage : Page
    {
        public ImageListPage()
        {
            InitializeComponent();
            bingServices = new BingServices();
            Images = new IncrementalCollection<ImageInfo>(LoadAsync);
        }

        readonly BingServices bingServices;

        public IncrementalCollection<ImageInfo> Images { get; }

        int loadCount = 0;
        private async Task<IEnumerable<ImageInfo>> LoadAsync()
        {
            if (loadCount < 2)
            {
                loadCount++;
                return await bingServices.LoadAsync();
            }
            else
            {
                Images.HasMoreItems = false;
                return new List<ImageInfo>();
            }
        }

        private void AdaptiveGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            //Windows.Storage.ApplicationData.Current.LocalFolder.
            //Windows.System.UserProfile.UserProfilePersonalizationSettings.Current.TrySetWallpaperImageAsync()
            Frame.Navigate(typeof(ImageDetailPage), e.ClickedItem);
        }
    }
}
