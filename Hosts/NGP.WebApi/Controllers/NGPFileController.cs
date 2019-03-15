/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * NGPFileController Description:
 * NGP文件控制器
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-3-7   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using Microsoft.AspNetCore.Mvc;
using NGP.Foundation.Service.File;
using NGP.Framework.Core;
using NGP.Framework.WebApi.Core;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace NGP.WebApi
{
    /// <summary>
    /// NGP文件控制器
    /// </summary>
    public class NGPFileController : ApiController
    {
        /// <summary>
        /// 文件服务
        /// </summary>
        private readonly INGPFileService _nGPFileService;

        /// <summary>
        /// 文件提供者
        /// </summary>
        private readonly INGPFileProvider _fileProvider;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="nGPFileService"></param>
        /// <param name="nGPFileProvider"></param>
        public NGPFileController(INGPFileService nGPFileService, INGPFileProvider nGPFileProvider)
        {
            _nGPFileService = nGPFileService;
            _fileProvider = nGPFileProvider;
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="fileRequest"></param>
        /// <returns></returns>
        [HttpPost("UploadFiles")]
        public ActionResult<NGPResponse<List<SingleFileResponse>>> UploadFiles([FromForm] UploadFileRequest fileRequest)
        {
            return Ok(_nGPFileService.UploadFiles(fileRequest));
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("DeleteFiles")]
        public ActionResult<NGPResponse> DeleteFiles(NGPSingleRequest<List<string>> request)
        {
            return Ok(_nGPFileService.DeleteFiles(request));
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("DownloadFile")]
        public async Task<IActionResult> DownloadFile(NGPSingleRequest request)
        {
            var file = _nGPFileService.QueryFileById(request);

            var fullPath = Path.Combine(_fileProvider.MapPath(file.Data.FilePath), file.Data.FileName);

            var memory = new MemoryStream();
            using (var stream = new FileStream(fullPath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "application/octet-stream", file.Data.FileName);
        }
    }
}
