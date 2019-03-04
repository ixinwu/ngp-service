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

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NGP.Framework.Core;
using System;
using System.Linq;
using System.Net;

namespace NGP.Framework.WebApi.Core
{
    /// <summary>
    /// IServiceCollection扩展
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 向应用程序添加服务并配置服务提供程序
        /// </summary>
        /// <param name="services">服务描述符的集合</param>
        /// <param name="configuration">配置应用程序</param>
        /// <returns>配置的服务提供商</returns>
        public static IServiceProvider ConfigureApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            // 添加ngpconfig配置参数
            services.ConfigureStartupConfig<NGPConfig>(configuration.GetSection("NGP"));
            // 向httpContext添加访问器
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // 创建、初始化和配置引擎
            EngineWebFactory.Create();

            // 添加TLS 1.2
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            // 配置默认提供者
            var provider = services.BuildServiceProvider();

            // 文件提供者
            var hostingEnvironment = provider.GetRequiredService<IHostingEnvironment>();
            CommonHelper.DefaultFileProvider = new NGPWebFileProvider(hostingEnvironment);

            // 初始化插件
            var mvcCoreBuilder = services.AddMvcCore();
            PluginManager.Initialize(mvcCoreBuilder.PartManager);

            // 查找其他程序集提供的启动配置
            var typeFinder = new WebAppTypeFinder();
            var startupConfigurations = typeFinder.FindClassesOfType<INGPStartup>();

            // 初始化数据库
            var dbConfigType = typeFinder.FindClassesOfType<IDbInitConfig>().FirstOrDefault();
            var dbConfig = Activator.CreateInstance(dbConfigType) as IDbInitConfig;
            dbConfig.ConfigureDataBase(services, configuration);

            // 创建和排序启动配置的实例
            var instances = startupConfigurations
                .Select(startup => (INGPStartup)Activator.CreateInstance(startup))
                .OrderBy(startup => startup.Order);

            // 配置服务
            foreach (var instance in instances)
            {
                instance.ConfigureServices(services, configuration);
            }

            // 注册依赖
            Singleton<IEngine>.Instance.RegisterDependencies(services, typeFinder);

            // 初始化引擎
            var serviceProvider = Singleton<IEngine>.Instance.Initialize(services, configuration);

            return serviceProvider;
        }

        /// <summary>
        /// 创建，绑定和注册服务指定的配置参数
        /// </summary>
        /// <typeparam name="TConfig">Configuration parameters</typeparam>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">一组键/值应用程序配置属性</param>
        /// <returns>配置参数的实例</returns>
        public static TConfig ConfigureStartupConfig<TConfig>(this IServiceCollection services, IConfiguration configuration) where TConfig : class, new()
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            // 创建配置实例
            var config = new TConfig();

            // 将其绑定到配置的相应部分
            configuration.Bind(config);

            // 并将其注册为服务
            services.AddSingleton(config);

            return config;
        }
    }
}