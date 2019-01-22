/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * RequestContextMiddleware Description:
 * 请求上下文中间件
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-28   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using Microsoft.AspNetCore.Http;
using NGP.Framework.Core;
using System.Threading.Tasks;

namespace NGP.Framework.WebApi.Core
{
    /// <summary>
    /// 请求上下文中间件
    /// </summary>
    public class RequestContextMiddleware
    {
        #region Fields

        private readonly RequestDelegate _next;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="next">Next</param>
        public RequestContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Invoke middleware actions
        /// </summary>
        /// <param name="context">HTTP context</param>
        /// <param name="webHelper">Web helper</param>
        /// <param name="workContext">Work context</param>
        /// <returns>Task</returns>
        public Task Invoke(HttpContext context, IWebHelper webHelper, IWorkContext workContext)
        {
            var request = context.Request;
            //var body = request.Body;

            //// 这一行允许我们在其流的开头设置读取器。
            //request.EnableRewind();

            //// 读取请求流。 创建一个与请求流长度相同的新byte []
            //var buffer = new byte[Convert.ToInt32(request.ContentLength)];

            //// 将整个请求流复制到新缓冲区中
            //request.Body.ReadAsync(buffer, 0, buffer.Length);

            //// 使用UTF8编码将byte []转换为字符串
            //var bodyAsText = Encoding.UTF8.GetString(buffer);

            //// 将读取的主体分配回请求主体，由于EnableRewind（），这是允许的
            //request.Body = body;

            // 设定上下文
            workContext.CurrentRequest = new WorkRequest
            {
                IpAddress = webHelper.GetCurrentIpAddress(),
                Parameter = request.QueryString.HasValue ? request.QueryString.Value : "",
                Url = webHelper.GetThisRequestUrl(true)
            };

            // 调用请求管道中的下一个中间件
            return _next(context);
        }

        #endregion
    }
}
