/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * UnitDbContext Description:
 * 工作单元上下文
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-20   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using Microsoft.EntityFrameworkCore;
using NGP.Framework.Core;
using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;

namespace NGP.Framework.DataAccess
{
    /// <summary>
    /// 工作单元上下文
    /// </summary>
    public class UnitObjectContext : DbContext, IDbContext
    {
        #region Ctor
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="options"></param>
        public UnitObjectContext(DbContextOptions<UnitObjectContext> options) : base(options)
        {
        }

        #endregion

        #region Utilities

        /// <summary>
        /// 进一步配置模型
        /// </summary>
        /// <param name="modelBuilder">用于构造此上下文的模型的生成器</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //dynamically load all entity and query type configurations
            var typeConfigurations = Assembly.GetExecutingAssembly().GetTypes().Where(type =>
                (type.BaseType?.IsGenericType ?? false)
                    && (type.BaseType.GetGenericTypeDefinition() == typeof(BaseDBEntityMap<>)));

            foreach (var typeConfiguration in typeConfigurations)
            {
                dynamic configuration = Activator.CreateInstance(typeConfiguration);
                configuration.ApplyConfiguration(modelBuilder);
            }

            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// 通过添加传递的参数修改输入SQL查询
        /// </summary>
        /// <param name="sql">The raw SQL query</param>
        /// <param name="parameters">要分配给参数的值</param>
        /// <returns>修改的原始SQL查询</returns>
        protected virtual string CreateSqlWithParameters(string sql, params object[] parameters)
        {
            //add parameters to sql
            for (var i = 0; i <= (parameters?.Length ?? 0) - 1; i++)
            {
                if (!(parameters[i] is DbParameter parameter))
                    continue;

                sql = $"{sql}{(i > 0 ? "," : string.Empty)} {parameter.ParameterName}";

                //whether parameter is output
                if (parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Output)
                    sql = $"{sql} output";
            }

            return sql;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 创建可用于查询和保存实体实例的数据库集
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <returns>给定实体类型的集合</returns>
        public virtual new DbSet<TEntity> Set<TEntity>() where TEntity : class
            => base.Set<TEntity>();

        /// <summary>
        /// 生成脚本以创建当前模型的所有表
        /// </summary>
        /// <returns>A SQL script</returns>
        public virtual string GenerateCreateScript()
            => Database.GenerateCreateScript();

        /// <summary>
        /// 基于原始SQL查询为查询类型创建LINQ查询
        /// </summary>
        /// <typeparam name="TQuery">Query type</typeparam>
        /// <param name="sql">The raw SQL query</param>
        /// <param name="parameters">The values to be assigned to parameters</param>
        /// <returns>表示原始SQL查询的IQueryable</returns>
        public virtual IQueryable<TQuery> QueryFromSql<TQuery>(string sql, params object[] parameters) where TQuery : class
            => Query<TQuery>().FromSql(CreateSqlWithParameters(sql, parameters), parameters);


        /// <summary>
        /// 基于原始SQL查询为实体创建LINQ查询
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <param name="sql">The raw SQL query</param>
        /// <param name="parameters">The values to be assigned to parameters</param>
        /// <returns>表示原始SQL查询的IQueryable</returns>
        public virtual IQueryable<TEntity> EntityFromSql<TEntity>(string sql, params object[] parameters) where TEntity : BaseDBEntity
            => Set<TEntity>().FromSql(CreateSqlWithParameters(sql, parameters), parameters);


        /// <summary>
        /// 对数据库执行给定的SQL
        /// </summary>
        /// <param name="sql">The SQL to execute</param>
        /// <param name="doNotEnsureTransaction">true-不确保事务创建；false-确保事务创建。.</param>
        /// <param name="timeout">用于命令的超时。注意，命令超时与连接超时不同，连接超时通常设置在数据库连接字符串上。</param>
        /// <param name="parameters">用于SQL的参数</param>
        /// <returns>受影响的行数</returns>
        public virtual int ExecuteSqlCommand(RawSqlString sql, bool doNotEnsureTransaction = false, int? timeout = null, params object[] parameters)
        {
            //set specific command timeout
            var previousTimeout = Database.GetCommandTimeout();
            Database.SetCommandTimeout(timeout);

            var result = 0;
            if (!doNotEnsureTransaction)
            {
                //use with transaction
                using (var transaction = Database.BeginTransaction())
                {
                    result = Database.ExecuteSqlCommand(sql, parameters);
                    transaction.Commit();
                }
            }
            else
                result = Database.ExecuteSqlCommand(sql, parameters);

            //return previous timeout back
            Database.SetCommandTimeout(previousTimeout);

            return result;
        }

        #endregion
    }
}
