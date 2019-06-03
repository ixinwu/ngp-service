/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * NGPCrawlerScheduler Description:
 * ngp爬虫任务管理
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-6-3   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using System;
using System.Threading.Tasks;

namespace NGP.Middleware.Crawlar
{
    /// <summary>
    /// ngp爬虫任务管理
    /// </summary>
    public class NGPCrawlerScheduler : INGPCrawlerScheduler
    {
        /// <summary>
        /// 重试次数
        /// </summary>
        public long RetryTime { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        /// <summary>
        /// 任务执行
        /// </summary>
        /// <returns></returns>
        public Task Schedule()
        {
            // TODO : Implement Quartz or Hangfire
            throw new NotImplementedException();
        }
    }
}
