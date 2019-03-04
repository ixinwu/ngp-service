/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * IDbContext Description:
 * 数据库上下文
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-2   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using NGP.Framework.Core;
using System;
using System.Linq;

namespace NGP.Framework.DataAccess
{
    /// <summary>
    /// 数据库上下文
    /// </summary>
    public partial interface IDbContext : IDisposable
    {
        #region Methods

        /// <summary>
        /// 创建可用于查询和保存实体实例的数据库集
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <returns>给定实体类型的集合</returns>
        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        /// <summary>
        /// 生成脚本以创建当前模型的所有表
        /// </summary>
        /// <returns>A SQL script</returns>
        string GenerateCreateScript();

        /// <summary>
        /// 基于原始SQL查询为查询类型创建LINQ查询
        /// </summary>
        /// <typeparam name="TQuery">Query type</typeparam>
        /// <param name="sql">The raw SQL query</param>
        /// <param name="parameters">The values to be assigned to parameters</param>
        /// <returns>表示原始SQL查询的IQueryable</returns>
        IQueryable<TQuery> QueryFromSql<TQuery>(string sql, params object[] parameters) where TQuery : class;

        /// <summary>
        /// 基于原始SQL查询为实体创建LINQ查询
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <param name="sql">原始SQL查询</param>
        /// <param name="parameters">要分配给参数的值</param>
        /// <returns>表示原始SQL查询的IQueryable</returns>
        IQueryable<TEntity> EntityFromSql<TEntity>(string sql, params object[] parameters) where TEntity : BaseDBEntity;

        /// <summary>
        /// 对数据库执行给定的SQL
        /// </summary>
        /// <param name="sql">The SQL to execute</param>
        /// <param name="doNotEnsureTransaction">true-不确保事务创建；false-确保事务创建。.</param>
        /// <param name="timeout">用于命令的超时。注意，命令超时与连接超时不同，连接超时通常设置在数据库连接字符串上。</param>
        /// <param name="parameters">用于SQL的参数</param>
        /// <returns>受影响的行数</returns>
        int ExecuteSqlCommand(RawSqlString sql, bool doNotEnsureTransaction = false, int? timeout = null, params object[] parameters);

        /// <summary>
        /// 数据库对象
        /// </summary>
        DatabaseFacade Database { get; }

        /// <summary>
        /// 将此上下文中所做的所有更改保存到数据库
        /// </summary>
        /// <returns>写入数据库的状态条目数</returns>
        int SaveChanges();

        #endregion
    }
}
