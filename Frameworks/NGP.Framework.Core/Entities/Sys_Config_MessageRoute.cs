/* ---------------------------------------------------------------------    
 * Copyright:
 * Wuxi Efficient Technology Co., Ltd. All rights reserved. 
 * 
 * Class Description:
 * 
 *
 * Comment 					        Revision	        Date                 Author
 * -----------------------------    --------         --------            -----------
 * Created							1.0		    2019/3/4 15:37:39   rock@xcloudbiz.com
 *
 * ------------------------------------------------------------------------------*/

namespace NGP.Framework.Core
{
    /// <summary>
    /// 消息路由配置
    /// </summary>
    public partial class Sys_Config_MessageRoute : BaseDBEntity
    {
        /// <summary>
        /// 消息队列key
        /// </summary>
        public string MessageRouteKey { get; set; }

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