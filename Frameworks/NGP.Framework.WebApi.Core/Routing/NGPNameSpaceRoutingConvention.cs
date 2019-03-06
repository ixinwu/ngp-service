/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * NamespaceHttpControllerSelector Description:
 * 区分命名空间的路由筛选器
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-2-28   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;
using System.Linq;

namespace NGP.Framework.WebApi.Core.RouteExtensions
{
    /// <summary>
    /// 区分命名空间的路由筛选器
    /// </summary>
    public class NGPNameSpaceRoutingConvention : IApplicationModelConvention
    {

        private readonly string apiPrefix;
        private const string urlTemplate = "{0}/{1}/{2}";

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="apiPrefix"></param>
        public NGPNameSpaceRoutingConvention(string apiPrefix = "api")
        {
            this.apiPrefix = apiPrefix;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        public void Apply(ApplicationModel application)
        {
            foreach (var controller in application.Controllers)
            {
                var hasRouteAttribute = controller.Selectors.Any(x => x.AttributeRouteModel != null);
                if (hasRouteAttribute)
                {
                    continue;
                }
                var segments = controller.ControllerType.Namespace.Split(Type.Delimiter);
                var nameSpace = segments[1];
                controller.Selectors[0].AttributeRouteModel = new AttributeRouteModel()
                {
                    Template = string.Format(urlTemplate, apiPrefix, nameSpace, controller.ControllerName)
                };
            }
        }
    }
}
