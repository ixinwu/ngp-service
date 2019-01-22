﻿/* ---------------------------------------------------------------------    
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

using Microsoft.AspNetCore.Mvc;
using NGP.Foundation.Service.System;
using NGP.Framework.WebApi.Core;

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
        private readonly IUserService _userService;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="ngpConfig"></param>
        public OAuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("token")]
        public IActionResult Token([FromBody]TokenRequest userDto)
        {
            var result = _userService.Certification(userDto);
            return Ok(result);
        }
    }
}
