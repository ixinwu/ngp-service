/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * Sys_Config_Common Description:
 * 通用配置
 *
 * Comment 					        Revision	        Date                 Author
 * -----------------------------    --------         --------            -----------
 * Created							1.0		    2019/6/3 15:37:41   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/


using System;

namespace NGP.Framework.Core
{
    /// <summary>
    /// 爬虫配置
    /// </summary>
    public partial class Sys_Config_Common : BaseDBEntity
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
        /// json配置
        /// </summary>
        public string ConfigJson { get; set; }
    }
}