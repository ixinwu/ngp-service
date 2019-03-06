/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * Startup Description:
 * 程序启动入口
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-2-28   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NGP.Framework.Core;
using NGP.Framework.DependencyInjection;
using System;
using System.IO;

namespace NGP.ServiceHost
{
    /// <summary>
    /// 启动类
    /// </summary>
    public class Startup
    {
        #region Properties

        /// <summary>
        /// 获取应用程序的配置
        /// </summary>
        public IConfiguration Configuration { get; set; }

        #endregion

        /// <summary>
        /// host启动执行的任务
        /// </summary>
        public void Start()
        {
            var services = new ServiceCollection();

            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(AppContext.BaseDirectory))
                .AddJsonFile("appsettings.json", optional: true);

            Configuration = builder.Build();

            services.ConfigureApplicationServices(Configuration);

            // 运行服务
            Singleton<IEngine>.Instance.Resolve<IServiceContainer>().Start();

            // 获取队列
            var mqInfo = Singleton<IEngine>.Instance.Resolve<IUnitRepository>().GetMessageRouteByBusinessKey(MessageQueueKey.__TestKey);
            // 启动消息监听
            Singleton<IEngine>.Instance.Resolve<IMessageSubscriber>().Monitor<NGPSingleRequest>(mqInfo,
            param =>
             {
                 var msg = string.Format("ThreadId : {0}\n, ReceiveMessage : {1}\n ReceiveTime: {2}\n",
                     System.Threading.Thread.CurrentThread.ManagedThreadId,
                     param.RequestData,
                     DateTime.Now);
                 Console.WriteLine(msg);
             });
        }

        /// <summary>
        /// 释放全局的对象
        /// </summary>
        public void Shutdown()
        {
            // 停止服务
            Singleton<IEngine>.Instance.Resolve<IServiceContainer>().Shutdown();
        }
    }
}
