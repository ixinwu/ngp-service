/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved.
 * 
 * DynamicDeleteRequest Description:
 * 动态删除请求对象
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019/2/19  hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using System.Collections.Generic;

namespace NGP.Foundation.Service.Analysis
{
    /// <summary>
    /// 动态删除请求对象
    /// </summary>
    public class DynamicDeleteRequest : DynamicBaseRequest
    {
        /// <summary>
        /// 主键key
        /// </summary>
        public string PrimaryFieldKey { get; set; }

        /// <summary>
        /// key
        /// </summary>
        public List<string> KeyValues { get; set; }

        /// <summary>
        /// 是否删除子集
        /// </summary>
        public bool IsDeleteSubsets { get; set; }
    }
}
