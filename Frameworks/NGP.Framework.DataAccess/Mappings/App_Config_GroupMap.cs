/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * Class Description:
 * 
 *
 * Comment 					        Revision	        Date                 Author
 * -----------------------------    --------         --------            -----------
 * Created							1.0		    2019/3/7 14:50:07   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NGP.Framework.Core;

namespace NGP.Framework.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
	public class App_Config_GroupMap : BaseDBEntityMap<App_Config_Group>
    {
        /// <summary>
        /// 关联映射
        /// </summary>
        /// <param name="builder"></param>
        protected override void PostConfigure(EntityTypeBuilder<App_Config_Group> builder)
        {
            // Table & Column Mappings
            builder.ToTable("App_Config_Group");
            builder.Property(t => t.GroupKey)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(t => t.GroupValue)
                .IsRequired()
                .HasMaxLength(200);
            builder.Property(t => t.AscriptionAppKey)
                .HasMaxLength(100);

            base.PostConfigure(builder);
        }
    }
}
