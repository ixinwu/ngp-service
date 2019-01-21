/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * AuthenticationMiddleware Description:
 * 身份认证中间件
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-28   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using System;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NGP.Framework.Core;

namespace NGP.Framework.WebApi.Core
{
    /// <summary>
    /// 身份认证中间件
    /// </summary>
    public class AuthenticationMiddleware
    {
        #region Fields
        /// <summary>
        /// 请求委托
        /// </summary>
        private readonly RequestDelegate _next;

        #endregion

        #region Ctor
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="schemes"></param>
        /// <param name="next"></param>
        public AuthenticationMiddleware(IAuthenticationSchemeProvider schemes, RequestDelegate next)
        {
            Schemes = schemes ?? throw new ArgumentNullException(nameof(schemes));
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        #endregion

        #region Properties

        /// <summary>
        /// 认证方案提供者
        /// </summary>
        public IAuthenticationSchemeProvider Schemes { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// 调用中间件操作
        /// </summary>
        /// <param name="context">HTTP context</param>
        /// <param name="workContext">工作上下文</param>
        /// <returns>Task</returns>
        public async Task Invoke(HttpContext context, IWorkContext workContext)
        {
            context.Features.Set<IAuthenticationFeature>(new AuthenticationFeature
            {
                OriginalPath = context.Request.Path,
                OriginalPathBase = context.Request.PathBase
            });

            // 为任何IAuthenticationRequestHandler方案提供处理请求的机会
            var handlers = context.RequestServices.GetRequiredService<IAuthenticationHandlerProvider>();
            foreach (var scheme in await Schemes.GetRequestHandlerSchemesAsync())
            {
                try
                {
                    if (await handlers.GetHandlerAsync(context, scheme.Name) is IAuthenticationRequestHandler handler && await handler.HandleRequestAsync())
                        return;
                }
                catch
                {
                    // ignored
                }
            }

            var defaultAuthenticate = await Schemes.GetDefaultAuthenticateSchemeAsync();
            if (defaultAuthenticate != null)
            {
                var result = await context.AuthenticateAsync(defaultAuthenticate.Name);
                if (result?.Principal != null)
                {
                    context.User = result.Principal;

                    // 设定上下文用户对象
                    workContext.Current = new WorkEmployee();
                    foreach (var item in result.Principal.Claims)
                    {
                        switch (item.Type)
                        {
                            case "DeptId":
                                workContext.Current.DeptId = item.Value;
                                break;
                            case JwtClaimTypes.Id:
                                workContext.Current.EmplId = item.Value;
                                break;
                            case JwtClaimTypes.Name:
                                workContext.Current.EmplName = item.Value;
                                break;
                            case "EmplNo":
                                workContext.Current.EmplNo = item.Value;
                                break;
                            case "LoginName":
                                workContext.Current.LoginName = item.Value;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }

            await _next(context);
        }

        #endregion
    }
}