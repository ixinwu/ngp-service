/* ------------------------------------------------------------------------------
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * DynamicPageQueryReponse Description:
 * 动态分页查询返回
 *
 * Comment 					        Revision	Date                  Author
 * -----------------------------    --------    ------------------    ----------------
 * Created							1.0		    2019/1/22 10:29:45    hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

namespace NGP.Foundation.Service.Analysis
{
    /// <summary>
    /// 动态分页查询返回
    /// </summary>
    public class DynamicPageQueryReponse
    {
        /// <summary>
        /// 数据总数
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// 数据列表
        /// </summary>
        public dynamic RowData { get; set; }
    }
}
