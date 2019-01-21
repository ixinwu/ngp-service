/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * INGPStartup Description:
 * 应用程序启动时配置服务和中间件的对象
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
    /// 应用程序启动时配置服务和中间件的对象
    /// </summary>
    public interface INGPStartup
    {
        /// <summary>
        /// 添加和配置任何中间件
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        void ConfigureServices(IServiceCollection services, IConfiguration configuration);

        /// <summary>
        /// 配置添加中间件的使用
        /// </summary>
        /// <param name="application">用于配置应用程序请求管道的构建器</param>
        void Configure(IApplicationBuilder application);

        /// <summary>
        /// 获取此启动配置实现的顺序
        /// </summary>
        int Order { get; }
    }
}
