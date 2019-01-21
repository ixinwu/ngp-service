/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * AuthenticationStartup Description:
 * 身份认证启动器
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-28   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NGP.Framework.Core;
using System.Text;
using System.Threading.Tasks;

namespace NGP.Framework.WebApi.Core
{
    /// <summary>
    /// 身份认证启动器
    /// </summary>
    public class AuthenticationStartup : INGPStartup
    {
        /// <summary>
        /// 添加和配置身份认证
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            // 获取ngp配置
            var config = services.BuildServiceProvider().GetRequiredService<NGPConfig>();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.Audience = "api";
                o.Events = new JwtBearerEvents()
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Query["access_token"];
                        return Task.CompletedTask;
                    },
                };

                o.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = JwtClaimTypes.Name,
                    RoleClaimType = JwtClaimTypes.Role,

                    // 用于适配本地Token
                    ValidateIssuer = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(config.Secret))
                };
            }); ;
        }

        /// <summary>
        /// 配置添加中间件的使用
        /// </summary>
        /// <param name="application"></param>
        public void Configure(IApplicationBuilder application)
        {
            // 配置身份认证并且添加身份认证中间件
            application.UseAuthentication();
            application.UseMiddleware<AuthenticationMiddleware>();

            // 添加区域信息中间件
            application.UseMiddleware<CultureMiddleware>();
        }

        /// <summary>
        /// 配置启动顺序
        /// </summary>
        public int Order
        {
            //应在api之前加载身份验证
            get { return 500; }
        }
    }
}
