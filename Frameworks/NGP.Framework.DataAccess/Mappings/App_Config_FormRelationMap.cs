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
	public class App_Config_FormRelationMap : BaseDBEntityMap<App_Config_FormRelation>
    {
        /// <summary>
        /// 关联映射
        /// </summary>
        /// <param name="builder"></param>
        protected override void PostConfigure(EntityTypeBuilder<App_Config_FormRelation> builder)
        {
            // Table & Column Mappings
            builder.ToTable("App_Config_FormRelation");
            builder.Property(t => t.SourceAppKey)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(t => t.SourceFormKey)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(t => t.SourceFieldKey)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(t => t.RelationAppKey)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(t => t.RelationFormKey)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(t => t.RelationFieldKey)
                .IsRequired()
                .HasMaxLength(100);

            base.PostConfigure(builder);
        }
    }
}
