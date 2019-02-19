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
        /// <param name="defaultSortName">默认排序字段</param>
        /// <param name="defaultSort">默认排序方向</param>
        public static void ResetPageQuery<T>(this NGPPageQueryRequest<T> query,
            string defaultSortExpression = "UpdatedTime DESC") where T : INGPRequest
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
            query.LikeValue = query.LikeValue ?? "";
            if (string.IsNullOrWhiteSpace(query.SortExpression))
            {
                query.SortExpression = defaultSortExpression;
            }
        }

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

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="source">查询源</param>
        /// <param name="queryInfo">查询对象</param>
        /// <param name="actionSet">设置值回调</param>
        /// <returns>查询结果</returns>
        public static NGPPageQueryResponse<TReponse> ParsePageQuery<TRequest, TReponse>(this IQueryable<TReponse> source,
            NGPPageQueryRequest<TRequest> query)
            where TRequest : INGPRequest
        {
            var result = new NGPPageQueryResponse<TReponse>();

            if (query == null)
            {
                return result;
            }

            query.ResetPageQuery();
            //起始页
            var startIndex = (query.PageNumber - 1) * query.PageSize;
            // 执行分页
            var pageSource = source.OrderBy(query.SortExpression)
                    .Skip(startIndex)
                    .Take(query.PageSize);

            // 总条数
            result.TotalCount = source.Select(s => 1).Count();

            // 行数据
            result.Data = pageSource.ToList();

            return result;
        }
    }
}
