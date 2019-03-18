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
    public class FormVerificationConfig
    {
        /// <summary>
        /// 约束字段列表
        /// </summary>
        public List<FormUniqueConfig> UniqueFields { get; set; }
    }
}
