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
        #region methods
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
            if (query == null || query.RequestData == null)
            {
                return new NGPResponse<NGPPageQueryResponse>
                {
                    AffectedRows = 0,
                    ErrorCode = ErrorCode.ParamEmpty,
                    Message = CommonResource.ParameterError,
                    Status = OperateStatus.Error
                };
            }

            var context = new QueryResolveContext
            {
                Request = query.RequestData,
                PageQueryRequest = query,
            };
            context.GenerateContext.ExtendSetItem = setItem;
            context.GenerateContext.ExtendTypes = extendTypes;

            ResolveProcessorFactory.PageQueryStep.HandleProcess(context);
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
            if (query == null)
            {
                return new NGPResponse<dynamic>
                {
                    AffectedRows = 0,
                    ErrorCode = ErrorCode.ParamEmpty,
                    Message = CommonResource.ParameterError,
                    Status = OperateStatus.Error
                };
            }

            var context = new QueryResolveContext
            {
                Request = query,
            };
            context.GenerateContext.ExtendSetItem = setItem;
            context.GenerateContext.ExtendTypes = extendTypes;

            ResolveProcessorFactory.AllQueryStep.HandleProcess(context);
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
            if (query == null)
            {
                return new NGPResponse<dynamic>
                {
                    AffectedRows = 0,
                    ErrorCode = ErrorCode.ParamEmpty,
                    Message = CommonResource.ParameterError,
                    Status = OperateStatus.Error
                };
            }

            var context = new QueryResolveContext
            {
                Request = query,
            };
            context.GenerateContext.ExtendSetItem = setItem;
            context.GenerateContext.ExtendTypes = extendTypes;

            ResolveProcessorFactory.SingleQueryStep.HandleProcess(context);
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
        /// <returns>操作结果</returns>
        public NGPResponse<List<NGPKeyValuePair>> InsertDynamicData(DynamicInsertRequest info)
        {
            if (info == null)
            {
                return new NGPResponse<List<NGPKeyValuePair>>
                {
                    AffectedRows = 0,
                    ErrorCode = ErrorCode.ParamEmpty,
                    Message = CommonResource.ParameterError,
                    Status = OperateStatus.Error,
                    Data = new List<NGPKeyValuePair>()
                };
            }

            var context = new OperatorResolveContext<DynamicInsertRequest>()
            {
                Request = info
            };

            ResolveProcessorFactory.InsertSetp.HandleProcess(context);
            if (context.Response.Status == OperateStatus.Error)
            {
                return new NGPResponse<List<NGPKeyValuePair>>
                {
                    AffectedRows = context.Response.AffectedRows,
                    ErrorCode = context.Response.ErrorCode,
                    Message = context.Response.Message,
                    Status = context.Response.Status,
                    Data = new List<NGPKeyValuePair>()
                };
            }
            return new NGPResponse<List<NGPKeyValuePair>>
            {
                AffectedRows = context.Response.AffectedRows,
                ErrorCode = context.Response.ErrorCode,
                Message = context.Response.Message,
                Status = context.Response.Status,
                Data = context.InsertPrimaryKeys
            };
        }

        /// <summary>
        /// 更新动态数据
        /// </summary>
        /// <param name="info">更新对象</param>
        /// <returns>操作结果</returns>
        public NGPResponse UpdateDynamicData(DynamicUpdateRequest info)
        {
            if (info == null)
            {
                return new NGPResponse
                {
                    AffectedRows = 0,
                    ErrorCode = ErrorCode.ParamEmpty,
                    Message = CommonResource.ParameterError,
                    Status = OperateStatus.Error
                };
            }

            var context = new OperatorResolveContext<DynamicUpdateRequest>()
            {
                Request = info
            };

            ResolveProcessorFactory.UpdateSetp.HandleProcess(context);
            return context.Response;
        }

        /// <summary>
        /// 删除动态数据(包含详情)
        /// </summary>
        /// <param name="info">删除对象</param>
        /// <returns>操作结果</returns>
        public NGPResponse DeleteDynamicData(DynamicDeleteRequest info)
        {
            if (info == null)
            {
                return new NGPResponse
                {
                    AffectedRows = 0,
                    ErrorCode = ErrorCode.ParamEmpty,
                    Message = CommonResource.ParameterError,
                    Status = OperateStatus.Error
                };
            }

            var context = new OperatorResolveContext<DynamicDeleteRequest>()
            {
                Request = info
            };

            ResolveProcessorFactory.DeleteSetp.HandleProcess(context);
            return context.Response;
        }
        #endregion
    }
}
