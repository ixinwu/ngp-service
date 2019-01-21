/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * LogPublisher Description:
 * 日志发布者
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2017-3-9   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using NGP.Framework.Core;
using System.Threading.Tasks.Dataflow;

namespace NGP.Foundation.Service.Logger
{
    /// <summary>
    /// 日志发布者
    /// </summary>
    public class LogPublisher : ILogPublisher
    {
        /// <summary>
        /// 初始化错误日志TPL
        /// </summary>
        private static readonly ActionBlock<ErrorLogInfo> _sendErrorHandler = null;

        /// <summary>
        /// 初始化业务日志TPL
        /// </summary>
        private static readonly ActionBlock<BusinessLogContext> _sendBusinessHandler = null;

        /// <summary>
        /// ctor
        /// </summary>
        static LogPublisher()
        {
            _sendErrorHandler = new ActionBlock<ErrorLogInfo>(context => SendErrorHandler(context));
            _sendBusinessHandler = new ActionBlock<BusinessLogContext>(context => SendBusinessHandler(context));
        }

        /// <summary>
        /// 注册错误日志
        /// </summary>
        /// <param name="obj">日志对象</param>
        public void RegisterError(ErrorLogInfo obj)
        {
            _sendErrorHandler.Post(obj);
        }

        /// <summary>
        /// 错误日志数据处理
        /// </summary>
        /// <param name="context">上下文</param>
        private static void SendErrorHandler(ErrorLogInfo context)
        {
            Singleton<IEngine>.Instance.Resolve<ILogProvider>().InsertSysErrorLog(context);
        }

        /// <summary>
        /// 业务日志数据处理
        /// </summary>
        /// <param name="context">上下文</param>
        private static void SendBusinessHandler(BusinessLogContext context)
        {
            Singleton<IEngine>.Instance.Resolve<ILogProvider>().InsertBusinessLog(context);
        }

        /// <summary>
        /// 注册业务日志
        /// </summary>
        /// <param name="info">日志对象</param>
        public void RegisterBusiness(BusinessLogContext info)
        {
            _sendBusinessHandler.Post(info);
        }

    }
}
