using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BingWallpaper
{
    class Wallpaper
    {
        /// <summary>
        /// 壁纸的路径
        /// </summary>
        public string WallpaperPath { get; private set; }

        /// <summary>
        /// 壁纸是否存在
        /// </summary>
        public bool Exists
        {
            get
            {
                if(Directory.Exists(WallpaperPath))
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(WallpaperPath);
                    var fileInfo = dirInfo.EnumerateFiles();
                    foreach(FileInfo file in fileInfo)
                    {
                        if(file.CreationTime.Date == DateTime.Now.Date)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }


        public Wallpaper(string path)
        {
            WallpaperPath = path;
        }

        #region 下载壁纸
        /// <summary>
        /// 下载壁纸
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task Download(string url)
        {
            HttpClient client = new HttpClient();
            string json = client.GetStringAsync(url).Result;

            JObject obj = JObject.Parse(json);
            string imgUrl = obj["images"].FirstOrDefault()["url"].Value<string>();
            string fileName = Path.GetFileName(imgUrl);
            byte[] buffer = await client.GetByteArrayAsync(imgUrl);
            if(!Directory.Exists(WallpaperPath))
            {
                Directory.CreateDirectory(WallpaperPath);
            }
            File.WriteAllBytes(WallpaperPath + fileName, buffer);
            //设置壁纸
            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, WallpaperPath + fileName, 1);
        }
        #endregion

        #region 引用user32.dll包
        /// <summary>
        /// 引用user32.dll包
        /// </summary>
        /// <param name="uAction"></param>
        /// <param name="uParam"></param>
        /// <param name="lpvparam"></param>
        /// <param name="fuwinIni"></param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "SystemParametersInfoA")]
        static extern Int32 SystemParametersInfo(Int32 uAction, Int32 uParam, string lpvparam, Int32 fuwinIni);
        private const int SPI_SETDESKWALLPAPER = 20;
        #endregion
    }
}
