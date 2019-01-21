/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * RouteProvider Description:
 * 路由提供者
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-28   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using Microsoft.AspNetCore.Routing;

namespace NGP.Framework.WebApi.Core
{
    /// <summary>
    /// 表示提供基本路由的提供程序
    /// </summary>
    public partial class RouteProvider : IRouteProvider
    {
        #region Methods

        /// <summary>
        /// 注册路由
        /// </summary>
        /// <param name="routeBuilder">Route builder</param>
        public void RegisterRoutes(IRouteBuilder routeBuilder)
        {
            // api目前不需要独立注册路由，在apicontroll里配置
            //routeBuilder.MapRoute("default", "{controller=Home}/{action=Get}/{id?}");
        }

        #endregion

        #region Properties

        /// <summary>
        /// 优先级
        /// </summary>
        public int Priority
        {
            get { return 0; }
        }

        #endregion
    }
}
