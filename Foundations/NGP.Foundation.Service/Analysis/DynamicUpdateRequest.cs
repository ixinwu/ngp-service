/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved.
 * 
 * DynamicDetailOperateInfo Description:
 * 动态详情页面操作对象
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
    /// 动态详情页面操作对象
    /// </summary>
    public class DynamicUpdateRequest : DynamicBaseRequest
    {
        /// <summary>
        /// 主键值
        /// </summary>
        public string PrimaryKeyValue { get; set; }

        /// <summary>
        /// 操作字段列表
        /// </summary>
        public List<DynamicAddRequest> OperateFields { get; set; }
    }
}
