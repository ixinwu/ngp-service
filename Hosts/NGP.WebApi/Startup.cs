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

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NGP.Framework.WebApi.Core;
using System;

namespace NGP.WebApi
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
        public IConfiguration Configuration { get; }

        #endregion

        #region Ctor

        public Startup(IConfiguration configuration)
        {
            // 设置配置
            Configuration = configuration;
        }

        #endregion

        /// <summary>
        /// 将服务添加到应用程序并配置服务提供者
        /// </summary>
        /// <param name="services"></param>
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            return services.ConfigureApplicationServices(Configuration);
        }

        /// <summary>
        /// 配置应用程序HTTP请求管道
        /// </summary>
        /// <param name="application"></param>
        /// <param name="loggerFactory"></param>
        public void Configure(IApplicationBuilder application, ILoggerFactory loggerFactory)
        {
            application.ConfigureRequestPipeline(loggerFactory);
        }
    }
}
