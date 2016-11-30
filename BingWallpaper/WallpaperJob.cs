using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingWallpaper
{
    /// <summary>
    /// Quartz壁纸任务
    /// </summary>
    class WallpaperJob : IJob
    {
        /// <summary>
        /// 执行调度任务
        /// </summary>
        /// <param name="context"></param>
        public async void Execute(IJobExecutionContext context)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) + $"\\BingWallpaper\\{DateTime.Now.Year}\\";
            Wallpaper wallpaper = new Wallpaper(path);
            if(!wallpaper.Exists)
            {
                await wallpaper.DownloadAsync("http://www.bing.com/HPImageArchive.aspx?format=js&idx=0&n=1");
            }
        }
    }
}
