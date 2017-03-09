using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace BingWallpaper
{
    class Wallpaper
    {
        /// <summary>
        /// 壁纸的路径
        /// </summary>
        public string WallpaperPath
        {
            get { return Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) + $"\\BingWallpapers\\{DateTime.Now.Year}\\"; }
        }

        #region 壁纸是否存在
        /// <summary>
        /// 壁纸是否存在
        /// </summary>
        public bool Exists
        {
            get
            {
                DirectoryInfo dirInfo = new DirectoryInfo(WallpaperPath);
                if(!dirInfo.Exists)
                {
                    return false;
                }
                var fileInfo = dirInfo.EnumerateFiles("*.jpg");
                foreach(FileInfo file in fileInfo)
                {
                    if(file.CreationTime.Date == DateTime.Now.Date)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        #endregion

        #region 下载壁纸
        /// <summary>
        /// 下载壁纸
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public void Download(string url)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://www.bing.com/");
            string json = client.GetStringAsync(url).Result;
            dynamic data = JsonConvert.DeserializeObject(json);
            string imgUrl = data.images[0].url;
            string fileName = Path.GetFileName(imgUrl);
            byte[] buffer = client.GetByteArrayAsync(imgUrl).Result;
            if(!Directory.Exists(WallpaperPath))
            {
                Directory.CreateDirectory(WallpaperPath);
            }
            File.WriteAllBytes(WallpaperPath + fileName, buffer);
        }
        #endregion

        #region 设置最新的壁纸
        public void SetWindowsWallpaper()
        {
            //查找壁纸文件夹下最新的壁纸
            DirectoryInfo dirInfo = new DirectoryInfo(WallpaperPath);
            FileInfo fileInfo = dirInfo.EnumerateFiles().OrderBy(f => f.CreationTime).LastOrDefault();
            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, fileInfo.FullName, 1);
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
        [DllImport("user32.dll", EntryPoint = "SystemParametersInfo")]
        static extern int SystemParametersInfo(int uAction, int uParam, string lpvparam, int fuwinIni);
        private const int SPI_SETDESKWALLPAPER = 20;
        #endregion
    }
}
