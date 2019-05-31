/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * IMongoRepository Description:
 * mongo仓储接口
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-5-30   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NGP.Framework.Core
{
    /// <summary>
    /// mongo仓储接口
    /// </summary>
    public interface IMongoRepository : IDisposable
    {
        /// <summary>
        /// 追加文档
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        void Add<TEntity>(TEntity obj) where TEntity : class, new();

        /// <summary>
        /// 获取文档
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity> GetById<TEntity>(string id) where TEntity : class, new();

        /// <summary>
        /// 查询所有文档内容
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetAll<TEntity>() where TEntity : class, new();

        /// <summary>
        /// 更新文档内容
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        void Update<TEntity>(TEntity obj) where TEntity : class, new();

        /// <summary>
        /// 移除文档内容
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void Remove<TEntity>(Guid id) where TEntity : class, new();

        /// <summary>
        /// 读取GridFS信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        byte[] ReadGridFS(string id);
    }
}
