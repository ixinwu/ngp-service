/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * HttpRequestExtension Description:
 * http请求扩展
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-20   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using NGP.Framework.Core;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace NGP.Framework.WebApi.Core
{
    /// <summary>
    /// http请求扩展
    /// </summary>
   public static  class HttpRequestExtension
    {

        /// <summary>
        /// 获取指定的HTTP请求URI是否引用本地主机。
        /// </summary>
        /// <param name="req">HTTP request</param>
        /// <returns>如果HTTP请求URI引用本地主机，则为True</returns>
        public static bool IsLocalRequest(this HttpRequest req, IWebHelper webHelper)
        {
            //source: https://stackoverflow.com/a/41242493/7860424
            var connection = req.HttpContext.Connection;
            if (webHelper.IsIpAddressSet(connection.RemoteIpAddress))
            {
                // 远程地址
                return webHelper.IsIpAddressSet(connection.LocalIpAddress)
                    // 本地是否与远程相同，那么是本地的
                    ? connection.RemoteIpAddress.Equals(connection.LocalIpAddress)
                    // 否则，如果远程IP地址不是环回地址，我们就是远程的
                    : IPAddress.IsLoopback(connection.RemoteIpAddress);
            }

            return true;
        }

        /// <summary>
        /// 获取原始路径和完整的请求查询
        /// </summary>
        /// <param name="request">HTTP request</param>
        /// <returns>Raw URL</returns>
        public static string GetRawUrl(this HttpRequest request)
        {
            //首先尝试从请求功能获取原始目标
            var rawUrl = request.HttpContext.Features.Get<IHttpRequestFeature>()?.RawTarget;

            // 或手动撰写原始URL
            if (string.IsNullOrEmpty(rawUrl))
                rawUrl = $"{request.PathBase}{request.Path}{request.QueryString}";

            return rawUrl;
        }
    }
}
