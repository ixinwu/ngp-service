/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * FieldBusinessConfig Description:
 * 字段业务配置
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-15   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/


namespace NGP.Framework.Core
{
    /// <summary>
    /// 字段业务配置
    /// </summary>
    public class FieldBusinessConfig
    {
        /// <summary>
        /// 组key
        /// </summary>
        public string GroupKey { get; set; }

        /// <summary>
        /// 是否系统字段
        /// </summary>
        public bool? IsSystemField { get; set; }
    }
}
