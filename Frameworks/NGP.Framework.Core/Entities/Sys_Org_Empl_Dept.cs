/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * Sys_Org_Empl_Dept Description:
 * 雇员部门关联
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-3-20   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

namespace NGP.Framework.Core
{
    /// <summary>
    /// 雇员部门关联
    /// </summary>
    public class Sys_Org_Empl_Dept : BaseDBEntity
    {
        /// <summary>
        /// 雇员id
        /// </summary>
        public string EmplId { get; set; }

        /// <summary>
        /// 部门id
        /// </summary>
        public string DeptId { get; set; }
    }
}
