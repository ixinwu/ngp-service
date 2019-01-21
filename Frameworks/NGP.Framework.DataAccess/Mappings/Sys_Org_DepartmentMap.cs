/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * Sys_Org_DepartmentMap Description:
 * 部门实体映射
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
    /// 部门实体映射
    /// </summary>
    public class Sys_Org_DepartmentMap : BaseDBEntityMap<Sys_Org_Department>
    {
        /// <summary>
        /// 部门实体映射
        /// </summary>
        /// <param name="builder"></param>
        protected override void PostConfigure(EntityTypeBuilder<Sys_Org_Department> builder)
        {
            // 表
            builder.ToTable("Sys_Org_Department");

            builder.Property(t => t.DeptName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(t => t.DeptShortName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(t => t.ParentId)
                .HasMaxLength(50);

            base.PostConfigure(builder);
        }
    }
}
