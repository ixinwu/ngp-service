/* ------------------------------------------------------------------------------
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * ServiceJobDescriptor Description:
 * 服务项描述
 *
 * Comment 					        Revision	Date                  Author
 * -----------------------------    --------    ------------------    ----------------
 * Created							1.0		    2019/2/26 10:56:02    hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

namespace NGP.Framework.Core
{
    /// <summary>
    /// 服务项描述
    /// </summary>
    public class ServiceJobDescriptor : BaseEntity
    {
        /// <summary>
        /// 应用Key
        /// </summary>
        public string AppKey { get; set; }

        /// <summary>
        /// 服务Key
        /// </summary>
        public string ServiceKey { get; set; }

        /// <summary>
        /// 服务类型
        /// </summary>
        public string ServiceType { get; set; }

        /// <summary>
        /// 运行配置对象
        /// </summary>
        public ServiceRunningDescriptor WorkConfig { get; set; }

        /// <summary>
        /// 策略执行方法
        /// </summary>
        public IServiceCommand Command { get; set; }

        /// <summary>
        /// 重复次数
        /// </summary>
        public int? RepeatCount { get; set; }
    }
}
