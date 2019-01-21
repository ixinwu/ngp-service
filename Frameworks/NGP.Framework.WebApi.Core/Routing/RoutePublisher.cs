/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * RoutePublisher Description:
 * 路由发布者
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-28   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using System;
using System.Linq;
using Microsoft.AspNetCore.Routing;
using NGP.Framework.Core;

namespace NGP.Framework.WebApi.Core
{
    /// <summary>
    /// 路由发布者
    /// </summary>
    public class RoutePublisher : IRoutePublisher
    {
        #region Fields

        /// <summary>
        /// Type finder
        /// </summary>
        protected readonly ITypeFinder typeFinder;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="typeFinder">Type finder</param>
        public RoutePublisher(ITypeFinder typeFinder)
        {
            this.typeFinder = typeFinder;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 注册路由
        /// </summary>
        /// <param name="routeBuilder">Route builder</param>
        public virtual void RegisterRoutes(IRouteBuilder routeBuilder)
        {
            // 查找路由提供程序
            var routeProviders = typeFinder.FindClassesOfType<IRouteProvider>();

            // 创建和排序路由提供程序的实例
            var instances = routeProviders
                .Where(routeProvider => PluginManager.FindPlugin(routeProvider)?.Installed ?? true) //ignore not installed plugins
                .Select(routeProvider => (IRouteProvider)Activator.CreateInstance(routeProvider))
                .OrderByDescending(routeProvider => routeProvider.Priority);

            // 注册所有提供的路由
            foreach (var routeProvider in instances)
                routeProvider.RegisterRoutes(routeBuilder);
        }

        #endregion
    }
}
