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


namespace NGP.Framework.Core
{
    /// <summary>
    /// 文件扩展
    /// </summary>
    public static class NGPFileExtend
    {
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
