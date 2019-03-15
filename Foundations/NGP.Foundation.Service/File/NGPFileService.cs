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
    public class NGPFileService : INGPFileService
    {
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="fileRequest"></param>
        /// <returns></returns>
        public NGPResponse<List<SingleFileResponse>> UploadFiles(UploadFileRequest fileRequest)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public NGPResponse DeleteFiles(NGPSingleRequest<List<string>> request)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 通过id查询文件
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public NGPResponse<SingleFileResponse> QueryFileById(NGPSingleRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
