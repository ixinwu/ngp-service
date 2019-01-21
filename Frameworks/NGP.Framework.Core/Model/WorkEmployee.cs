/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * WorkEmployee Description:
 * 当前工作的人员信息
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-15   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using System.Collections.Generic;

namespace NGP.Framework.Core
{
    /// <summary>
    /// 当前工作的人员信息
    /// </summary>
    public class WorkEmployee
    {
        /// <summary>
        /// 人员ID
        /// </summary>
        public string EmplId { get; set; }
        /// <summary>
        /// 登录名
        /// </summary>
        public string LoginName { get; set; }
        /// <summary>
        /// 人员No
        /// </summary>
        public string EmplNo { get; set; }
        /// <summary>
        /// 人员名称
        /// </summary>
        public string EmplName { get; set; }
        /// <summary>
        /// 是否系统admin
        /// </summary>
        public bool IsSystemAdmin { get; set; }
        /// <summary>
        /// 所属角色id
        /// </summary>
        public List<string> RoleIds { get; set; }
        /// <summary>
        /// 部门ID
        /// </summary>
        public string DeptId { get; set; }
    }
}
