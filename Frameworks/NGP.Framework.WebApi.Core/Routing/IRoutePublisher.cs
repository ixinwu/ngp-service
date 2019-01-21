/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * IRoutePublisher Description:
 * 路由发布者接口
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
    /// 路由发布者接口
    /// </summary>
    public interface IRoutePublisher
    {
        /// <summary>
        /// 注册路由
        /// </summary>
        /// <param name="routeBuilder">Route builder</param>
        void RegisterRoutes(IRouteBuilder routeBuilder);
    }
}
