/* ------------------------------------------------------------------------------
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * DynamicDataSetPageQueryRequest Description:
 * 动态查询请求
 *
 * Comment 					        Revision	Date                  Author
 * -----------------------------    --------    ------------------    ----------------
 * Created							1.0		    2019/1/22 16:53:09    hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/


using System.Collections.Generic;

namespace NGP.Foundation.Service.Analysis
{
    /// <summary>
    /// 动态分页查询请求
    /// </summary>
    public class DynamicQueryRequest : DynamicBaseRequest
    {
        /// <summary>
        /// 查询表达式
        /// </summary>
        public string WhereExpression { get; set; }

        /// <summary>
        /// 排序表达式
        /// </summary>
        public string SortExpression { get; set; }

        /// <summary>
        /// 查询字段key列表
        /// </summary>
        public List<string> QueryFieldKeys { get; set; }
    }
}
