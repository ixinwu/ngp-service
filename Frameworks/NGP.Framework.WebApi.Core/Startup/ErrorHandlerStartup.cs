/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * ErrorHandlerStartup Description:
 * 错误处理启动器
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
    /// 错误处理启动器
    /// </summary>
    public class ErrorHandlerStartup : INGPStartup
    {
        /// <summary>
        /// 添加错误启动
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
           
        }

        /// <summary>
        /// 配置错误处理中间件
        /// </summary>
        /// <param name="application"></param>
        public void Configure(IApplicationBuilder application)
        {
        }

        /// <summary>
        /// 启动顺序
        /// </summary>
        public int Order
        {
            // 应首先加载错误处理程序
            get { return 0; }
        }
    }
}
