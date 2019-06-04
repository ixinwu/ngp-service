/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * INGPCrawler Description:
 * ngp爬虫
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-6-3   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using NGP.Framework.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NGP.Middleware.Crawlar
{
    /// <summary>
    /// ngp爬虫
    /// </summary>
    public class NGPCrawler<TEntity,TRequest> : INGPCrawler<TEntity> 
        where TEntity : INGPCrawlerEntity, new()
        where TRequest : INGPCrawlerRequest
    {
        /// <summary>
        /// 请求对象
        /// </summary>
        public List<TRequest> Requests { get; private set; } = new List<TRequest>();

        /// <summary>
        /// downloader对象
        /// </summary>
        public INGPCrawlerDownloader<TRequest> Downloader { get; private set; }

        /// <summary>
        /// 处理对象
        /// </summary>
        public INGPCrawlerProcessor<TEntity> Processor { get; private set; }

        /// <summary>
        /// 任务管理
        /// </summary>
        public INGPCrawlerScheduler Scheduler { get; private set; }

        /// <summary>
        /// 处理管道
        /// </summary>
        public List<INGPCrawlerPipeline<TEntity>> Pipelines { get; private set; } = new List<INGPCrawlerPipeline<TEntity>>();

        /// <summary>
        /// ctor
        /// </summary>
        public NGPCrawler()
        {

        }

        /// <summary>
        /// 添加请求
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public NGPCrawler<TEntity, TRequest> AddRequest(TRequest request)
        {
            Requests.Add(request);
            return this;
        }

        /// <summary>
        /// 添加下载器
        /// </summary>
        /// <param name="downloader"></param>
        /// <returns></returns>
        public NGPCrawler<TEntity, TRequest> AddDownloader(INGPCrawlerDownloader<TRequest> downloader)
        {
            Downloader = downloader;
            return this;
        }

        /// <summary>
        /// 添加处理器
        /// </summary>
        /// <param name="processor"></param>
        /// <returns></returns>
        public NGPCrawler<TEntity, TRequest> AddProcessor(INGPCrawlerProcessor<TEntity> processor)
        {
            Processor = processor;
            return this;
        }

        public NGPCrawler<TEntity, TRequest> AddScheduler(INGPCrawlerScheduler scheduler)
        {
            Scheduler = scheduler;
            return this;
        }

        /// <summary>
        /// 添加管道
        /// </summary>
        /// <param name="pipeline"></param>
        /// <returns></returns>
        public NGPCrawler<TEntity, TRequest> AddPipeline(INGPCrawlerPipeline<TEntity> pipeline)
        {
            Pipelines.Add(pipeline);
            return this;
        }

        /// <summary>
        /// 执行爬虫
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public async Task<List<TEntity>> Crawle()
        {
            var result = new List<TEntity>();
            foreach (var request in Requests)
            {
                var document = await Downloader.Download(request);
                var entity = Processor.Process(document);
                result.AddRange(entity);

            }
            foreach (var pipeline in Pipelines)
            {
                await pipeline.Run(result);
            }
            return result;
        }
    }
}
