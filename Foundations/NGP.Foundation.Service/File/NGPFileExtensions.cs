/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * NGPFileService Description:
 * 文件扩展
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-3-7    hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using NGP.Framework.Core;
using System.Linq;

namespace NGP.Foundation.Service.File
{
    /// <summary>
    /// 文件扩展
    /// </summary>
    public static class NGPFileExtensions
    {
        /// <summary>
        /// 获取文件路径
        /// </summary>
        /// <param name="areaId"></param>
        /// <param name="comanpyId"></param>
        /// <returns></returns>
        public static string VirtualFilePath(string areaId, string comanpyId)
        {
            var index = 0;
            var path = areaId.Aggregate(string.Empty, (areaPath, next) =>
             {
                 areaPath += next;
                 index++;
                 if (index % 2 == 0 && index < 7)
                 {
                     areaPath += "/";
                 }
                 return areaPath;
             });
            if (!string.IsNullOrWhiteSpace(comanpyId))
            {
                path += string.Format("/{0}", comanpyId);
            }
            return GlobalConst.__AttachmentFilesPath + path;
        }

        /// <summary>
        /// 获取文件url
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string FileUrl(string filePath,string fileName)
        {
            var webHelper = Singleton<IEngine>.Instance.Resolve<IWebHelper>();
            return string.Format("{0}{1}", filePath, fileName).Replace("wwwroot", webHelper.GetStoreHost(false));
        }
    }
}
