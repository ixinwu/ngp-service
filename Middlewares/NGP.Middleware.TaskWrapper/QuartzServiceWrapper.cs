/* ------------------------------------------------------------------------------
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * QuartzServiceWrapper Description:
 * Quartz服务包装
 *
 * Comment 					        Revision	Date                  Author
 * -----------------------------    --------    ------------------    ----------------
 * Created							1.0		    2017/7/26 17:03:05    yulin@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using NGP.Framework.Core;
using Quartz;
using System;
using System.Threading.Tasks;

namespace NGP.Middleware.TaskWrapper
{
    /// <summary>
    /// Quartz服务包装
    /// </summary>
    public class QuartzServiceWrapper : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            JobDataMap dataMap = context.MergedJobDataMap;

            if (dataMap.ContainsKey(GlobalConst.__ServiceDataMapKey))
            {
                // 获取当前job传递的数据
                var serviceDescriptor = dataMap[GlobalConst.__ServiceDataMapKey] as ServiceJobDescriptor;

                if (serviceDescriptor.Command != null)
                {
                    try
                    {
                        // 执行绑定的委托
                         serviceDescriptor.Command.Excute(serviceDescriptor);
                    }
                    catch (Exception ex)
                    {
                        var type = serviceDescriptor.Command.GetType();
                        // 写日志
                        var info = new NGPExceptionLog
                        {
                            BusinessMethod = string.Format("{0}.{1}", type.FullName, "Excute"),
                            ExceptionContent = string.IsNullOrEmpty(ex.Message) ? (ex.InnerException != null ? ex.InnerException.Message : "Unknow Error") : ex.Message,
                            Exception = ex
                        };
                        Singleton<IEngine>.Instance.Resolve<ILogPublisher>().RegisterError(info);
                    }
                }
            }
            return Task.FromResult(0);
        }
    }
}
