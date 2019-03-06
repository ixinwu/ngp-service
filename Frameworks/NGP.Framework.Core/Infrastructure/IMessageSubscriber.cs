/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * IMessageSubscriber Description:
 * 消息订阅者接口
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-15   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/
using System;

namespace NGP.Framework.Core
{
    /// <summary>
    /// 消息订阅者接口
    /// </summary>
    public interface IMessageSubscriber
    {
        /// <summary>
        /// 注册监听
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mqInfo"></param>
        /// <param name="action"></param>
        void Monitor<T>(MessageRouteInfo mqInfo, Action<T> action);
    }
}
