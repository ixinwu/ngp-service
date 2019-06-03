/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * INGPCrawlerRequest Description:
 * ngp爬虫请求对象
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-6-3   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/


namespace NGP.Middleware.Crawlar
{
    /// <summary>
    /// ngp爬虫请求对象
    /// </summary>
    public interface INGPCrawlerRequest
    {
        /// <summary>
        /// url
        /// </summary>
        string Url { get; set; }
    }
}
