/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * Sys_Org_Empl_DeptMap Description:
 * 雇员部门关联映射
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
    /// 雇员部门关联映射
    /// </summary>
    public class Sys_Org_Empl_DeptMap : BaseDBEntityMap<Sys_Org_Empl_Dept>
    {
        /// <summary>
        /// 雇员部门关联映射
        /// </summary>
        /// <param name="builder"></param>
        protected override void PostConfigure(EntityTypeBuilder<Sys_Org_Empl_Dept> builder)
        {
            // 表
            builder.ToTable("Sys_Org_Empl_Dept");

            builder.Property(t => t.EmplId)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(t => t.DeptId)
                .IsRequired()
                .HasMaxLength(50);

            base.PostConfigure(builder);
        }
    }
}
