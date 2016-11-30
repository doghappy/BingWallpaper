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
    class WallpaperJob
    {
        private Wallpaper wallpaper;

        public WallpaperJob(Wallpaper wallpaper)
        {
            this.wallpaper = wallpaper;
        }

        /// <summary>
        /// 执行调度任务
        /// </summary>
        /// <param name="context"></param>
        public async void ExcuteAsync(IJobExecutionContext context)
        {
            if(!wallpaper.Exists)
            {
                await wallpaper.DownloadAsync("http://www.bing.com/HPImageArchive.aspx?format=js&idx=0&n=1")
                    .ContinueWith(wallpaper.SetWindowsWallpaper);
            }
        }
    }
}
