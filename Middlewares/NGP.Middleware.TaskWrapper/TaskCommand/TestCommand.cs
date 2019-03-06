/* ------------------------------------------------------------------------------
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * TestCommand Description:
 * 测试
 *
 * Comment 					        Revision	Date                  Author
 * -----------------------------    --------    ------------------    ----------------
 * Created							1.0		    2019/3/5 15:27:42    hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/


using NGP.Framework.Core;
using System;
using System.Linq;

namespace NGP.Middleware.TaskWrapper.TaskCommand
{
    /// <summary>
    /// 测试
    /// </summary>
    public class TestCommand : IServiceCommand
    {
        #region Properties
        /// <summary>
        /// 服务类型
        /// </summary>
        public string ServiceKey
        {
            get
            {
                return ServiceCommandKey.__TestCommand;
            }
        }
        #endregion

        #region IServiceCommand Methods
        /// <summary>
        /// 当前执行的任务
        /// </summary>
        /// <param name="descriptor">服务描述</param>
        public void Excute(ServiceJobDescriptor descriptor)
        {
            var workContext = Singleton<IEngine>.Instance.Resolve<IWorkContext>();
            var unitRepository = Singleton<IEngine>.Instance.Resolve<IUnitRepository>();

            var mqInfo = unitRepository.GetMessageRouteByBusinessKey(MessageQueueKey.__TestKey);
            var sendMessage = string.Format("CurrentEmplId = {0} , DBEmployeeCount = {1}",
                workContext.Current.EmplId,
                unitRepository.AllNoTracking<Sys_Org_Employee>().Count());

            Singleton<IEngine>.Instance.Resolve<IMessagePublisher>().Send(mqInfo, new NGPSingleRequest
            {
                RequestData = sendMessage
            });
            var msg = string.Format("ThreadId : {0}\n, SendMessage : {1}\n SendTime : {2}\n",
                     System.Threading.Thread.CurrentThread.ManagedThreadId,
                     sendMessage,
                     DateTime.Now);
            Console.WriteLine(msg);
        }
        #endregion
    }
}
