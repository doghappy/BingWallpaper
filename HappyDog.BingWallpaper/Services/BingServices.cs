using HappyDog.BingWallpaper.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace HappyDog.BingWallpaper.Services
{
    public class BingServices
    {
        public BingServices()
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
                    Url = "https://www.bing.com/" + item.url,
                    Copyright = item.copyright,
                    CopyrightLink = item.copyrightlink,
                    Date = new DateTime(year, month, day)
                });
            }
            pageIndex += 8;
            return list;
        }
    }
}
