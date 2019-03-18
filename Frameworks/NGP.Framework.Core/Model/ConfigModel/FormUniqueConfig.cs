/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * FormUniqueConfig Description:
 * 表单唯一性验证
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
    /// 表单唯一性验证
    /// </summary>
    public class FormUniqueConfig
    {
        /// <summary>
        /// 唯一字段key
        /// </summary>
        public string FieldKey { get; set; }

        /// <summary>
        /// 字段唯一范围key
        /// </summary>
        public List<string> ScopeFieldKeys { get; set; }
    }
}
