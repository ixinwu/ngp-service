/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * MessageRouteConfigSpecification Description:
 * 消息路由配置规则
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019/3/6    layne@xcloudbiz.com
 *
 * ------------------------------------------------------------------------------*/

using System.Linq;

namespace NGP.Framework.Core
{
    /// <summary>
    /// 消息路由配置规则
    /// </summary>
    public static class MessageQueueExtension
    {
        /// <summary>
        /// 通过业务key获取消息路由
        /// </summary>
        /// <param name="repository">仓储</param>
        /// <param name="key">业务key</param>
        /// <returns>消息路由</returns>
        public static MessageRouteInfo GetMessageRouteByBusinessKey(this IUnitRepository repository,
            string key)
        {
            var item = repository.FirstOrDefault<Sys_Config_MessageRoute>(s =>
                    s.MessageRouteKey == key);

            return item.CopyItem<Sys_Config_MessageRoute, MessageRouteInfo>();
        }
    }
}
