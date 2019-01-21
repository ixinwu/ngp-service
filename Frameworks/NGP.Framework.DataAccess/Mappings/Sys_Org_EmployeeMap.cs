/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * Sys_Org_EmployeeMap Description:
 * 雇员实体映射
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-3-20   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NGP.Framework.Core;

namespace NGP.Framework.DataAccess
{
    /// <summary>
    /// 雇员实体
    /// </summary>
    public class Sys_Org_EmployeeMap : BaseDBEntityMap<Sys_Org_Employee>
    {
        /// <summary>
        /// 雇员实体映射
        /// </summary>
        /// <param name="builder"></param>
        protected override void PostConfigure(EntityTypeBuilder<Sys_Org_Employee> builder)
        {
            // 表
            builder.ToTable("Sys_Org_Employee");

            builder.Property(t => t.EmplNo)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(t => t.EmplName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(t => t.EmplSex)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(t => t.EmplBirth);

            builder.Property(t => t.EmplTel)
                .HasMaxLength(50);

            builder.Property(t => t.EmplPhone)
                .HasMaxLength(50);

            builder.Property(t => t.AttendDate)
                .IsRequired();

            builder.Property(t => t.ManagerId)
                .HasMaxLength(50);

            builder.Property(t => t.EmplDisabled)
                .IsRequired();

            builder.Property(t => t.IsSystemAdmin)
                .IsRequired();

            base.PostConfigure(builder);
        }
    }
}
