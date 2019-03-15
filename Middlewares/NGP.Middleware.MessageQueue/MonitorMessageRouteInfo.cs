/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * MonitorMessageRouteInfo Description:
 * 监听消息路由对象
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    1.0		    2019/3/4 11:20:22 hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using System;
using NGP.Framework.Core;

namespace NGP.Middleware.MessageQueue
{
    /// <summary>
    /// 监听消息路由对象
    /// </summary>
    public class MonitorMessageRouteInfo<T>
    {
        /// <summary>
        /// 路由对象
        /// </summary>
        public MessageRouteInfo MessageRoute { get; set; } 

        /// <summary>
        /// 回调函数
        /// </summary>
        public Action<T> Action { get; set; }
    }
}
