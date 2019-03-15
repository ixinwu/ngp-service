/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * IUnitRepository Description:
 * 仓储接口
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-15   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NGP.Framework.Core
{
    /// <summary>
    /// 仓储接口
    /// </summary>
    public interface IUnitRepository : IDisposable
    {
        /// <summary>
        /// 查询满足条件的第一个
        /// </summary>
        /// <typeparam name="TEntity">返回值类型</typeparam>
        /// <param name="criteria">查询条件</param>
        /// <returns>查询结果</returns>
        TEntity FirstOrDefault<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : BaseDBEntity;

        /// <summary>
        /// 根据ID查找
        /// </summary>
        /// <typeparam name="TEntity">返回值类型</typeparam>
        /// <param name="id">主键值</param>
        /// <returns>查询结果</returns>
        TEntity FindById<TEntity>(string id) where TEntity : BaseDBEntity;

        /// <summary>
        /// 追加对象
        /// </summary>
        /// <typeparam name="TEntity">参数类型</typeparam>
        /// <param name="entity">追加对象</param>
        void Insert<TEntity>(TEntity entity) where TEntity : BaseDBEntity;

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <typeparam name="TEntity">参数类型</typeparam>
        /// <param name="entities">插入列表</param>
        void Insert<TEntity>(List<TEntity> entities) where TEntity : class;

        /// <summary>
        /// 更新对象
        /// </summary>
        /// <typeparam name="TEntity">参数类型</typeparam>
        /// <param name="entity">需要更新的实体</param>
        void Update<TEntity>(TEntity entity) where TEntity : BaseDBEntity;

        /// <summary>
        /// EF扩展更新
        /// </summary>
        /// <typeparam name="TEntity">参数类型</typeparam>
        /// <param name="updateExpression">更新表达式</param>
        /// <param name="criteria">查询表达式</param>
        /// <returns></returns>        
        int UpdateByExpression<TEntity>(Expression<Func<TEntity, TEntity>> updateExpression,
            Expression<Func<TEntity, bool>> criteria = null) where TEntity : class;

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <typeparam name="TEntity">参数类型</typeparam>
        /// <param name="entity">需要删除的实体</param>
        void Delete<TEntity>(TEntity entity) where TEntity : BaseDBEntity;

        /// <summary>
        /// EF扩展删除
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="criteria">查询表达式</param>
        /// <returns></returns>
        int DeleteByExpression<T>(Expression<Func<T, bool>> criteria = null) where T : class;

        /// <summary>
        /// 获取表（EF功能）
        /// </summary>
        /// <typeparam name="TEntity">查询类型</typeparam>
        ///  <param name="criteria">条件表达式</param>
        /// <returns>当前类型的表接口</returns>
        IQueryable<TEntity> All<TEntity>(Expression<Func<TEntity, bool>> criteria = null) where TEntity : class;

        /// <summary>
        /// 获取一个启用了“无跟踪”的表（EF功能），仅当只为只读操作加载记录时才使用该表
        /// </summary>
        /// <typeparam name="TEntity">查询类型</typeparam>
        ///  <param name="criteria">条件表达式</param>
        /// <returns>当前类型的表接口</returns>
        IQueryable<TEntity> AllNoTracking<TEntity>(Expression<Func<TEntity, bool>> criteria = null) where TEntity : class;

        /// <summary>
        /// 基于原始SQL查询为查询类型创建LINQ查询
        /// </summary>
        /// <typeparam name="TQuery">Query type</typeparam>
        /// <param name="sql">The raw SQL query</param>
        /// <param name="parameters">The values to be assigned to parameters</param>
        /// <returns>表示原始SQL查询的IQueryable</returns>
        IQueryable<TQuery> QueryFromSql<TQuery>(string sql, IDictionary<string, object> parameters = null) where TQuery : class;

        /// <summary>
        /// 基于原始SQL查询为实体创建LINQ查询
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <param name="sql">原始SQL查询</param>
        /// <param name="parameters">要分配给参数的值</param>
        /// <returns>表示原始SQL查询的IQueryable</returns>
        IQueryable<TEntity> EntityFromSql<TEntity>(string sql, IDictionary<string, object> parameters = null) where TEntity : BaseDBEntity;

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <typeparam name="TEntity">查询类型</typeparam>
        ///  <param name="criteria">条件表达式</param>
        /// <returns>当前条件是否存在</returns>
        bool Any<TEntity>(Expression<Func<TEntity, bool>> criteria = null) where TEntity : BaseDBEntity;

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <typeparam name="TEntity">参数类型</typeparam>
        /// <param name="entities">插入列表</param>
        void BulkInsert<TEntity>(IList<TEntity> entities) where TEntity : class;

        /// <summary>
        /// 将此上下文中所做的所有更改保存到数据库
        /// </summary>
        /// <returns>写入数据库的状态条目数</returns>
        int SaveChanges();

        #region excute command
        /// <summary>
        /// 读取列表数据(泛型)
        /// </summary>
        /// <typeparam name="TEntity">返回结果类型</typeparam>
        /// <param name="commandText">执行语句</param>
        /// <param name="parameters">参数列表</param>
        /// <param name="setItem">设定值回调</param>
        /// <returns>返回结果</returns>
        List<TEntity> ReadValues<TEntity>(string commandText,
            IDictionary<string, object> parameters = null,
            Action<TEntity> setItem = null)
            where TEntity : class, new();

        /// <summary>
        /// 读取列表数据
        /// </summary>
        /// <param name="commandText">执行语句</param>
        /// <param name="type">返回结果类型</param>
        /// <param name="parameters">参数列表</param>
        /// <param name="setItem">设定值回调</param>
        /// <returns>返回结果</returns>
        List<object> ReadValues(string commandText,
            Type type,
             IDictionary<string, object> parameters = null
            , Action<dynamic> setItem = null);

        /// <summary>
        /// 读取列表数据,根据回调设定值,值通过key,value提供
        /// </summary>
        /// <param name="commandText">执行语句</param>
        /// <param name="parameters">参数列表</param>        
        /// <returns>返回结果</returns>
        List<IDictionary<string, object>> ReadValues(string commandText,
            IDictionary<string, object> parameters = null);

        /// <summary>
        /// 读取datatable
        /// </summary>
        /// <param name="commandText">执行语句</param>
        /// <param name="parameters">参数列表</param>        
        /// <returns>返回结果</returns>
        DataTable ReadTable(string commandText,
            IDictionary<string, object> parameters = null);

        /// <summary>
        /// 读取单条记录（泛型）
        /// </summary>
        /// <typeparam name="TEntity">返回结果类型</typeparam>
        /// <param name="commandText">执行语句</param>
        /// <param name="parameters">参数列表</param>
        /// <param name="setItem">设定值回调</param>
        /// <returns>返回结果</returns>
        TEntity ExecuteReader<TEntity>(string commandText,
            IDictionary<string, object> parameters = null
            , Action<TEntity> setItem = null)
            where TEntity : class, new();

        /// <summary>
        /// 读取详细记录,根据回调设定值,值通过key,value提供
        /// </summary>
        /// <param name="commandText">执行语句</param>
        /// <param name="parameters">参数列表</param>        
        /// <returns>返回结果</returns>
        IDictionary<string, object> ExecuteReader(string commandText,
            IDictionary<string, object> parameters = null);

        /// <summary>
        /// 读取详细记录,根据回调设定值,值通过key,value提供
        /// </summary>
        /// <param name="commandText">执行语句</param>
        /// <param name="type">类型</param>
        /// <param name="parameters">参数列表</param>
        /// <param name="valueFormatters">值格式化</param>
        /// <param name="propertyMapping">属性映射</param>
        /// <param name="setItem">对象格式化</param>     
        /// <returns>返回结果</returns>
        object ExecuteReader(string commandText,
             Type type,
             IDictionary<string, object> parameters = null,
             IDictionary<string, Func<object, object>> valueFormatters = null,
             IDictionary<string, string> propertyMapping = null,
             Action<dynamic> setItem = null);

        /// <summary>
        /// 读取第一条第一列的结果（泛型）
        /// </summary>
        /// <typeparam name="T">结果类型</typeparam>
        /// <param name="commandText">执行语句</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>返回结果</returns>
        T ExecuteScalar<T>(string commandText, IDictionary<string, object> parameters = null);

        /// <summary>
        /// 执行数据库操作
        /// </summary>
        /// <param name="commandText">执行语句</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>影响条数</returns>
        int ExecuteNonQuery(string commandText, IDictionary<string, object> parameters = null);

        /// <summary>
        /// 执行大批量插入数据库操作
        /// </summary>
        /// <param name="tableName">指定表</param>
        /// <param name="insertData">指定DataTable数据源</param>
        /// <returns></returns>
        string InsertSqlBulkNonQuery(string tableName, DataTable insertData);
        #endregion
    }
}
