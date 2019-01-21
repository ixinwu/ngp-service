/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * MessageRouteInfo Description:
 * 消息路由信息
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-15   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/


namespace NGP.Framework.Core
{
    /// <summary>
    /// 消息路由信息
    /// </summary>
    public class MessageRouteInfo
    {
        /// <summary>
        /// 主机地址
        /// </summary>
        public string HostName { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 交换机名称
        /// </summary>
        public string ExchangeName { get; set; }

        /// <summary>
        /// 交换机类型
        /// </summary>
        public string ExchangeType { get; set; }

        /// <summary>
        /// 队列名称
        /// </summary>
        public string QueueName { get; set; }

        /// <summary>
        /// 队列是否持久化
        /// </summary>
        public bool QueueDurable { get; set; }

        /// <summary>
        /// 路由Key
        /// </summary>
        public string RouteKey { get; set; }
    }
}
