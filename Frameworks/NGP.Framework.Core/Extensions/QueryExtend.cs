/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * PageQueryExtend Description:
 * 分页扩展
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-2-15   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using System.Linq;
using System.Linq.Dynamic.Core;

namespace NGP.Framework.Core
{
    /// <summary>
    /// 分页扩展
    /// </summary>
    public static class QueryExtend
    {
        /// <summary>
        /// 重置分页对象
        /// </summary>
        /// <param name="query">查询对象</param>
        public static void ResetPageQuery<T>(this NGPPageQueryRequest<T> query) where T : INGPRequest
        {
            //充值当前页码
            if (query.PageNumber == 0)
            {
                query.PageNumber = 1;
            }
            //分页大小默认为10
            if (query.PageSize == 0)
            {
                query.PageSize = 10;
            }
        }
    }
}
