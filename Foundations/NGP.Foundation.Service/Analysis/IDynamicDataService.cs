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
        /// <param name="query">查询对象</param>
        /// <param name="extendTypes">扩展类型定义</param>
        /// <param name="setItem">设定值回调</param>
        /// <returns>查询结果</returns>
        NGPResponse<NGPPageQueryResponse> QueryDynamicListPageData(NGPPageQueryRequest<DynamicQueryRequest> query,
            IEnumerable<DynamicGenerateObject> extendTypes = null,
            Action<dynamic> setItem = null);

        /// <summary>
        /// 获取列表页面数据
        /// </summary>
        /// <param name="query">查询对象</param>
        /// <param name="extendTypes">扩展类型定义</param>
        /// <param name="setItem">设定值回调</param>
        /// <returns>查询结果</returns>
        NGPResponse<dynamic> QueryDynamicAllData(DynamicQueryRequest query,
            IEnumerable<DynamicGenerateObject> extendTypes = null,
            Action<dynamic> setItem = null);

        /// <summary>
        /// 获取列表页面数据
        /// </summary>
        /// <param name="query">查询对象</param>
        /// <param name="extendTypes">扩展类型定义</param>
        /// <param name="setItem">设定值回调</param>
        /// <returns>查询结果</returns>
        NGPResponse<dynamic> QueryDynamicSingleData(DynamicQueryRequest query,
            IEnumerable<DynamicGenerateObject> extendTypes = null,
            Action<dynamic> setItem = null);

        /// <summary>
        /// 通过主键获取详情数据
        /// </summary>
        /// <param name="query">查询对象</param>
        /// <param name="extendTypes">扩展类型定义</param>
        /// <param name="setItem">设定值回调</param>
        /// <returns>查询结果</returns>
        NGPResponse<dynamic> QueryDynamicSingleDataByKey(DynamicQueryRequest query,
            IEnumerable<DynamicGenerateObject> extendTypes = null,
            Action<dynamic> setItem = null);

        /// <summary>
        /// 添加动态数据
        /// </summary>
        /// <param name="info">追加对象</param>
        /// <param name="extendFunc">扩展操作(返回影响行数)</param>
        /// <param name = "extendRelationFunc" > 扩展操作关联信息(返回影响行数) </param >
        /// <returns>操作结果</returns>
        NGPResponse AddDynamicData(DynamicAddRequest info,
            Func<int> extendFunc = null,
            Func<string, int> extendRelationFunc = null);

        /// <summary>
        /// 更新动态数据
        /// </summary>
        /// <param name="info">更新对象</param>
        /// <param name="extendFunc">扩展更新(返回影响行数)</param>
        /// <returns>操作结果</returns>
        NGPResponse UpdateDynamicData(DynamicUpdateRequest info,
            Func<int> extendFunc = null);
    }
}
