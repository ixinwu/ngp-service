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

using MongoDB.Bson;
using MongoDB.Driver;
using NGP.Framework.Core;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NGP.Middleware.Mongo
{
    /// <summary>
    /// mongo仓储接口
    /// </summary>
    public class MongoRepository : IMongoRepository
    {
        private readonly IMongoContext _context;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="context"></param>
        public MongoRepository(IMongoContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 追加文档
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual void Add<TEntity>(TEntity obj)where TEntity:class,new ()
        {
           var  dbSet = _context.GetCollection<TEntity>(typeof(TEntity).Name);
            _context.AddCommand(() => dbSet.InsertOneAsync(obj));
        }

        /// <summary>
        /// 获取文档
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> GetById<TEntity>(string id) where TEntity : class, new()
        {
            var dbSet = _context.GetCollection<TEntity>(typeof(TEntity).Name);
            var data = await dbSet.FindAsync(Builders<TEntity>.Filter.Eq("_id", id));
            return data.SingleOrDefault();
        }

        /// <summary>
        /// 查询所有文档内容
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public virtual async Task<IEnumerable<TEntity>> GetAll<TEntity>() where TEntity : class, new()
        {
            var dbSet = _context.GetCollection<TEntity>(typeof(TEntity).Name);
            var all = await dbSet.FindAsync(Builders<TEntity>.Filter.Empty);
            return all.ToList();
        }

        /// <summary>
        /// 更新文档内容
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual void Update<TEntity>(TEntity obj) where TEntity : class, new()
        {
            var dbSet = _context.GetCollection<TEntity>(typeof(TEntity).Name);
            _context.AddCommand(() => dbSet.ReplaceOneAsync(Builders<TEntity>.Filter.Eq("_id", obj.GetId()), obj));
        }

        /// <summary>
        /// 移除文档内容
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual void Remove<TEntity>(Guid id) where TEntity : class, new()
        {
            var dbSet = _context.GetCollection<TEntity>(typeof(TEntity).Name);
            _context.AddCommand(() => dbSet.DeleteOneAsync(Builders<TEntity>.Filter.Eq("_id", id)));
        }

        /// <summary>
        /// 读取GridFS信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public byte[] ReadGridFS(string id)
        {
            var objectId = new ObjectId(id);
            var bytes = _context.GridFSBucket.DownloadAsBytes(objectId);
            return bytes;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
