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
    public class FieldExtendConfig
    {
        /// <summary>
        /// 组key
        /// </summary>
        public string GroupKey { get; set; }

        /// <summary>
        /// 是否必须
        /// </summary>
        public bool? IsRequired { get; set; }
    }
}
