/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * IMessagePublisher Description:
 * 消息发布者
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-15   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/
using System.Collections.Generic;

namespace NGP.Framework.Core
{
    /// <summary>
    /// 消息发布者
    /// </summary>
    public interface IMessagePublisher
    {
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <typeparam name="T">发送数据对象</typeparam>
        /// <param name="mqInfo">目标通道信息</param>
        /// <param name="data">数据</param>
        void Send<T>(MessageRouteInfo mqInfo, T data);
    }
}
