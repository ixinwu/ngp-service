/* ---------------------------------------------------------------------    
 * Copyright:
 * Wuxi Efficient Technology Co., Ltd. All rights reserved. 
 * 
 * Class Description:
 * 
 *
 * Comment 					        Revision	        Date                 Author
 * -----------------------------    --------         --------            -----------
 * Created							1.0		    2019/3/4 15:37:41   rock@xcloudbiz.com
 *
 * ------------------------------------------------------------------------------*/


using System;

namespace NGP.Framework.Core
{
    /// <summary>
    /// 服务运行配置
    /// </summary>
    public partial class Sys_Config_ServiceRunning : BaseDBEntity
    {
        /// <summary>
        /// 配置Key
        /// </summary>
        public string ConfigKey { get; set; }
        /// <summary>
        /// 配置名称
        /// </summary>
        public string ConfigName { get; set; }
        /// <summary>
        /// 有效起始时间
        /// </summary>
        public DateTime? ValidStartTime { get; set; }
        /// <summary>
        /// 有效截止时间
        /// </summary>
        public DateTime? ValidEndTime { get; set; }
        /// <summary>
        /// 是否可用
        /// </summary>
        public bool IsEnable { get; set; }
        /// <summary>
        /// Cron配置
        /// </summary>
        public string CronConfig { get; set; }
    }
}