/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * Sys_Org_User Description:
 * 用户实体
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
    /// 用户实体
    /// </summary>
    public class Sys_Org_User : BaseDBEntity
    {
        /// <summary>
        /// 登录名称
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string UserPwd { get; set; }

        /// <summary>
        /// 雇员id
        /// </summary>
        public string EmpId { get; set; }

        /// <summary>
        /// 是否需要更改密码
        /// </summary>
        public bool? UserNeedChangePwd { get; set; }

        /// <summary>
        /// 最后更新密码时间
        /// </summary>
        public DateTime? UserPwdLastChangeTime { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool UserDisabled { get; set; }

        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime? UserLastLogonTime { get; set; }

        /// <summary>
        /// 最后登录ip
        /// </summary>
        public string UserLastLogonIp { get; set; }

        /// <summary>
        /// 登录次数
        /// </summary>
        public int? UserLogonTimes { get; set; }
    }
}
