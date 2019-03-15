/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * IFileService Description:
 * 文件服务接口
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-3-7    hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using NGP.Framework.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace NGP.Foundation.Service.File
{
    /// <summary>
    /// 文件服务接口
    /// </summary>
    public interface INGPFileService
    {
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="fileRequest"></param>
        /// <returns></returns>
        NGPResponse<List<SingleFileResponse>> UploadFiles(UploadFileRequest fileRequest);

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        NGPResponse DeleteFiles(NGPSingleRequest<List<string>> request);

        /// <summary>
        /// 通过id查询文件
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        NGPResponse<SingleFileResponse> QueryFileById(NGPSingleRequest request);
    }
}
