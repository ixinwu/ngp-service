/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * INGPCrawlerScheduler Description:
 * ngp爬虫任务管理
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-6-3   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using System.Threading.Tasks;

namespace NGP.Framework.Core
{
    /// <summary>
    /// ngp爬虫任务管理
    /// </summary>
    public interface INGPCrawlerScheduler
    {
        /// <summary>
        /// 重试次数
        /// </summary>
        long RetryTime { get; set; }

        /// <summary>
        /// 计划执行
        /// </summary>
        /// <returns></returns>
        Task Schedule();
    }
}
