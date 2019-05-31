/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * IMongoContext Description:
 * mongo上下文接口
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-5-30   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NGP.Middleware.Mongo
{
    /// <summary>
    /// mongo上下文接口
    /// </summary>
    public interface IMongoContext : IDisposable
    {
        /// <summary>
        /// 文件操作块
        /// </summary>
        GridFSBucket GridFSBucket { get; }

        /// <summary>
        /// 添加command
        /// </summary>
        /// <param name="func"></param>
        void AddCommand(Func<Task> func);

        /// <summary>
        /// 保存内容
        /// </summary>
        /// <returns></returns>
        Task<int> SaveChanges();

        /// <summary>
        /// 获取集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        IMongoCollection<T> GetCollection<T>(string name);
    }
}
