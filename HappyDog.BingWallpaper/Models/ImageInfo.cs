using Newtonsoft.Json;
using System;

namespace HappyDog.BingWallpaper.Models
{
    public class ImageInfo
    {
        public DateTime Date { get; set; }
        public string Url { get; set; }
        public string Copyright { get; set; }
        public string CopyrightLink { get; set; }
    }
}
