/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * FormVerificationConfig Description:
 * 表单验证配置
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
    /// 表单验证配置
    /// </summary>
    public class FormExtendConfig
    {
        /// <summary>
        /// 显示字段key
        /// </summary>
        public string DisplayFieldKey { get; set; }

        /// <summary>
        /// 约束字段列表
        /// </summary>
        public List<FormUniqueConfig> UniqueFields { get; set; }
    }
}
