/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * NGPCrawlerPipeline Description:
 * ngp爬虫管道
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-6-3   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/


using System.Collections.Generic;
using System.Threading.Tasks;
using NGP.Framework.Core;

namespace NGP.Middleware.Crawlar
{
    /// <summary>
    /// ngp爬虫管道
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class NGPCrawlerPipeline<TEntity> : INGPCrawlerPipeline<TEntity> where TEntity : BaseEntity, new()
    {
        /// <summary>
        /// 执行管道
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Task Run(IEnumerable<TEntity> entity)
        {
            return Task.FromResult(0);   
        }
    }
}
