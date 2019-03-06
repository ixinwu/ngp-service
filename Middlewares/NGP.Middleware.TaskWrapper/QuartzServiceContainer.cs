/* ------------------------------------------------------------------------------
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * QuartzServiceContainer Description:
 * 策略容器实现
 *
 * Comment 					        Revision	Date                  Author
 * -----------------------------    --------    ------------------    ----------------
 * Created							1.0		    2017/7/21 16:22:16    yulin@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using NGP.Framework.Core;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NGP.Middleware.TaskWrapper
{
    public class QuartzServiceContainer : IServiceContainer
    {
        #region private fields
        /// <summary>
        /// 计划工厂
        /// </summary>
        private ISchedulerFactory _schedulerFactory;

        /// <summary>
        /// 计划接口
        /// </summary>
        private IScheduler _scheduler;

        /// <summary>
        /// 类型查找器
        /// </summary>
        private readonly ITypeFinder _typeFinder;

        /// <summary>
        /// 工作单一仓储
        /// </summary>
        private readonly IUnitRepository _unitRepository;
        #endregion

        #region ctor
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="typeFinder"></param>
        /// <param name="unitRepository"></param>
        public QuartzServiceContainer(ITypeFinder typeFinder, IUnitRepository unitRepository)
        {
            _typeFinder = typeFinder;
            _unitRepository = unitRepository;
        }
        #endregion

        #region properties
        #region 从数据库获取服务项
        /// <summary>
        /// 从数据库获取服务项
        /// </summary>
        private IEnumerable<ServiceJobDescriptor> DbServiceDescriptors
        {
            get
            {
                var configs = _unitRepository.AllNoTracking<Sys_Config_ServiceRunning>(s => s.IsEnable && !s.IsDelete)
                    .OrderBy(o => o.OrderIndex)
                    .Select(s => new ServiceJobDescriptor
                    {
                        CronExpression = s.CronConfig,
                        Id = s.Id,
                        ServiceKey = s.ConfigKey,
                        OrderIndex = s.OrderIndex,
                        ValidEndTime = s.ValidEndTime,
                        ValidStartTime = s.ValidStartTime
                    }).ToList();
                return configs;
            }
        }
        #endregion

        #region 获取所有的服务执行command
        /// <summary>
        /// 获取所有的服务执行command
        /// </summary>
        private IEnumerable<IServiceCommand> CodeServiceCommands
        {
            get
            {
                var serviceCommandTypes = _typeFinder.FindClassesOfType<IServiceCommand>();
                var serviceCommands = new List<IServiceCommand>();
                foreach (var serviceCommandType in serviceCommandTypes)
                {
                    var serviceCommand = Singleton<IEngine>.Instance.ResolveUnregistered(serviceCommandType) as IServiceCommand;
                    if (serviceCommand == null)
                    {
                        continue;
                    }
                    serviceCommands.Add(serviceCommand);
                }
                return serviceCommands;
            }
        }
        #endregion
        #endregion

        #region IServiceContainer Methods
        #region 启动所有定时任务
        /// <summary>
        /// 启动所有定时任务
        /// </summary>
        public void Start()
        {
            // 计划工厂
            _schedulerFactory = new StdSchedulerFactory();
            _scheduler = _schedulerFactory.GetScheduler().Result;
            _scheduler.Start();

            var serviceJobDescriptors = InitializePolicyDescriptors();
            foreach (var item in serviceJobDescriptors)
            {
                AddServiceJob(item);
            }
        }
        #endregion

        #region 停止所有定时任务
        /// <summary>
        /// 停止所有定时任务
        /// </summary>
        public void Shutdown()
        {
            if (_scheduler != null)
            {
                if (!_scheduler.IsShutdown)
                    _scheduler.Shutdown(false);
            }
        }
        #endregion

        #region 添加服务项
        /// <summary>
        /// 添加服务项
        /// </summary>
        public void AddServiceJob(ServiceJobDescriptor item)
        {
            // 传递数据的map
            var map = new JobDataMap();
            var now = DateTime.Now;
            // 添加数据映射
            map.Add(GlobalConst.__ServiceDataMapKey, item);
            // 绑定job的详细信息，以及执行容器PolicyWrapper
            var job = JobBuilder.Create<QuartzServiceWrapper>()
                .WithIdentity(item.Id)
                .SetJobData(map)
                .Build();

            // 启动时间
            DateTime? firstDateTime = null;

            // 存在有效开始日期
            if (item.ValidStartTime.HasValue &&
                DateTime.Compare(item.ValidStartTime.Value, now) > 0)
            {
                firstDateTime = item.ValidStartTime;
            }
            else
            {
                firstDateTime = DateTime.Now.AddMinutes(1);
            }

            // 绑定job的执行计划
            var trigger = TriggerBuilder.Create()
                 .WithIdentity("trigger" + item.Id)
                 // 表达式启动
                 .WithCronSchedule(item.CronExpression);

            trigger.StartAt(firstDateTime.Value);

            if (item.ValidEndTime.HasValue)
            {
                trigger = trigger.EndAt(item.ValidEndTime.Value);
            }

            //重复次数
            if (item.RepeatCount != null)
                trigger.WithSimpleSchedule(x => x.WithRepeatCount(item.RepeatCount.Value));

            _scheduler.ScheduleJob(job, trigger.Build());
        }
        #endregion
        #endregion

        #region private methods
        #region 初始化服务项列表
        /// <summary>
        /// 初始化服务项列表
        /// </summary>
        /// <returns></returns>
        private IEnumerable<ServiceJobDescriptor> InitializePolicyDescriptors()
        {
            // 获取数据库服务项
            var allServiceDescriptors = DbServiceDescriptors;

            // 获取所有的策略执行command
            var serviceCommands = CodeServiceCommands;

            IServiceCommand command;

            // 初始化回调
            foreach (var item in allServiceDescriptors)
            {
                command = serviceCommands.FirstOrDefault(a => a.ServiceKey == item.ServiceKey);

                if (command != null)
                {
                    item.Command = command;
                }
            }

            return allServiceDescriptors.Where(s => s.Command != null).ToList();
        }
        #endregion

        #endregion
    }
}
