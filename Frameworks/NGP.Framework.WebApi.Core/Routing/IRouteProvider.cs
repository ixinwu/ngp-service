/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * IRouteProvider Description:
 * 路由提供者接口
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
    /// 路由提供者接口
    /// </summary>
    public interface IRouteProvider
    {
        /// <summary>
        /// 注册路由
        /// </summary>
        /// <param name="routeBuilder">Route builder</param>
        void RegisterRoutes(IRouteBuilder routeBuilder);

        /// <summary>
        /// 优先级
        /// </summary>
        int Priority { get; }
    }
}
