/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * BasePageQueryInfo Description:
 * 分页查询模型
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-2-15   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

namespace NGP.Framework.Core
{
    /// <summary>
    /// 分页查询模型
    /// </summary>
    public class NGPPageQueryRequest<T> where T : INGPRequest
    {
        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// 查询条件(文本框输入的模糊查询条件)
        /// </summary>
        public string LikeValue { get; set; }

        /// <summary>
        /// 排序表达式
        /// </summary>
        public string SortExpression { get; set; }

        /// <summary>
        /// 请求数据
        /// </summary>
        public T RequestData { get; set; }
    }
}
