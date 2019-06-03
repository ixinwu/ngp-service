/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * NGPCrawlerDownloader Description:
 * 默认下载器
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-6-3   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NGP.Framework.Core;

namespace NGP.Middleware.Crawlar
{
    /// <summary>
    /// 默认下载器
    /// </summary>
    public class NGPCrawlerDownloader : INGPCrawlerDownloader
    {
        /// <summary>
        /// 下载处理
        /// </summary>
        /// <param name="crawlUrl"></param>
        /// <returns></returns>
        public virtual async Task<HtmlDocument> Download(string crawlUrl)
        {
            var htmlDocument = new HtmlDocument();

            var config = Singleton<List<CrawlerConfig>>.Instance.FirstOrDefault(s => crawlUrl.Contains(s.Domain));

            using (WebClient client = new WebClient())
            {
                client.Headers.Add("cookie", config.Cookie);
                client.Headers.Add("accept", config.Accept);
                client.Headers.Add("user-agent", config.UserAgent);
                string htmlCode = await client.DownloadStringTaskAsync(crawlUrl);
                htmlDocument.LoadHtml(htmlCode);
            }
            return htmlDocument;
        }

    }
}
