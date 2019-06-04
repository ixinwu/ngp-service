/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * Sys_Config_CommonMap Description:
 * 
 *
 * Comment 					        Revision	        Date                 Author
 * -----------------------------    --------         --------            -----------
 * Created							1.0		    2019/3/4 15:37:45   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NGP.Framework.Core;

namespace NGP.Framework.DataAccess
{
    /// <summary>
    /// 通用配置映射
    /// </summary>
	public class Sys_Config_CommonMap : BaseDBEntityMap<Sys_Config_Common>
    {
        /// <summary>
        /// 服务运行配置映射
        /// </summary>
        /// <param name="builder"></param>
        protected override void PostConfigure(EntityTypeBuilder<Sys_Config_Common> builder)
        {
            // Table & Column Mappings
            builder.ToTable("Sys_Config_Common");
            builder.Property(t => t.ConfigKey)
                .IsRequired()    
                .HasMaxLength(50)
                .HasColumnName("ConfigKey");
            builder.Property(t => t.ConfigName)
                .IsRequired()    
                .HasMaxLength(100)
                .HasColumnName("ConfigName");
            builder.Property(t => t.ConfigJson)
                .HasColumnName("ConfigJson");
            base.PostConfigure(builder);
        }
    }
}
