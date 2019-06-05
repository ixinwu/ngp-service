/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * IDynamicDataService Description:
 * 动态数据服务接口
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
    /// 动态数据服务接口
    /// </summary>
    public interface IDynamicDataService
    {
        /// <summary>
        /// 获取列表页面数据
        /// </summary>
        /// <param name="query">查询对象</param>
        /// <returns>查询结果</returns>
        NGPResponse<NGPPageQueryResponse> QueryDynamicListPageData(NGPPageQueryRequest<DynamicQueryRequest> query);

        /// <summary>
        /// 获取列表页面数据
        /// </summary>
        /// <param name="query">查询对象</param>
        /// <returns>查询结果</returns>
        NGPResponse<dynamic> QueryDynamicAllData(DynamicQueryRequest query);

        /// <summary>
        /// 获取列表页面数据
        /// </summary>
        /// <param name="query">查询对象</param>
        /// <returns>查询结果</returns>
        NGPResponse<dynamic> QueryDynamicSingleData(DynamicQueryRequest query);

        /// <summary>
        /// 添加动态数据
        /// </summary>
        /// <param name="info">追加对象</param>
        /// <returns>操作结果</returns>
        NGPResponse<List<NGPKeyValuePair>> InsertDynamicData(DynamicInsertRequest info);

        /// <summary>
        /// 批量添加动态数据
        /// </summary>
        /// <param name="info">追加对象</param>
        /// <returns>操作结果</returns>
        NGPResponse<List<List<NGPKeyValuePair>>> BulkInsertDynamicData(DynamicBulkInsertRequest info);

        /// <summary>
        /// 更新动态数据
        /// </summary>
        /// <param name="info">更新对象</param>
        /// <returns>操作结果</returns>
        NGPResponse UpdateDynamicData(DynamicUpdateRequest info);

        /// <summary>
        /// 删除动态数据(包含详情)
        /// </summary>
        /// <param name="info">删除对象</param>
        /// <returns>操作结果</returns>
        NGPResponse DeleteDynamicData(DynamicDeleteRequest info);
    }
}
