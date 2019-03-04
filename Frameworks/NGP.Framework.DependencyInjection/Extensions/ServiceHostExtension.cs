/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * ServiceCollectionExtensions Description:
 * IServiceCollection扩展
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-20   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NGP.Framework.Core;
using System;
using System.Linq;

namespace NGP.Framework.DependencyInjection
{
    /// <summary>
    /// IServiceCollection扩展
    /// </summary>
    public static class ServiceHostExtension
    {
        /// <summary>
        /// 向应用程序添加服务并配置服务提供程序
        /// </summary>
        /// <param name="services">服务描述符的集合</param>
        /// <param name="configuration">配置应用程序</param>
        /// <returns>配置的服务提供商</returns>
        public static IServiceProvider ConfigureApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            // 创建、初始化和配置引擎
            EngineFactory.Create();

            // 配置默认提供者
            var provider = services.BuildServiceProvider();

            CommonHelper.DefaultFileProvider = new NGPFileProvider();

            // 类型查找器
            var typeFinder = new AppDomainTypeFinder();

            var dbConfigType = typeFinder.FindClassesOfType<IDbInitConfig>().FirstOrDefault();
            var dbConfig = Activator.CreateInstance(dbConfigType) as IDbInitConfig;
            dbConfig.ConfigureDataBase(services, configuration);

            // 注册依赖
            Singleton<IEngine>.Instance.RegisterDependencies(services, typeFinder);

            // 初始化引擎
            var serviceProvider = Singleton<IEngine>.Instance.Initialize(services, configuration);


            return serviceProvider;
        }
    }
}
