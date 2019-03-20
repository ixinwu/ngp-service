/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * IDynamicDataService Description:
 * 动态数据服务
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019/1/22 9:07:18   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using NGP.Foundation.Resources;
using NGP.Framework.Core;
using System;
using System.Collections.Generic;

namespace NGP.Foundation.Service.Analysis
{
    /// <summary>
    /// 动态数据服务
    /// </summary>
    [ExceptionCallHandler]
    public class DynamicDataService : IDynamicDataService
    {
        #region private fields
        /// <summary>
        /// 工作上下文
        /// </summary>
        private readonly IWorkContext _workContext;

        /// <summary>
        /// 仓储
        /// </summary>
        private readonly IUnitRepository _unitRepository;
        #endregion

        #region ctor
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="workContext"></param>
        /// <param name="unitRepository"></param>
        public DynamicDataService(IWorkContext workContext, IUnitRepository unitRepository)
        {
            _workContext = workContext;
            _unitRepository = unitRepository;
        }
        #endregion

        #region methods
        /// <summary>
        /// 删除动态数据(包含详情)
        /// </summary>
        /// <param name="info">删除对象</param>
        /// <param name="extendFunc">扩展删除(返回影响行数)</param>
        /// <returns>操作结果</returns>
        public NGPResponse<List<string>> DeleteDynamicData(DynamicDeleteRequest info,
            Func<int> extendFunc = null)
        {
            return null;
        }

        /// <summary>
        /// 获取列表页面数据
        /// </summary>
        /// <param name="query">查询对象</param>
        /// <param name="extendTypes">扩展类型定义</param>
        /// <param name="setItem">设定值回调</param>
        /// <returns>查询结果</returns>
        public NGPResponse<NGPPageQueryResponse> QueryDynamicListPageData(NGPPageQueryRequest<DynamicQueryRequest> query,
            IEnumerable<DynamicGenerateObject> extendTypes = null,
            Action<dynamic> setItem = null)
        {
            var context = new QueryResolveContext
            {
                Request = query.RequestData,
                PageQueryRequest = query,
            };
            context.GenerateContext.ExtendSetItem = setItem;
            context.GenerateContext.ExtendTypes = extendTypes;

            ResolveProcessorFactory.StepResolveQuery.HandleProcess(context);
            return new NGPResponse<NGPPageQueryResponse>
            {
                Message = CommonResource.OperatorSuccess,
                Status = OperateStatus.Success,
                Data = context.Response
            };
        }

        /// <summary>
        /// 获取列表页面数据
        /// </summary>
        /// <param name="query">查询对象</param>
        /// <param name="extendTypes">扩展类型定义</param>
        /// <param name="setItem">设定值回调</param>
        /// <returns>查询结果</returns>
        public NGPResponse<dynamic> QueryDynamicAllData(DynamicQueryRequest query,
            IEnumerable<DynamicGenerateObject> extendTypes = null,
            Action<dynamic> setItem = null)
        {
            var context = new QueryResolveContext
            {
                Request = query,
            };
            context.GenerateContext.ExtendSetItem = setItem;
            context.GenerateContext.ExtendTypes = extendTypes;

            ResolveProcessorFactory.StepResolveAllQuery.HandleProcess(context);
            return new NGPResponse<dynamic>
            {
                Message = CommonResource.OperatorSuccess,
                Status = OperateStatus.Success,
                Data = context.Response.Data
            };
        }

        /// <summary>
        /// 获取列表页面数据
        /// </summary>
        /// <param name="query">查询对象</param>
        /// <param name="extendTypes">扩展类型定义</param>
        /// <param name="setItem">设定值回调</param>
        /// <returns>查询结果</returns>
        public NGPResponse<dynamic> QueryDynamicSingleData(DynamicQueryRequest query,
            IEnumerable<DynamicGenerateObject> extendTypes = null,
            Action<dynamic> setItem = null)
        {
            var context = new QueryResolveContext
            {
                Request = query,
            };
            context.GenerateContext.ExtendSetItem = setItem;
            context.GenerateContext.ExtendTypes = extendTypes;

            ResolveProcessorFactory.StepResolveSingleQuery.HandleProcess(context);
            return new NGPResponse<dynamic>
            {
                Message = CommonResource.OperatorSuccess,
                Status = OperateStatus.Success,
                Data = context.Response.Data
            };
        }

        /// <summary>
        /// 添加动态数据
        /// </summary>
        /// <param name="info">追加对象</param>
        /// <param name="extendFunc">扩展操作(返回影响行数)</param>
        /// <param name = "extendRelationFunc" > 扩展操作关联信息(返回影响行数) </param >
        /// <returns>操作结果</returns>
        public NGPResponse AddDynamicData(DynamicOperatorRequest info,
            Func<int> extendFunc = null,
            Func<string, int> extendRelationFunc = null)
        {
            return null;
        }

        /// <summary>
        /// 更新动态数据
        /// </summary>
        /// <param name="info">更新对象</param>
        /// <param name="extendFunc">扩展更新(返回影响行数)</param>
        /// <returns>操作结果</returns>
        public NGPResponse UpdateDynamicData(DynamicOperatorRequest info,
            Func<int> extendFunc = null)
        {
            return null;
        }
        #endregion
    }
}
