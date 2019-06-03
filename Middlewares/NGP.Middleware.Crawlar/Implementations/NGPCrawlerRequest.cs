/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * NGPCrawlerRequest Description:
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
    public class NGPCrawlerRequest : INGPCrawlerRequest
    {
        /// <summary>
        /// url
        /// </summary>
        public string Url { get; set; }
    }
}
