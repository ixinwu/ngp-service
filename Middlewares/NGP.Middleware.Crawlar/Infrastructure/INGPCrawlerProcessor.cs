/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * INGPCrawlerProcessor Description:
 * ngp爬虫执行器
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-6-3   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using HtmlAgilityPack;
using NGP.Framework.Core;
using System.Collections.Generic;

namespace NGP.Middleware.Crawlar
{
    /// <summary>
    /// ngp爬虫执行器
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface INGPCrawlerProcessor<TEntity> where TEntity : BaseEntity, new()
    {
        /// <summary>
        /// 执行解析
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        IEnumerable<TEntity> Process(HtmlDocument document);
    }
}
