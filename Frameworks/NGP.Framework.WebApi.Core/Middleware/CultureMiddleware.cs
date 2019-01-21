/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * CultureMiddleware Description:
 * 本地化中间件
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-28   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using Microsoft.AspNetCore.Http;
using NGP.Framework.Core;
using System.Globalization;
using System.Threading.Tasks;

namespace NGP.Framework.WebApi.Core
{
    /// <summary>
    /// 本地化中间件
    /// </summary>
    public class CultureMiddleware
    {
        #region Fields

        private readonly RequestDelegate _next;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="next">Next</param>
        public CultureMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        #endregion

        #region Methods
        /// <summary>
        /// 调用中间件操作
        /// </summary>
        /// <param name="context">HTTP context</param>
        /// <param name="workContext">工作上下文</param>
        /// <returns>Task</returns>
        public Task Invoke(HttpContext context, IWorkContext workContext)
        {
            // 设定语言
            workContext.WorkingLanguage = new WorkLanguage
            {
                Name = "zh-CN"
            };
            var culture = new CultureInfo("zh-CN");
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;

            // 调用请求管道中的下一个中间件
            return _next(context);
        }

        #endregion
    }
}
