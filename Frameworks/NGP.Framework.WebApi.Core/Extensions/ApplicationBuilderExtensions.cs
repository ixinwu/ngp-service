/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * ApplicationBuilderExtensions Description:
 * IApplicationBuilder的扩展
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-20   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using NGP.Framework.Core;
using System;
using System.Linq;

namespace NGP.Framework.WebApi.Core
{
    /// <summary>
    /// IApplicationBuilder的扩展
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// 配置应用程序HTTP请求管道
        /// </summary>
        /// <param name="application">用于配置应用程序请求管道的生成器</param>
        /// <param name="loggerFactory"></param>
        public static void ConfigureRequestPipeline(this IApplicationBuilder application,ILoggerFactory loggerFactory)
        {
            // 通过类型查找器获取启动配置
            var typeFinder = Singleton<IEngine>.Instance.Resolve<ITypeFinder>();
            var startupConfigurations = typeFinder.FindClassesOfType<INGPStartup>();

            // 根据顺序创建启动配置
            var instances = startupConfigurations
                .Select(startup => (INGPStartup)Activator.CreateInstance(startup))
                .OrderBy(startup => startup.Order);

            // 配置日志
            loggerFactory.AddLog4Net();

            // 配置请求管道
            foreach (var instance in instances)
            {
                instance.Configure(application);
            }
        }
    }
}