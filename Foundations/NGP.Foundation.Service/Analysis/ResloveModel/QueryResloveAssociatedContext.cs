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
    public class QueryResloveAssociatedContext
    {
        /// <summary>
        /// 人员列表
        /// </summary>
        public List<Sys_Org_Employee> Employees { get; set; }

        /// <summary>
        /// 部门列表
        /// </summary>
        public List<Sys_Org_Department> Departments { get; set; }

        
    }
}
