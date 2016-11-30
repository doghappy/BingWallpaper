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
        public void Start()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            //壁纸调度任务
            IJobDetail job = JobBuilder.Create<WallpaperJob>().Build();
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("triggerName", "groupName")
                .WithSimpleSchedule(s => s.WithIntervalInHours(1)
                .RepeatForever())
                .Build();

            scheduler.ScheduleJob(job, trigger);
        }
    }
}
