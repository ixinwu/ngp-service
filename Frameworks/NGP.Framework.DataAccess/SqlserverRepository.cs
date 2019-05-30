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

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace NGP.Framework.DataAccess
{
    /// <summary>
    /// 工作单元仓储
    /// </summary>
    public class SqlserverRepository : UnitRepository
    {
        #region Ctor
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="context"></param>
        public SqlserverRepository(IDbContext context) : base(context)
        {
        }
        #endregion

        /// <summary>
        /// 创建MySqlCommand执行
        /// </summary>
        /// <typeparam name="T">结果泛型</typeparam>
        /// <param name="commandText">执行语句</param>
        /// <param name="parameters">参数列表</param>
        /// <param name="excute">执行回调</param>
        /// <returns>执行结果</returns>
        protected override T CreateDbCommondAndExcute<T>(string commandText,
           IDictionary<string, object> parameters, Func<IDbCommand, T> excute)
        {
            var conn = _context.Database.GetDbConnection() as SqlConnection;
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }

            using (var dbCommand = new SqlCommand())
            {
                dbCommand.CommandType = CommandType.Text;
                dbCommand.Connection = conn;
                SetParameters(dbCommand, parameters);
                dbCommand.CommandText = commandText;

                return excute(dbCommand);
            }
        }

        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="dbCommand"></param>
        /// <param name="parameters"></param>
        protected override void SetParameters(IDbCommand dbCommand, IDictionary<string, object> parameters)
        {
            if (parameters == null)
            {
                return;
            }
            foreach (var parameter in parameters)
            {
                if (parameter.Value != null)
                {
                    dbCommand.Parameters.Add(new SqlParameter(parameter.Key, parameter.Value));
                    continue;
                }

                dbCommand.Parameters.Add(new SqlParameter(parameter.Key, DBNull.Value));
            }
        }
    }
}
