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

using HtmlAgilityPack;
using System.Threading.Tasks;

namespace NGP.Middleware.Crawlar
{
    /// <summary>
    /// ngp爬虫下载器
    /// </summary>
    public interface INGPCrawlerDownloader
    {
        /// <summary>
        /// 下载执行
        /// </summary>
        /// <param name="crawlUrl"></param>
        /// <returns></returns>
        Task<HtmlDocument> Download(string crawlUrl);
    }
}
