/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * IDbInitConfig Description:
 * Db初始化配置
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-3-4   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace NGP.Framework.Core
{
    /// <summary>
    /// Db初始化配置
    /// </summary>
    public interface IDbInitConfig
    {
        /// <summary>
        /// 配置数据库
        /// </summary>
        /// <param name="services">服务描述符的集合</param>
        /// <param name="configuration">配置应用程序</param>
        void ConfigureDataBase(IServiceCollection services, IConfiguration configuration);
    }
}
