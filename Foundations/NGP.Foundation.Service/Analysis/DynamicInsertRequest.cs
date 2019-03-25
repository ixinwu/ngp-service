/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved.
 * 
 * DynamicInsertRequest Description:
 * 动态追加请求对象
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
    /// 动态追加请求对象
    /// </summary>
    public class DynamicInsertRequest : DynamicBaseRequest
    {
        /// <summary>
        /// 操作字段列表
        /// </summary>
        public List<DynamicOperateFieldRequest> OperateFields { get; set; }
    }
}
