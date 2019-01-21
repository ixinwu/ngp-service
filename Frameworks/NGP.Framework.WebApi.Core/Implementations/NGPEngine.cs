/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * NGPEngine Description:
 * 引擎实现
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-2-28   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NGP.Framework.Core;
using NGP.Framework.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NGP.Framework.WebApi.Core
{
    /// <summary>
    /// 引擎实现
    /// </summary>
    public class NGPEngine : IEngine
    {
        #region Properties

        /// <summary>
        /// 服务提供者
        /// </summary>
        private IServiceProvider _serviceProvider { get; set; }

        #endregion

        #region Utilities

        /// <summary>
        /// 获取 IServiceProvider
        /// </summary>
        /// <returns>IServiceProvider</returns>
        protected IServiceProvider GetServiceProvider()
        {
            var accessor = ServiceProvider.GetService<IHttpContextAccessor>();
            var context = accessor.HttpContext;
            return context?.RequestServices ?? ServiceProvider;
        }

        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="typeFinder">Type finder</param>
        public IServiceProvider RegisterDependencies(IServiceCollection services, ITypeFinder typeFinder)
        {
            var containerBuilder = new ContainerBuilder();

            // 注册引擎
            containerBuilder.RegisterInstance(this).As<IEngine>().SingleInstance();

            // 注册类型查找器
            containerBuilder.RegisterInstance(typeFinder).As<ITypeFinder>().SingleInstance();

            // 查找IDependencyRegistrar
            var dependencyRegistrars = typeFinder.FindClassesOfType<IDependencyRegistrar>();

            // 创建依赖注入器
            var instances = dependencyRegistrars
                //.Where(dependencyRegistrar => PluginManager.FindPlugin(dependencyRegistrar)?.Installed ?? true) //ignore not installed plugins
                .Select(dependencyRegistrar => (IDependencyRegistrar)Activator.CreateInstance(dependencyRegistrar))
                .OrderBy(dependencyRegistrar => dependencyRegistrar.Order);

            // 注册所有提供的依赖项
            foreach (var dependencyRegistrar in instances)
                dependencyRegistrar.Register(containerBuilder, typeFinder);

            // 使用注册的服务描述符集填充Autofac容器构建器
            containerBuilder.Populate(services);

            // 创建服务提供商
            _serviceProvider = new AutofacServiceProvider(containerBuilder.Build());
            return _serviceProvider;
        }
        #endregion

        #region Methods
        /// <summary>
        /// 初始化服务和配置
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration of the application</param>
        /// <returns>Service provider</returns>
        public IServiceProvider Initialize(IServiceCollection services, IConfiguration configuration)
        {
            // 组件加载事件
            AppDomain.CurrentDomain.AssemblyResolve += (sender,args)=>
            {
                // 检查已加载的装配
                var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName == args.Name);
                if (assembly != null)
                    return assembly;

                // 从TypeFinder获取程序集
                var tf = Resolve<ITypeFinder>();
                assembly = tf.GetAssemblies().FirstOrDefault(a => a.FullName == args.Name);
                return assembly;
            };
            
            return _serviceProvider;
        }

        /// <summary>
        /// 获取注入实例
        /// </summary>
        /// <typeparam name="T">Type of resolved service</typeparam>
        /// <returns>Resolved service</returns>
        public T Resolve<T>() where T : class
        {
            return (T)GetServiceProvider().GetRequiredService(typeof(T));
        }

        /// <summary>
        /// 获取注入实例
        /// </summary>
        /// <param name="type">Type of resolved service</param>
        /// <returns>Resolved service</returns>
        public object Resolve(Type type)
        {
            return GetServiceProvider().GetRequiredService(type);
        }

        /// <summary>
        /// 获取注入实例列表
        /// </summary>
        /// <typeparam name="T">Type of resolved services</typeparam>
        /// <returns>Collection of resolved services</returns>
        public IEnumerable<T> ResolveAll<T>()
        {
            return (IEnumerable<T>)GetServiceProvider().GetServices(typeof(T));
        }

        /// <summary>
        /// 获取没有注册的实例
        /// </summary>
        /// <param name="type">Type of service</param>
        /// <returns>Resolved service</returns>
        public virtual object ResolveUnregistered(Type type)
        {
            Exception innerException = null;
            foreach (var constructor in type.GetConstructors())
            {
                try
                {
                    // 尝试解析构造函数参数
                    var parameters = constructor.GetParameters().Select(parameter =>
                    {
                        var service = Resolve(parameter.ParameterType);
                        if (service == null)
                            throw new NGPException("Unknown dependency");
                        return service;
                    });

                    // 创建对象
                    return Activator.CreateInstance(type, parameters.ToArray());
                }
                catch (Exception ex)
                {
                    innerException = ex;
                }
            }

            throw new NGPException("No constructor was found that had all the dependencies satisfied.", innerException);
        }

        #endregion

        #region Properties

        /// <summary>
        /// 服务提供者
        /// </summary>
        public virtual IServiceProvider ServiceProvider => _serviceProvider;

        #endregion
    }
}
