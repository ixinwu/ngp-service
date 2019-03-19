/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved.
 * 
 * ResloveContext Description:
 * 解析上下文
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019/2/19  hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using NGP.Framework.Core;
using System;
using System.Collections.Generic;

namespace NGP.Foundation.Service.Analysis
{
    /// <summary>
    /// 解析上下文
    /// </summary>
    public class QueryResloveCommandContext
    {
        /// <summary>
        /// join命令
        /// </summary>
        public string JoinCommand { get; set; }

        /// <summary>
        /// where命令
        /// </summary>
        public ExcuteSqlCommand WhereCommand { get; set; }

        /// <summary>
        /// 权限表达式
        /// </summary>
        public string PermissionWhere { get; set; }

        /// <summary>
        /// 排序表达式
        /// </summary>
        public string SortCommand { get; set; }

        /// <summary>
        /// 执行对象
        /// </summary>
        public ExcuteSqlCommand ExcuteCommand { get; set; }

        /// <summary>
        /// 总条数command
        /// </summary>
        public ExcuteSqlCommand TotalCommand { get; set; }
    }
}
