/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * MongoContext Description:
 * mongo上下文
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-5-30   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using NGP.Framework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NGP.Middleware.Mongo
{
    /// <summary>
    /// mongo上下文
    /// </summary>
    public class MongoContext : IMongoContext
    {
        /// <summary>
        /// data base
        /// </summary>
        private IMongoDatabase Database { get; }
        /// <summary>
        /// commands
        /// </summary>
        private readonly List<Func<Task>> _commands;
        /// <summary>
        /// grid fs bucket
        /// </summary>
        public GridFSBucket GridFSBucket { get; private set; }

        /// <summary>
        /// ctor
        /// </summary>
        public MongoContext()
        {
            _commands = new List<Func<Task>>();

            //RegisterConventions();
            var mongoClient = new MongoClient(Singleton<MongoConfig>.Instance.Connection);
            Database = mongoClient.GetDatabase(Singleton<MongoConfig>.Instance.DatabaseName);
            GridFSBucket = new GridFSBucket(Database);
        }

        /// <summary>
        /// 注册约定
        /// </summary>
        private void RegisterConventions()
        {
            var pack = new ConventionPack
            {
                new IgnoreExtraElementsConvention(true),
                new IgnoreIfDefaultConvention(true)
            };
            ConventionRegistry.Register("My Solution Conventions", pack, t => true);
        }

        /// <summary>
        /// 提交保存
        /// </summary>
        /// <returns></returns>
        public async Task<int> SaveChanges()
        {
            var commandTasks = _commands.Select(c => c());

            await Task.WhenAll(commandTasks);

            return _commands.Count;
        }

        /// <summary>
        /// 获取集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return Database.GetCollection<T>(name);
        }

        /// <summary>
        /// 添加命令
        /// </summary>
        /// <param name="func"></param>
        public void AddCommand(Func<Task> func)
        {
            _commands.Add(func);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
