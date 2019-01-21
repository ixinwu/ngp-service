/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * BaseApiController Description:
 * 接口基类
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-2-28   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NGP.Framework.WebApi.Core
{
    /// <summary>
    /// 接口基类
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public abstract class ApiController : ControllerBase
    {

    }
}
