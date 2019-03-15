/* ------------------------------------------------------------------------------
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * UploadFileRequest Description:
 * 上传文件请求对象
 *
 * Comment 					        Revision	Date                  Author
 * -----------------------------    --------    ------------------    ----------------
 * Created							1.0		    2019/3/7 16:53:09    hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/


using Microsoft.AspNetCore.Http;
using NGP.Framework.Core;
using System.Collections.Generic;

namespace NGP.Foundation.Service.File
{
    /// <summary>
    /// 追加人员请求
    /// </summary>
    public class UploadFileRequest : INGPRequest
    {
        /// <summary>
        /// 文件列表
        /// </summary>
        public List<IFormFile> Files { get; set; }
    }
}
