/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * INGPCrawlerDownloader Description:
 * ngp爬虫下载器
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-6-3   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using System.Threading.Tasks;

namespace NGP.Framework.Core
{
    /// <summary>
    /// ngp爬虫下载器
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    public interface INGPCrawlerDownloader<TRequest> where TRequest : INGPCrawlerRequest
    {
        /// <summary>
        /// 下载执行
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<string> Download(TRequest request);
    }
}
