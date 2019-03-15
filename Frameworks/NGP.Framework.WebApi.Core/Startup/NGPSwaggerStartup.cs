/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * NGPSwaggerStartup.cs Description:
 * Swagger配置启动器
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-28   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NGP.Framework.Core;
using Swashbuckle.AspNetCore.Swagger;

namespace NGP.Framework.WebApi.Core
{
    /// <summary>
    /// Swagger配置启动器
    /// </summary>
    public class NGPSwaggerStartup : INGPStartup
    {
        /// <summary>
        /// 添加Swagger配置启动
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = GlobalConst.__ApiName
                });

                // 获取xml
                var files = CommonHelper.DefaultFileProvider.GetFiles(CommonHelper.DefaultFileProvider.MapPath(GlobalConst.__CodeXmlDocuments), "*.xml");
                foreach (var file in files)
                {
                    c.IncludeXmlComments(file);
                }

                // Swagger认证拦截器
                c.OperationFilter<SwaggerAuthorizationOperationFilter>();

                // swagger文件上传拦截器
                c.OperationFilter<SwaggerFileUploadOperationFilter>();
            });
        }

        /// <summary>
        /// 配置api中间件
        /// </summary>
        /// <param name="application"></param>
        public void Configure(IApplicationBuilder application)
        {

            // 启用中间件以将生成的Swagger作为JSON端点提供服务。
            application.UseSwagger();

            // 启用中间件以提供swagger-ui（HTML，JS，CSS等），指定Swagger JSON端点。
            application.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(GlobalConst.__SwaggerJson, GlobalConst.__ApiName);
            });
        }

        /// <summary>
        /// 配置顺序
        /// </summary>
        public int Order
        {
            get { return 1050; }
        }
    }
}