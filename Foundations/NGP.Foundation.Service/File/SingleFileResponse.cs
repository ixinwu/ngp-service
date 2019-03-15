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



namespace NGP.Foundation.Service.File
{
    /// <summary>
    /// 追加人员请求
    /// </summary>
    public class SingleFileResponse
    {
        /// <summary>
        /// 文件数据库Id
        /// </summary>
        public string FileId { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 文件扩展名
        /// </summary>
        public string Extension { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// 文件url
        /// </summary>
        public string Url { get; set; }
    }
}
