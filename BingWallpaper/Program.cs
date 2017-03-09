using System;

namespace BingWallpaper
{
    class Program
    {
        static void Main(string[] args)
        {
            Wallpaper wallpaper = new Wallpaper();
            Console.WriteLine("检测是否已经下载过壁纸……");
            if(!wallpaper.Exists)
            {
                Console.WriteLine("正在下载壁纸……");
                wallpaper.Download("HPImageArchive.aspx?format=js&idx=0&n=1");
                Console.WriteLine("壁纸下载成功，正在更改壁纸……");
                wallpaper.SetWindowsWallpaper();
            }
            else
            {
                Console.WriteLine("今日壁纸已经下载过了");
            }
        }
    }
}
