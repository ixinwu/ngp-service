/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * NGPEngine Description:
 * 引擎实现
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-2-28   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NGP.Framework.Core;
using NGP.Framework.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NGP.Framework.WebApi.Core
{
    /// <summary>
    /// 引擎实现
    /// </summary>
    public class NGPWebEngine : NGPEngine
    {
        /// <summary>
        /// 获取 IServiceProvider
        /// </summary>
        /// <returns>IServiceProvider</returns>
        protected override IServiceProvider GetServiceProvider()
        {
            var accessor = ServiceProvider.GetService<IHttpContextAccessor>();
            var context = accessor.HttpContext;
            return context?.RequestServices ?? ServiceProvider;
        }
    }
}
