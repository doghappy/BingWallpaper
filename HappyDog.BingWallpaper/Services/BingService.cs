using HappyDog.BingWallpaper.Models;
using Microsoft.QueryStringDotNET;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Storage;
using Windows.System.UserProfile;
using Windows.UI.Xaml.Controls;

namespace HappyDog.BingWallpaper.Services
{
    public class BingService
    {
        public BingService()
        {
            client = new HttpClient
            {
                BaseAddress = new Uri("https://www.bing.com/")
            };
        }

        readonly HttpClient client;

        int pageIndex = -1;

        public async Task<List<ImageInfo>> LoadAsync()
        {
            var list = new List<ImageInfo>();
            string url = $"HPImageArchive.aspx?format=js&idx={pageIndex}&n=8";
            string json = await client.GetStringAsync(url);
            dynamic data = JsonConvert.DeserializeObject(json);
            foreach (var item in data.images)
            {
                string date = item.enddate;
                int year = int.Parse(date.Substring(0, 4));
                int month = int.Parse(date.Substring(4, 2));
                int day = int.Parse(date.Substring(6));
                list.Add(new ImageInfo
                {
                    Url = "https://www.bing.com" + item.url,
                    Copyright = item.copyright,
                    CopyrightLink = item.copyrightlink,
                    Date = new DateTime(year, month, day)
                });
            }
            pageIndex += 8;
            return list;
        }

        public async Task DownloadAsync(string url)
        {
            QueryString args = QueryString.Parse(url);
            string name = args["https://www.bing.com/th?id"];
            var file = await ApplicationData.Current.LocalFolder.TryGetItemAsync(name);
            if (file == null)
            {
                var newFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(name, CreationCollisionOption.OpenIfExists);
                byte[] buffer = await client.GetByteArrayAsync(url);
                await FileIO.WriteBytesAsync(newFile, buffer);
            }
        }

        public async Task SetWallpaperByUrlAsync(string url)
        {
            QueryString args = QueryString.Parse(url);
            string name = args["https://www.bing.com/th?id"];
            await SetWallpaperByNameAsync(name);
        }

        public async Task SetWallpaperByNameAsync(string fileName)
        {
            var file = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
            bool result = await UserProfilePersonalizationSettings.Current.TrySetWallpaperImageAsync(file);
            var srcLoader = ResourceLoader.GetForCurrentView();
            if (result)
            {
                ContentDialog dialog = new ContentDialog
                {
                    Title = srcLoader.GetString("TipDialogTitle"),
                    Content = srcLoader.GetString("SetWallpaperSuccessDialogContent"),
                    PrimaryButtonText = srcLoader.GetString("DialogOK")
                };
                await dialog.ShowAsync();
            }
            else
            {
                ContentDialog dialog = new ContentDialog
                {
                    Title = srcLoader.GetString("TipDialogTitle"),
                    Content = srcLoader.GetString("SetWallpaperFailDialogContent"),
                    PrimaryButtonText = srcLoader.GetString("DialogOK")
                };
                await dialog.ShowAsync();
            }
        }
    }
}
