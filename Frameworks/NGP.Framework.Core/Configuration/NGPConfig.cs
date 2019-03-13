/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * NGPConfig Description:
 * 系统配置
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-15   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

namespace NGP.Framework.Core
{
    /// <summary>
    /// 启动时的配置参数
    /// </summary>
    public partial class NGPConfig
    {
        /// <summary>
        /// 获取或设置一个值，该值指示是否应使用Redis服务器进行缓存（默认是内存缓存）
        /// </summary>
        public bool RedisCachingEnabled { get; set; }
        /// <summary>
        /// 获取redis缓存连接串
        /// </summary>
        public string RedisCachingConnectionString { get; set; }

        /// <summary>
        /// 加密Secret
        /// </summary>
        public string Secret { get; set; }

        /// <summary>
        /// token过期时间（单位小时）
        /// </summary>
        public string TokenExpiresHour { get; set; }

        /// <summary>
        /// 是否启用日志
        /// </summary>
        public bool LogEnabled { get; set; }
    }
}