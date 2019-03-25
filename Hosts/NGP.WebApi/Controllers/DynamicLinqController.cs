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

namespace NGP.WebApi
{
    /// <summary>
    /// 动态数据控制器
    /// </summary>
    public class DynamicLinqController : ApiController
    {
        /// <summary>
        /// 动态数据服务
        /// </summary>
        private readonly IDynamicLinqService _dynamicLinqService;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="dynamicLinqService"></param>
        public DynamicLinqController(IDynamicLinqService dynamicLinqService)
        {
            _dynamicLinqService = dynamicLinqService;
        }

        /// <summary>
        /// 动态linq执行
        /// </summary>
        /// <param name="request">请求对象</param>
        /// <returns>查询结果</returns>
        [HttpPost("extute")]
        public ActionResult<NGPResponse<dynamic>> Extute(DynamicLinqRequest request)
        {
            return Ok(_dynamicLinqService.Extute(request));
        }
    }
}
