/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * UnitRepository Description:
 * 工作单元仓储
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-20   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using NGP.Framework.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using Z.EntityFramework.Plus;

namespace NGP.Framework.DataAccess
{
    /// <summary>
    /// 工作单元仓储
    /// </summary>
    public abstract class UnitRepository : IUnitRepository
    {
        /// <summary>
        /// ef上下文
        /// </summary>
        protected readonly IDbContext _context;

        #region Ctor
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="context"></param>
        protected UnitRepository(IDbContext context)
          => _context = context;
        #endregion

        #region Methods
        /// <summary>
        /// 查询满足条件的第一个
        /// </summary>
        /// <typeparam name="TEntity">返回值类型</typeparam>
        /// <param name="criteria">查询条件</param>
        /// <returns>查询结果</returns>
        public TEntity FirstOrDefault<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : BaseDBEntity
            => _context.Set<TEntity>().FirstOrDefault(criteria);


        /// <summary>
        /// 根据ID查找
        /// </summary>
        /// <typeparam name="TEntity">返回值类型</typeparam>
        /// <param name="id">主键值</param>
        /// <returns>查询结果</returns>
        public TEntity FindById<TEntity>(string id) where TEntity : BaseDBEntity
            => _context.Set<TEntity>().Find(id);

        /// <summary>
        /// 追加对象
        /// </summary>
        /// <typeparam name="TEntity">参数类型</typeparam>
        /// <param name="entity">追加对象</param>
        public void Insert<TEntity>(TEntity entity) where TEntity : BaseDBEntity
        {
            if (entity == null)
            {
                return;
            }
            _context.Set<TEntity>().Add(entity);
        }

        /// <summary>
        /// 更新对象
        /// </summary>
        /// <typeparam name="TEntity">参数类型</typeparam>
        /// <param name="entity">需要更新的实体</param>
        public void Update<TEntity>(TEntity entity) where TEntity : BaseDBEntity
        {
            if (entity == null)
            {
                return;
            }
            _context.Set<TEntity>().Attach(entity);
        }

        /// <summary>
        /// EF扩展更新
        /// </summary>
        /// <typeparam name="TEntity">参数类型</typeparam>
        /// <param name="updateExpression">更新表达式</param>
        /// <param name="criteria">查询表达式</param>
        /// <returns></returns>      
        public int UpdateByExpression<TEntity>(Expression<Func<TEntity, TEntity>> updateExpression,
            Expression<Func<TEntity, bool>> criteria = null) where TEntity : class
        {
            var source = All<TEntity>(criteria);
            return source.Update(updateExpression);
        }

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <typeparam name="TEntity">参数类型</typeparam>
        /// <param name="entity">需要删除的实体</param>
        public void Delete<TEntity>(TEntity entity) where TEntity : BaseDBEntity
        {
            if (entity == null)
            {
                return;
            }
            _context.Set<TEntity>().Remove(entity);
        }

        /// <summary>
        /// EF扩展删除
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="criteria">查询表达式</param>
        /// <returns></returns>
        public int DeleteByExpression<T>(Expression<Func<T, bool>> criteria = null) where T : class
        {
            var source = _context.Set<T>();
            if (criteria == null)
            {
                return _context.Set<T>().Delete();
            }
            return source.Where(criteria).Delete();
        }

        /// <summary>
        /// 基于原始SQL查询为查询类型创建LINQ查询
        /// </summary>
        /// <typeparam name="TQuery">Query type</typeparam>
        /// <param name="sql">The raw SQL query</param>
        /// <param name="parameters">The values to be assigned to parameters</param>
        /// <returns>表示原始SQL查询的IQueryable</returns>
        public IQueryable<TQuery> QueryFromSql<TQuery>(string sql, IDictionary<string, object> parameters = null) where TQuery : class
        {
            var dbParameters = (parameters ?? new Dictionary<string, object>()).Select(s => new SqlParameter(s.Key, s.Value)).ToArray();
            return _context.QueryFromSql<TQuery>(sql, dbParameters);
        }


        /// <summary>
        /// 基于原始SQL查询为实体创建LINQ查询
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <param name="sql">原始SQL查询</param>
        /// <param name="parameters">要分配给参数的值</param>
        /// <returns>表示原始SQL查询的IQueryable</returns>
        public IQueryable<TEntity> EntityFromSql<TEntity>(string sql, IDictionary<string, object> parameters = null) where TEntity : BaseDBEntity
        {
            var dbParameters = (parameters ?? new Dictionary<string, object>()).Select(s => new SqlParameter(s.Key, s.Value)).ToArray();
            return _context.EntityFromSql<TEntity>(sql, dbParameters);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TEntity">查询类型</typeparam>
        public IQueryable<TEntity> All<TEntity>(Expression<Func<TEntity, bool>> criteria = null) where TEntity : class
            => criteria == null ? _context.Set<TEntity>() : _context.Set<TEntity>().Where(criteria);


        /// <summary>
        /// 获取一个启用了“无跟踪”的表（EF功能），仅当只为只读操作加载记录时才使用该表
        /// </summary>
        /// <typeparam name="TEntity">查询类型</typeparam>
        ///  <param name="criteria">条件表达式</param>
        /// <returns>当前类型的表接口</returns>
        public IQueryable<TEntity> AllNoTracking<TEntity>(Expression<Func<TEntity, bool>> criteria = null) where TEntity : class
        {
            var entities = _context.Set<TEntity>().AsNoTracking();
            return criteria == null ? entities : entities.Where(criteria);
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <typeparam name="TEntity">查询类型</typeparam>
        ///  <param name="criteria">条件表达式</param>
        /// <returns>当前条件是否存在</returns>
        public bool Any<TEntity>(Expression<Func<TEntity, bool>> criteria = null) where TEntity : BaseDBEntity
            => All(criteria).Select(s => 1).Distinct().Any();


        /// <summary>
        /// 批量插入
        /// </summary>
        /// <typeparam name="TEntity">参数类型</typeparam>
        /// <param name="entities">插入列表</param>
        public void Insert<TEntity>(List<TEntity> entities) where TEntity : class
        {
            if (entities.IsNullOrEmpty())
            {
                return;
            }
            foreach (var entitieItem in entities)
            {
                _context.Set<TEntity>().Add(entitieItem);
            }
        }

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <typeparam name="TEntity">参数类型</typeparam>
        /// <param name="entities">插入列表</param>
        public void BulkInsert<TEntity>(IList<TEntity> entities) where TEntity : class
        {
            if (entities.IsNullOrEmpty())
            {
                return;
            }

            (_context as DbContext).BulkInsert(entities);
        }

        /// <summary>
        /// 将此上下文中所做的所有更改保存到数据库
        /// </summary>
        /// <returns>写入数据库的状态条目数</returns>
        public int SaveChanges()
            => _context.SaveChanges();
        #endregion

        #region excute command
        /// <summary>
        /// 读取列表数据(泛型)
        /// </summary>
        /// <typeparam name="TEntity">返回结果类型</typeparam>
        /// <param name="commandText">执行语句</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>返回结果</returns>
        public List<TEntity> QueryListEntity<TEntity>(string commandText,
             IDictionary<string, object> parameters = null)
             where TEntity : class, new()
        {
            Func<IDbCommand, List<TEntity>> excute = (dbCommand) =>
            {
                using (IDataReader reader = dbCommand.ExecuteReader())
                {
                    var result = new List<TEntity>();
                    var converter = new DbReaderConverter<TEntity>(reader);
                    while (reader.Read())
                    {
                        var item = converter.CreateItemFromRow();
                        result.Add(item);
                    }
                    return result;
                }
            };
            return CreateDbCommondAndExcute(commandText, parameters,
                excute);
        }

        /// <summary>
        /// 读取列表数据
        /// </summary>
        /// <param name="commandText">执行语句</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>返回结果</returns>
        public IEnumerable<dynamic> QueryListDynamic(string commandText,
             IDictionary<string, object> parameters = null)
        {
            return CreateDbCommondAndExcute(commandText, parameters, ExcuteQueryListDynamic);
        }

        /// <summary>
        /// 执行动态列表读取
        /// </summary>
        /// <param name="dbCommand"></param>
        /// <returns></returns>
        private IEnumerable<dynamic> ExcuteQueryListDynamic(IDbCommand dbCommand)
        {
            using (IDataReader reader = dbCommand.ExecuteReader())
            {
                var names = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToList();
                foreach (IDataRecord record in reader as IEnumerable)
                {
                    var expando = new ExpandoObject() as IDictionary<string, object>;
                    foreach (var name in names)
                    {
                        expando[name] = record[name];
                    }
                    yield return expando;
                }
            }
        }

        /// <summary>
        /// 读取列表数据,根据回调设定值,值通过key,value提供
        /// </summary>
        /// <param name="commandText">执行语句</param>
        /// <param name="parameters">参数列表</param>        
        /// <returns>返回结果</returns>
        public IEnumerable<IDictionary<string, object>> QueryListDictionary(string commandText,
            IDictionary<string, object> parameters = null)
        {
            return CreateDbCommondAndExcute(commandText, parameters, ExcuteQueryListDictionary);
        }

        /// <summary>
        /// 读取列表数据,根据回调设定值,值通过key,value提供
        /// </summary>
        /// <param name="dbCommand"></param>
        /// <returns></returns>
        private IEnumerable<IDictionary<string, object>> ExcuteQueryListDictionary(IDbCommand dbCommand)
        {
            using (var reader = dbCommand.ExecuteReader())
            {
                var names = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToList();
                foreach (IDataRecord record in reader as IEnumerable)
                    yield return names.ToDictionary(n => n, n => record[n]);
            }
        }

        /// <summary>
        /// 执行数据行读取
        /// </summary>
        /// <param name="commandText">执行语句</param>
        /// <param name="parameters">参数列表</param>        
        /// <returns>返回结果</returns>
        public IEnumerable<DataRow> QueryListDataRow(string commandText,
            IDictionary<string, object> parameters = null)
        {
            return CreateDbCommondAndExcute(commandText, parameters, ExcuteQueryListDataRow);
        }

        /// <summary>
        /// 执行数据行读取
        /// </summary>
        /// <param name="dbCommand"></param>
        /// <returns></returns>
        private IEnumerable<DataRow> ExcuteQueryListDataRow(IDbCommand dbCommand)
        {
            using (var reader = dbCommand.ExecuteReader())
            {
                var table = new DataTable();
                table.BeginLoadData();
                table.Load(reader);
                table.EndLoadData();
                return table.AsEnumerable();
            }
        }

        /// <summary>
        /// 读取单条记录（泛型）
        /// </summary>
        /// <typeparam name="TEntity">返回结果类型</typeparam>
        /// <param name="commandText">执行语句</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>返回结果</returns>
        public TEntity QuerySingleEntity<TEntity>(string commandText,
            IDictionary<string, object> parameters = null)
            where TEntity : class, new()
        {
            Func<IDbCommand, TEntity> excute = (dbCommand) =>
            {
                using (IDataReader reader = dbCommand.ExecuteReader())
                {
                    var converter = new DbReaderConverter<TEntity>(reader);
                    if (reader.Read())
                    {
                        return converter.CreateItemFromRow();
                    }
                    return null;
                }
            };
            return CreateDbCommondAndExcute(commandText, parameters,
                excute);
        }

        /// <summary>
        /// 读取列表数据
        /// </summary>
        /// <param name="commandText">执行语句</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>返回结果</returns>
        public dynamic QuerySingleDynamic(string commandText,
             IDictionary<string, object> parameters = null)
        {
            Func<IDbCommand, dynamic> excute = (dbCommand) =>
             {
                 using (IDataReader reader = dbCommand.ExecuteReader())
                 {
                     var names = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToList();
                     foreach (IDataRecord record in reader as IEnumerable)
                     {
                         var expando = new ExpandoObject() as IDictionary<string, object>;
                         foreach (var name in names)
                         {
                             expando[name] = record[name];
                         }
                         return expando;
                     }
                     return null;
                 }
             };

            return CreateDbCommondAndExcute(commandText, parameters, excute);
        }

        /// <summary>
        /// 读取详细记录,根据回调设定值,值通过key,value提供
        /// </summary>
        /// <param name="commandText">执行语句</param>
        /// <param name="parameters">参数列表</param>        
        /// <returns>返回结果</returns>
        public IDictionary<string, object> QuerySingleDictionary(string commandText,
            IDictionary<string, object> parameters = null)
        {
            Func<IDbCommand, dynamic> excute = (dbCommand) =>
            {
                using (var reader = dbCommand.ExecuteReader())
                {
                    var names = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToList();
                    foreach (IDataRecord record in reader as IEnumerable)
                    {
                        return names.ToDictionary(n => n, n => record[n]);
                    }
                    return null;
                }
            };

            return CreateDbCommondAndExcute(commandText, parameters, excute);
        }

        /// <summary>
        /// 读取第一条第一列的结果（泛型）
        /// </summary>
        /// <typeparam name="T">结果类型</typeparam>
        /// <param name="commandText">执行语句</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>返回结果</returns>
        public T ExecuteScalar<T>(string commandText, IDictionary<string, object> parameters = null)
        {
            return CreateDbCommondAndExcute(commandText,
                parameters, (dbCommand) => dbCommand.ExecuteScalar().ToObject<T>());
        }

        /// <summary>
        /// 执行数据库操作
        /// </summary>
        /// <param name="commandText">执行语句</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>影响条数</returns>
        public int ExecuteNonQuery(string commandText, IDictionary<string, object> parameters = null)
        {
            return CreateDbCommondAndExcute(commandText,
                parameters, (dbCommand) => dbCommand.ExecuteNonQuery());
        }
        #endregion

        #region abstract methods
        /// <summary>
        /// 创建MySqlCommand执行
        /// </summary>
        /// <typeparam name="T">结果泛型</typeparam>
        /// <param name="commandText">执行语句</param>
        /// <param name="parameters">参数列表</param>
        /// <param name="excute">执行回调</param>
        /// <returns>执行结果</returns>
        protected abstract T CreateDbCommondAndExcute<T>(string commandText,
           IDictionary<string, object> parameters, Func<IDbCommand, T> excute);

        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="dbCommand"></param>
        /// <param name="parameters"></param>
        protected abstract void SetParameters(IDbCommand dbCommand, IDictionary<string, object> parameters);
        #endregion


        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            _context.Dispose();
        }
    }
}
