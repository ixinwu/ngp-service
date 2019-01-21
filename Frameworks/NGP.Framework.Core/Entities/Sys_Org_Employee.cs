/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * Sys_Org_Employee Description:
 * 雇员实体
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-3-20   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using System;

namespace NGP.Framework.Core
{
    /// <summary>
    /// 雇员实体
    /// </summary>
    public class Sys_Org_Employee : BaseDBEntity
    {
        /// <summary>
        /// 雇员no
        /// </summary>
        public string EmplNo { get; set; }

        /// <summary>
        /// 雇员姓名
        /// </summary>
        public string EmplName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public string EmplSex { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public DateTime EmplBirth { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string EmplTel { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public string EmplPhone { get; set; }

        /// <summary>
        /// 入职日期
        /// </summary>
        public DateTime AttendDate { get; set; }

        /// <summary>
        /// 上级id
        /// </summary>
        public string ManagerId { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool EmplDisabled { get; set; }

        /// <summary>
        /// 是否系统管理员
        /// </summary>
        public bool IsSystemAdmin { get; set; }
    }
}
