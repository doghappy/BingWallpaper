using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Topshelf;

namespace BingWallpaper
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(h =>
            {
                h.Service<Scheduler>(s =>
                {
                    //配置一个完全定制的服务，对Topshelf没有依赖关系。常用的方式
                    s.ConstructUsing(scheduler => new Scheduler());
                    s.WhenStarted(sc => sc.Start());
                    s.WhenStopped(sc => sc.ToString());
                });
                //服务使用Network_Service内置账户运行。身份标识符有好几种方式，如：
                //h.RunAs("username","password"); h.RunAsPrompt(); h.RunAsNetworkService();等
                h.RunAsLocalSystem();
                h.SetDescription("将桌面壁纸设置为Bing壁纸");
                h.SetDisplayName("BingWallpaper");
                h.SetServiceName("BingWallpaper");
            });
            Console.ReadKey();
        }
    }
}
