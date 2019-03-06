/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * NGPApiStartup Description:
 * api配置启动器
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-28   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using NGP.Framework.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace NGP.Framework.WebApi.Core
{
    /// <summary>
    /// api配置启动器
    /// </summary>
    public class NGPApiStartup : INGPStartup
    {
        /// <summary>
        /// 添加api配置启动
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            // 添加基本的MVC功能
            var mvcBuilder = services.AddMvc();

            // 设置MvcOptions上设置的默认值以匹配asp.net core mvc 2.1的行为
            mvcBuilder.SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // 配置json序列化
            //mvcBuilder.AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());
        }

        /// <summary>
        /// 配置api中间件
        /// </summary>
        /// <param name="application"></param>
        public void Configure(IApplicationBuilder application)
        {
            // 全局配置
            application.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            //路由
            application.UseMvc(routeBuilder =>
            {
                //注册路由
                Singleton<IEngine>.Instance.Resolve<IRoutePublisher>().RegisterRoutes(routeBuilder);
            });
        }

        /// <summary>
        /// 配置顺序
        /// </summary>
        public int Order
        {
            get { return 1000; }
        }
    }
}