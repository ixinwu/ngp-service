/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * IDynamicDataService Description:
 * 动态解析数据接口
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019/1/22 9:07:18   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using NGP.Framework.Core;
using System;
using System.Collections.Generic;

namespace NGP.Foundation.Service.Analysis
{
    /// <summary>
    /// 动态解析数据接口
    /// </summary>
    public interface IDynamicDataService
    {
        /// <summary>
        /// 删除动态数据(包含详情)
        /// </summary>
        /// <param name="info">删除对象</param>
        /// <param name="extendFunc">扩展删除(返回影响行数)</param>
        /// <returns>操作结果</returns>
        NGPResponse<List<string>> DeleteDynamicData(DynamicDeleteRequest info,
            Func<int> extendFunc = null);

        /// <summary>
        /// 获取列表页面数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        NGPResponse<DynamicListReponse> QueryDynamicListPageData(NGPPageQueryRequest<DynamicQueryRequest> query, Action<dynamic> action);

        /// <summary>
        /// 获取列表页面数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        NGPResponse<DynamicListReponse> QueryDynamicAllData(DynamicQueryRequest query);

        /// <summary>
        /// 获取列表页面数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        NGPResponse<dynamic> QueryDynamicSingleData(DynamicQueryRequest query);
    }
}
