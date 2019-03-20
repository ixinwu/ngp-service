/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * DynamicDataController Description:
 * 动态数据控制器
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-3-7   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using Microsoft.AspNetCore.Mvc;
using NGP.Foundation.Service.Analysis;
using NGP.Framework.Core;
using NGP.Framework.WebApi.Core;
using System.Collections.Generic;

namespace NGP.WebApi
{
    /// <summary>
    /// 动态数据控制器
    /// </summary>
    public class DynamicDataController : ApiController
    {
        /// <summary>
        /// 动态数据服务
        /// </summary>
        private readonly IDynamicDataService _dynamicDataService;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="dynamicDataService"></param>
        public DynamicDataController(IDynamicDataService dynamicDataService)
        {
            _dynamicDataService = dynamicDataService;
        }

        /// <summary>
        /// 删除动态数据(包含详情)
        /// </summary>
        /// <param name="info">删除对象</param>
        /// <returns>操作结果</returns>
        [HttpPost("deleteDynamicData")]
        public ActionResult<NGPResponse<List<string>>> DeleteDynamicData(DynamicDeleteRequest info)
        {
            return Ok(_dynamicDataService.DeleteDynamicData(info));
        }

        /// <summary>
        /// 获取列表页面数据
        /// </summary>
        /// <param name="query">查询对象</param>
        /// <returns>查询结果</returns>
        [HttpPost("queryDynamicListPageData")]
        public ActionResult<NGPResponse<NGPPageQueryResponse>> QueryDynamicListPageData(NGPPageQueryRequest<DynamicQueryRequest> query)
        {
            return Ok(_dynamicDataService.QueryDynamicListPageData(query));
        }

        /// <summary>
        /// 获取列表页面数据
        /// </summary>
        /// <param name="query">查询对象</param>
        /// <returns>查询结果</returns>
        [HttpPost("queryDynamicAllData")]
        public ActionResult<NGPResponse<dynamic>> QueryDynamicAllData(DynamicQueryRequest query)
        {
            return Ok(_dynamicDataService.QueryDynamicAllData(query));
        }

        /// <summary>
        /// 获取列表页面数据
        /// </summary>
        /// <param name="query">查询对象</param>
        /// <returns>查询结果</returns>
        [HttpPost("queryDynamicSingleData")]
        public ActionResult<NGPResponse<dynamic>> QueryDynamicSingleData(DynamicQueryRequest query)
        {
            return Ok(_dynamicDataService.QueryDynamicSingleData(query));
        }

        /// <summary>
        /// 通过主键获取详情数据
        /// </summary>
        /// <param name="query">查询对象</param>
        /// <returns>查询结果</returns>
        [HttpPost("queryDynamicSingleDataByKey")]
        public ActionResult<NGPResponse<dynamic>> QueryDynamicSingleDataByKey(DynamicQueryRequest query)
        {
            return Ok(_dynamicDataService.QueryDynamicSingleDataByKey(query));
        }

        /// <summary>
        /// 添加动态数据
        /// </summary>
        /// <param name="info">追加对象</param>        
        /// <returns>操作结果</returns>
        [HttpPost("addDynamicData")]
        public ActionResult<NGPResponse> AddDynamicData(DynamicAddRequest info)
        {
            return Ok(_dynamicDataService.AddDynamicData(info));
        }

        /// <summary>
        /// 更新动态数据
        /// </summary>
        /// <param name="info">更新对象</param>
        /// <returns>操作结果</returns>
        [HttpPost("updateDynamicData")]
        public ActionResult<NGPResponse> UpdateDynamicData(DynamicUpdateRequest info)
        {
            return Ok(_dynamicDataService.UpdateDynamicData(info));
        }
    }
}
