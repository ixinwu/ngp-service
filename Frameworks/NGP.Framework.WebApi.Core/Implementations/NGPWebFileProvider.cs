/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * NGPFileProvider Description:
 * NGP文件提供者(IO功能使用磁盘上的文件系统)
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-28   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using NGP.Framework.Core;

namespace NGP.Framework.WebApi.Core
{
    /// <summary>
    /// NGP文件提供者(IO功能使用磁盘上的文件系统)
    /// </summary>
    public class NGPWebFileProvider : NGPFileProvider
    {
        /// <summary>
        /// 初始化NGPFileProvider的新实例
        /// </summary>
        /// <param name="hostingEnvironment">托管环境</param>
        public NGPWebFileProvider(IHostingEnvironment hostingEnvironment) 
            : base(File.Exists(hostingEnvironment.WebRootPath) ? Path.GetDirectoryName(hostingEnvironment.WebRootPath) : hostingEnvironment.ContentRootPath)
        {
        }
    }
}