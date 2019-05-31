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

using NGP.Foundation.Resources;
using NGP.Framework.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NGP.Foundation.Service.File
{
    /// <summary>
    /// 文件服务接口
    /// </summary>
    [ExceptionCallHandler]
    public class NGPFileService : INGPFileService
    {
        #region private fields
        /// <summary>
        /// 工作上下文
        /// </summary>
        private readonly IWorkContext _workContext;

        /// <summary>
        /// 操作db仓储
        /// </summary>
        private readonly IUnitRepository _unitRepository;

        /// <summary>
        /// 文件提供者
        /// </summary>
        private readonly INGPFileProvider _fileProvider;
        #endregion

        #region ctor
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="workContext"></param>
        /// <param name="unitRepository"></param>
        /// <param name="fileProvider"></param>
        public NGPFileService(IWorkContext workContext, IUnitRepository unitRepository, INGPFileProvider fileProvider)
        {
            _workContext = workContext;
            _unitRepository = unitRepository;
            _fileProvider = fileProvider;
        }
        #endregion

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="fileRequest"></param>
        /// <returns></returns>
        [TransactionCallHandler]
        public NGPResponse<List<SingleFileResponse>> UploadFiles(UploadFileRequest fileRequest)
        {
            if (fileRequest == null ||
                fileRequest.Files.IsNullOrEmpty())
            {
                return new NGPResponse<List<SingleFileResponse>>
                {
                    ErrorCode = ErrorCode.ParamEmpty,
                    Message = CommonResource.ParameterError,
                    Status = OperateStatus.Error
                };
            }

            // 根据区域和平台划分文件夹
            var virtualPath = GlobalConst.__AttachmentFilesPath;

            // 创建划分的文件夹
            var folderPath = _fileProvider.MapPath(virtualPath);
            _fileProvider.CreateDirectory(folderPath);

            var insertList = new List<Sys_File_Info>();
            // 循环文件
            foreach (var file in fileRequest.Files)
            {
                var spitFileNames = file.FileName.Split('.');
                var extension = string.Empty;
                if (spitFileNames.Length > 0)
                {
                    extension = "." + spitFileNames[spitFileNames.Length - 1];
                }

                // 添加时间戳的文件名称
                var fileName = string.Format("{0}_{1}", DateTime.Now.ToString(GlobalConst.DateFormatConst.__DateTimeFormatNotBar),
                    file.FileName);

                // 插入数据库对象
                var insertFile = new Sys_File_Info
                {
                    ExtensionName = extension,
                    FileName = fileName,
                    FilePath = virtualPath,
                    Size = file.Length,
                };
                insertFile.InitAddDefaultFields(_workContext);
                insertList.Add(insertFile);

                // 完整路径
                var fullPath = Path.Combine(folderPath, fileName);

                // 保存文件
                using (var bits = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(bits);
                }
            }

            // 插入数据库
            _unitRepository.Insert(insertList);

            // 转换返回对象
            var result = insertList.Select(s => new SingleFileResponse
            {
                FileId = s.Id,
                FileName = s.FileName,
                FilePath = s.FilePath,
                Extension = s.ExtensionName,
                Size = s.Size,
                Url = NGPFileExtensions.FileUrl(s.FilePath, s.FileName)
            }).ToList();
            return new NGPResponse<List<SingleFileResponse>>
            {
                AffectedRows = insertList.Count,
                Message = CommonResource.OperatorSuccess,
                Status = OperateStatus.Success,
                Data = result
            };
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public NGPResponse DeleteFiles(NGPSingleRequest<List<string>> request)
        {
            // 参数验证
            if (request == null || request.RequestData.IsNullOrEmpty())
            {
                return new NGPResponse
                {
                    ErrorCode = ErrorCode.ParamEmpty,
                    Message = CommonResource.ParameterError,
                    Status = OperateStatus.Error
                };
            }

            // 查询id对应的文件列表
            var fileList = _unitRepository.All<Sys_File_Info>(s => request.RequestData.Contains(s.Id)).ToList();

            // 删除文件
            foreach (var file in fileList)
            {
                var fullPath = Path.Combine(_fileProvider.MapPath(file.FilePath), file.FileName);

                _fileProvider.DeleteFile(fullPath);

                _unitRepository.Delete(file);
            }
            return new NGPResponse
            {
                AffectedRows = fileList.Count,
                Message = CommonResource.OperatorSuccess,
                Status = OperateStatus.Success,
            };
        }

        /// <summary>
        /// 通过id查询文件
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public NGPResponse<SingleFileResponse> QueryFileById(NGPSingleRequest request)
        {
            // 参数验证
            if (request == null || string.IsNullOrWhiteSpace(request.RequestData))
            {
                return new NGPResponse<SingleFileResponse>
                {
                    ErrorCode = ErrorCode.ParamEmpty,
                    Message = CommonResource.ParameterError,
                    Status = OperateStatus.Error
                };
            }

            var file = _unitRepository.FindById<Sys_File_Info>(request.RequestData);

            return new NGPResponse<SingleFileResponse>
            {
                Message = CommonResource.OperatorSuccess,
                Status = OperateStatus.Success,
                Data = new SingleFileResponse
                {
                    FileId = file.Id,
                    FileName = file.FileName,
                    FilePath = file.FilePath,
                    Extension = file.ExtensionName,
                    Size = file.Size,
                    Url = NGPFileExtensions.FileUrl(file.FilePath, file.FileName)
                }
            };
        }
    }
}
