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
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using Z.EntityFramework.Plus;

namespace NGP.Framework.DataAccess
{
    /// <summary>
    /// 工作单元仓储
    /// </summary>
    public class UnitRepository : IUnitRepository
    {
        /// <summary>
        /// ef上下文
        /// </summary>
        private readonly IDbContext _context;

        #region Ctor
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="context"></param>
        public UnitRepository(IDbContext context)
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
        /// <param name="setItem">设定值回调</param>
        /// <returns>返回结果</returns>
        public List<TEntity> ReadValues<TEntity>(string commandText,
            IDictionary<string, object> parameters = null,
            Action<TEntity> setItem = null)
            where TEntity : class, new()
        {
            Func<SqlCommand, List<TEntity>> excute = (dbCommand) =>
            {
                var result = new List<TEntity>();

                using (IDataReader dataReader = dbCommand.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        var item = dataReader.GetReaderData<TEntity>();
                        setItem?.Invoke(item);
                        result.Add(item);
                    }
                }
                return result;
            };
            return CreateDbCommondAndExcute(commandText, parameters,
                excute);
        }

        /// <summary>
        /// 读取列表数据
        /// </summary>
        /// <param name="commandText">执行语句</param>
        /// <param name="type">返回结果类型</param>
        /// <param name="parameters">参数列表</param>
        /// <param name="setItem">设定值回调</param>
        /// <returns>返回结果</returns>
        public List<object> ReadValues(string commandText,
            Type type,
             IDictionary<string, object> parameters = null
            , Action<dynamic> setItem = null)
        {
            Func<SqlCommand, List<object>> excute = (dbCommand) =>
            {
                var result = new List<object>();

                using (IDataReader dataReader = dbCommand.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        var item = dataReader.GetReaderData(type);
                        setItem?.Invoke(item);
                        result.Add(item);

                    }
                }
                return result;
            };
            return CreateDbCommondAndExcute(commandText, parameters,
                excute);
        }

        /// <summary>
        /// 读取列表数据,根据回调设定值,值通过key,value提供
        /// </summary>
        /// <param name="commandText">执行语句</param>
        /// <param name="parameters">参数列表</param>        
        /// <returns>返回结果</returns>
        public List<IDictionary<string, object>> ReadValues(string commandText,
            IDictionary<string, object> parameters = null)
        {
            Func<SqlCommand, List<IDictionary<string, object>>> excute = (dbCommand) =>
            {
                var result = new List<IDictionary<string, object>>();

                using (IDataReader dataReader = dbCommand.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        var item = new Dictionary<string, object>();
                        item.SetReaderValue(dataReader);
                        result.Add(item);
                    }
                }
                return result;
            };
            return CreateDbCommondAndExcute(commandText, parameters,
                excute);
        }

        /// <summary>
        /// 读取datatable
        /// </summary>
        /// <param name="commandText">执行语句</param>
        /// <param name="parameters">参数列表</param>        
        /// <returns>返回结果</returns>
        public DataTable ReadTable(string commandText,
            IDictionary<string, object> parameters = null)
        {
            Func<SqlCommand, DataTable> excute = (dbCommand) =>
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(dbCommand);
                da.Fill(dt);
                return dt;
            };
            return CreateDbCommondAndExcute(commandText, parameters, excute);
        }

        /// <summary>
        /// 读取详细记录,根据回调设定值,值通过key,value提供
        /// </summary>
        /// <param name="commandText">执行语句</param>
        /// <param name="parameters">参数列表</param>        
        /// <returns>返回结果</returns>
        public IDictionary<string, object> ExecuteReader(string commandText,
             IDictionary<string, object> parameters = null)
        {
            Func<SqlCommand, IDictionary<string, object>> excute = (dbCommand) =>
            {
                var result = new Dictionary<string, object>();

                using (IDataReader dataReader = dbCommand.ExecuteReader())
                {
                    if (dataReader.Read())
                    {
                        result.SetReaderValue(dataReader);
                    }
                }
                return result;
            };
            return CreateDbCommondAndExcute(commandText, parameters,
                excute);
        }

        /// <summary>
        /// 读取详细记录,根据回调设定值
        /// </summary>
        /// <param name="commandText">执行语句</param>
        /// <param name="type">类型</param>
        /// <param name="parameters">参数列表</param>
        /// <param name="valueFormatters">值格式化</param>
        /// <param name="propertyMapping">属性映射</param>
        /// <param name="setItem">对象格式化</param>     
        /// <returns>返回结果</returns>
        public object ExecuteReader(string commandText,
              Type type,
              IDictionary<string, object> parameters = null,
              IDictionary<string, Func<object, object>> valueFormatters = null,
              IDictionary<string, string> propertyMapping = null,
              Action<dynamic> setItem = null)
        {
            Func<SqlCommand, object> excute = (dbCommand) =>
            {
                using (IDataReader dataReader = dbCommand.ExecuteReader())
                {
                    if (dataReader.Read())
                    {
                        var result = dataReader.GetReaderData(type, valueFormatters, propertyMapping);
                        setItem?.Invoke(result);
                        return result;
                    }
                }
                return null;
            };
            return CreateDbCommondAndExcute(commandText, parameters,
                excute);
        }

        /// <summary>
        /// 读取单条记录（泛型）
        /// </summary>
        /// <typeparam name="TEntity">返回结果类型</typeparam>
        /// <param name="commandText">执行语句</param>
        /// <param name="parameters">参数列表</param>
        /// <param name="setItem">设定值回调</param>
        /// <returns>返回结果</returns>
        public TEntity ExecuteReader<TEntity>(string commandText,
             IDictionary<string, object> parameters = null
             , Action<TEntity> setItem = null)
             where TEntity : class, new()
        {
            Func<SqlCommand, TEntity> excute = (dbCommand) =>
            {
                var result = default(TEntity);
                using (IDataReader dataReader = dbCommand.ExecuteReader())
                {
                    if (dataReader.Read())
                    {
                        result = dataReader.GetReaderData<TEntity>();
                        setItem?.Invoke(result);
                    }
                }
                return result;
            };

            return CreateDbCommondAndExcute<TEntity>(commandText, parameters, excute);
        }

        /// <summary>
        /// 读取第一条第一列的结果（泛型）
        /// </summary>
        /// <typeparam name="TValue">结果类型</typeparam>
        /// <param name="commandText">执行语句</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>返回结果</returns>
        public TValue ExecuteScalar<TValue>(string commandText, IDictionary<string, object> parameters = null)
        {
            Func<SqlCommand, object> excute = (dbCommand) => dbCommand.ExecuteScalar();
            var obj = CreateDbCommondAndExcute(commandText,
                parameters, excute);
            return obj.ToObject<TValue>();
        }

        /// <summary>
        /// 执行数据库操作
        /// </summary>
        /// <param name="commandText">执行语句</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>影响条数</returns>
        public int ExecuteNonQuery(string commandText, IDictionary<string, object> parameters = null)
        {
            Func<SqlCommand, int> excute = (dbCommand) => dbCommand.ExecuteNonQuery();
            return CreateDbCommondAndExcute(commandText,
                parameters, excute);
        }

        /// <summary>
        /// 大批量插入数据操作
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="insertData"></param>
        /// <returns></returns>
        public string InsertSqlBulkNonQuery(string tableName, DataTable insertData)
        {
            DataTable dtResult = insertData;
            //在处理前 需要把数据库表中列顺序和DataTable列的顺序一样
            //var strSql = string.Format(@"Select ColumnIndex = b.colorder, FieldKey = b.name FROM sysobjects A
            //                            Join syscolumns B ON A.ID = B.ID
            //                            Where A.Name ='{0}' Order by b.colorder",
            //                           tableName);
            var strSql = string.Format(@"Select ColumnIndex =row_number()OVER(ORDER BY A.name DESC), FieldKey = b.name FROM sysobjects A
                                        Join syscolumns B ON A.ID = B.ID
                                        Where A.Name ='{0}' Order by b.colorder",
                           tableName);
            var resultTemp = ReadValues<FieldOrderInfo>(strSql);

            try
            {
                for (int i = 0; i < resultTemp.Count; i++)
                {
                    dtResult.Columns[resultTemp[i].FieldKey].SetOrdinal(resultTemp[i].ColumnIndex - 1);
                }
                var conn = _context.Database.GetDbConnection() as SqlConnection;
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                SqlBulkCopy bulkCopy = new SqlBulkCopy(conn);
                bulkCopy.DestinationTableName = tableName;
                bulkCopy.BatchSize = dtResult.Rows.Count > 100000 ? 100000 : dtResult.Rows.Count;
                conn.Open();
                if (dtResult != null && dtResult.Rows.Count != 0)
                {
                    bulkCopy.WriteToServer(dtResult);
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        #region private methods
        /// <summary>
        /// 创建DBCommand执行
        /// </summary>
        /// <typeparam name="T">结果泛型</typeparam>
        /// <param name="commandText">执行语句</param>
        /// <param name="parameters">参数列表</param>
        /// <param name="excute">执行回调</param>
        /// <returns>执行结果</returns>
        private T CreateDbCommondAndExcute<T>(string commandText,
           IDictionary<string, object> parameters, Func<SqlCommand, T> excute)
        {
            var conn = _context.Database.GetDbConnection() as SqlConnection;
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }

            using (SqlCommand dbCommand = new SqlCommand())
            {

                dbCommand.CommandType = CommandType.Text;
                dbCommand.Connection = conn;
                dbCommand.SetParameters(parameters);
                dbCommand.CommandText = commandText;

                return excute(dbCommand);
            }
        }
        #endregion
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
