/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * SqlerverDbInitConfig Description:
 * sqlserver Db初始化配置
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-3-4   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NGP.Framework.Core;

namespace NGP.Framework.DataAccess
{
    /// <summary>
    /// sqlserver Db初始化配置
    /// </summary>
    public class SqlserverDbInitConfig : IDbInitConfig
    {
        /// <summary>
        /// 数据库类型
        /// </summary>
        public string DbType => "Sqlserver";

        /// <summary>
        /// 配置数据库
        /// </summary>
        /// <param name="services">服务描述符的集合</param>
        /// <param name="configuration">配置应用程序</param>
        public void ConfigureDataBase(IServiceCollection services, IConfiguration configuration)
        {
            //add object context
            services.AddEntityFrameworkSqlServer()
                .AddDbContext<UnitObjectContext>(optionsBuilder =>
                {
                    optionsBuilder.UseSqlServer(ConfigurationExtensions.GetConnectionString(configuration, "DbConnection"));
                },
                ServiceLifetime.Scoped);
        }
    }
}
