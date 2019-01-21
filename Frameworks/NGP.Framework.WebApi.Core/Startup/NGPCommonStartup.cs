/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * NGPCommonStartup Description:
 * 公共配置启动器
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-28   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace NGP.Framework.WebApi.Core
{
    /// <summary>
    /// 公共配置启动器
    /// </summary>
    public class NGPCommonStartup : INGPStartup
    {
        /// <summary>
        /// 添加公共配置启动
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            // 返回结果压缩
            services.AddResponseCompression();

            // 添加选项功能
            services.AddOptions();

            ////add memory cache
            //services.AddMemoryCache();

            ////add distributed memory cache
            //services.AddDistributedMemoryCache();

            // 添加本地化
            services.AddLocalization();
        }

        /// <summary>
        /// 配置中间件
        /// </summary>
        /// <param name="application"></param>
        public void Configure(IApplicationBuilder application)
        {
            // 使用请求本地化
            application.UseRequestLocalization();

            // 添加请求上下文中间件
            application.UseMiddleware<RequestContextMiddleware>();
        }

        /// <summary>
        /// 加载顺序
        /// </summary>
        public int Order
        {
            // 通用的在错误之后
            get { return 100; }
        }
    }
}