/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * INGPCrawlerPipeline Description:
 * ngp爬虫管道
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-6-3   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using System.Collections.Generic;
using System.Threading.Tasks;

namespace NGP.Framework.Core
{
    /// <summary>
    /// ngp爬虫管道
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface INGPCrawlerPipeline<TEntity> where TEntity : INGPCrawlerEntity, new()
    {
        /// <summary>
        /// 执行管道
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task Run(IEnumerable<TEntity> entity);
    }
}
