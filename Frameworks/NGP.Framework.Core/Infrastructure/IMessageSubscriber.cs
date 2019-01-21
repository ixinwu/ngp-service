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
    /// <typeparam name="T">监听数据类型</typeparam>
    public interface IMessageSubscriber<T>
    {
        /// <summary>
        /// 注册监听
        /// </summary>
        /// <param name="mqInfo">监听通道对象</param>
        /// <param name="action">监听回调</param>
        void Monitor(MessageRouteInfo mqInfo, Action<T> action);
    }
}
