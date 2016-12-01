
namespace BingWallpaper
{
    class Program
    {
        static void Main(string[] args)
        {
            Wallpaper wallpaper = new Wallpaper();
            if(!wallpaper.Exists)
            {
                wallpaper.DownloadAsync("http://www.bing.com/HPImageArchive.aspx?format=js&idx=0&n=1").Start();
            }
        }
    }
}
