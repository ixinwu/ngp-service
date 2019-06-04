/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * INGPCrawler Description:
 * ngp爬虫
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-15   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using System.Collections.Generic;
using System.Threading.Tasks;

namespace NGP.Framework.Core
{
    /// <summary>
    /// ngp爬虫
    /// </summary>
    public interface INGPCrawler<TEntity> where TEntity : INGPCrawlerEntity, new()
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<List<TEntity>> Crawle();
    }
}
