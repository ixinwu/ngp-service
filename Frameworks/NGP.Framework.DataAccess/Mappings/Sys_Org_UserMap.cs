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

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NGP.Framework.Core;
using System;

namespace NGP.Framework.DataAccess
{
    /// <summary>
    /// 用户实体
    /// </summary>
    public class Sys_Org_UserMap : BaseDBEntityMap<Sys_Org_User>
    {
        /// <summary>
        /// 雇员实体映射
        /// </summary>
        /// <param name="builder"></param>
        protected override void PostConfigure(EntityTypeBuilder<Sys_Org_User> builder)
        {
            // 表
            builder.ToTable("Sys_Org_User");

            builder.Property(t => t.LoginName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(t => t.UserPwd)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(t => t.EmpId)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(t => t.UserLastLogonIp)
                .HasMaxLength(200);

            builder.Property(t => t.UserLogonTimes);

            base.PostConfigure(builder);
        }
    }
}
