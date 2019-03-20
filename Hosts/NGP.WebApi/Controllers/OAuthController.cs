/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * OAuthController Description:
 * 认证服务
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-2-28   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using NGP.Foundation.Identity;
using Microsoft.AspNetCore.Mvc;
using NGP.Framework.WebApi.Core;
using NGP.Framework.Core;

namespace NGP.WebApi
{
    /// <summary>
    /// 认证服务
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class OAuthController : ControllerBase
    {
        /// <summary>
        /// config
        /// </summary>
        private readonly INGPAuthenticationService _userService;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="userService"></param>
        public OAuthController(INGPAuthenticationService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// 验证获取token
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        [HttpPost("token")]
        public ActionResult<NGPResponse<TokenReponse>> Token([FromBody]TokenRequest userDto)
        {
            var result = _userService.Certification(userDto);
            return Ok(result);
        }
    }
}
