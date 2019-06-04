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

using System.Collections.Generic;

namespace NGP.Framework.Core
{
    /// <summary>
    /// ngp爬虫执行器
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface INGPCrawlerProcessor<TEntity> where TEntity : INGPCrawlerEntity, new()
    {
        /// <summary>
        /// 执行解析
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        IEnumerable<TEntity> Process(string response);
    }
}
