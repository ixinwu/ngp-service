/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * SwaggerFileUploadOperationFilter Description:
 * Swagger文件上传拦截器
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-28   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/
using Microsoft.AspNetCore.Mvc.Authorization;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NGP.Framework.WebApi.Core
{
    /// <summary>
    /// Swagger文件上传拦截器
    /// </summary>
    public class SwaggerFileUploadOperationFilter : IOperationFilter
    {
        /// <summary>
        /// 应用拦截
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.OperationId.ToUpper() == "UploadFiles".ToUpper())
            {
                var fileParam = operation.Parameters.FirstOrDefault(s => s.Name == "Files");
                
                operation.Parameters.Remove(fileParam);                
                operation.Parameters.Add(new NonBodyParameter
                {
                    Name = "Files",
                    In = "formData",
                    Description = "Upload File",
                    Required = true,
                    Type = "file"
                });
                operation.Consumes.Add("multipart/form-data");
            }
        }
    }
}
