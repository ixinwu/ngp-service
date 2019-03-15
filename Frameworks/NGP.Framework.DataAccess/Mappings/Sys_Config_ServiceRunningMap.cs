/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * Class Description:
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
    /// 服务运行配置映射
    /// </summary>
	public class Sys_Config_ServiceRunningMap : BaseDBEntityMap<Sys_Config_ServiceRunning>
    {
        /// <summary>
        /// 服务运行配置映射
        /// </summary>
        /// <param name="builder"></param>
        protected override void PostConfigure(EntityTypeBuilder<Sys_Config_ServiceRunning> builder)
        {
            // Table & Column Mappings
            builder.ToTable("Sys_Config_ServiceRunning");
            builder.Property(t => t.ConfigKey)
                .IsRequired()    
                .HasMaxLength(50)
                .HasColumnName("ConfigKey");
            builder.Property(t => t.ConfigName)
                .IsRequired()    
                .HasMaxLength(100)
                .HasColumnName("ConfigName");
            builder.Property(t => t.ValidStartTime)
                .HasColumnName("ValidStartTime");
            builder.Property(t => t.ValidEndTime)
                .HasColumnName("ValidEndTime");
            builder.Property(t => t.IsEnable)
                .IsRequired()    
                .HasColumnName("IsEnable");
            builder.Property(t => t.CronConfig)
                .HasColumnName("CronConfig");
            base.PostConfigure(builder);
        }
    }
}
