/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved.
 * 
 * DynamicUpdateRequest Description:
 * 动态更新请求对象
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2017/3/14  hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using System.Collections.Generic;

namespace NGP.Foundation.Service.Analysis
{
    /// <summary>
    /// 动态更新请求对象
    /// </summary>
    public class DynamicUpdateRequest : DynamicBaseRequest
    {
        /// <summary>
        /// 操作字段列表
        /// </summary>
        public List<DynamicOperateFieldRequest> OperateFields { get; set; }

        /// <summary>
        /// 查询表达式
        /// </summary>
        public List<string> WhereExpressions { get; set; }
    }
}
