﻿/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * ExcuteSqlCommand Description:
 * 执行SQL对象
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-15   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using System.Collections.Generic;
using System.Linq;

namespace NGP.Framework.Core
{
    /// <summary>
    /// 执行SQL对象
    /// </summary>
    public class ExcuteSqlCommand
    {
        #region Ctor
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="commandText">sql语句</param>
        public ExcuteSqlCommand(string commandText) : this()
        {
            CommandText = commandText.Trim();
        }

        /// <summary>
        /// ctor
        /// </summary>
        public ExcuteSqlCommand()
        {
            ParameterCollection = new Dictionary<string, object>();
        }

        #endregion

        #region Properties
        /// <summary>
        /// SQL语句
        /// </summary>
        public string CommandText { get; set; }

        /// <summary>
        /// 参数列表
        /// </summary>
        public IDictionary<string, object> ParameterCollection { get; }
        #endregion

        /// <summary>
        /// 转换批量命令
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static ExcuteSqlCommand FormatterCommands(List<ExcuteSqlCommand> source)
        {
            var result = new ExcuteSqlCommand();
            result.CommandText = string.Join(" \n", source.Select(s => s.CommandText));
            foreach (var item in source)
            {
                foreach (var parame in item.ParameterCollection)
                {
                    result.ParameterCollection[parame.Key] = parame.Value;
                }
            }
            return result;
        }
    }
}
