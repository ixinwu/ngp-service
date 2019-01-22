/* ------------------------------------------------------------------------------
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * DynamicSingleQueryRequest Description:
 * 动态单条查询请求
 *
 * Comment 					        Revision	Date                  Author
 * -----------------------------    --------    ------------------    ----------------
 * Created							1.0		    2019/1/22 16:53:09    hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/


namespace NGP.Foundation.Service.Analysis
{
    /// <summary>
    /// 动态单条查询请求
    /// </summary>
    public class DynamicSingleQueryRequest : DynamicBaseRequest
    {
        /// <summary>
        /// 主键值
        /// </summary>
        public string PrimaryKeyValue { get; set; }
    }
}
