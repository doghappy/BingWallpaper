using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingWallpaper
{
    /// <summary>
    /// Quartz调度
    /// </summary>
    class Scheduler
    {
        /// <summary>
        /// 开始执行定时调度
        /// </summary>
        public static void Start()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            //壁纸调度任务
        }
    }
}
