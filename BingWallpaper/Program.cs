
namespace BingWallpaper
{
    class Program
    {
        static void Main(string[] args)
        {
            Wallpaper wallpaper = new Wallpaper();
            if(!wallpaper.Exists)
            {
                wallpaper.Download("HPImageArchive.aspx?format=js&idx=0&n=1");
            }
        }
    }
}
