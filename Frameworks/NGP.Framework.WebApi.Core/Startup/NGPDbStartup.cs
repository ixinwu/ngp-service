/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * NGPDbStartup Description:
 * 数据库配置启动器
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-28   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NGP.Framework.Core;
using NGP.Framework.DataAccess;

namespace NGP.Framework.WebApi.Core
{
    /// <summary>
    /// 数据库配置启动器
    /// </summary>
    public class NGPDbStartup : INGPStartup
    {
        /// <summary>
        /// 添加数据库配置启动
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            //add object context
            services.AddDbContext<UnitObjectContext>(optionsBuilder =>
            {
                optionsBuilder.UseSqlServer(ConfigurationExtensions.GetConnectionString(configuration, "DbConnection"));
            });

            //add EF services
            //services.AddEntityFrameworkSqlServer();
            //services.AddEntityFrameworkProxies();
        }

        /// <summary>
        /// 配置中间件
        /// </summary>
        /// <param name="application"></param>
        public void Configure(IApplicationBuilder application)
        {
        }

        /// <summary>
        /// 执行顺序
        /// </summary>
        public int Order
        {
            get { return 10; }
        }
    }
}