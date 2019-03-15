/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * Class Description:
 * 
 *
 * Comment 					        Revision	        Date                 Author
 * -----------------------------    --------         --------            -----------
 * Created							1.0		    2019/3/4 15:37:44   hulei@ixinwu.com
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
    public class Sys_Config_MessageRouteMap : BaseDBEntityMap<Sys_Config_MessageRoute>
    {
        /// <summary>
        /// 消息路由配置映射
        /// </summary>
        /// <param name="builder"></param>
        protected override void PostConfigure(EntityTypeBuilder<Sys_Config_MessageRoute> builder)
        {

            // Table & Column Mappings
            builder.ToTable("Sys_Config_MessageRoute");

            builder.Property(t => t.MessageRouteKey)
                .IsRequired()
                .HasMaxLength(200);
            builder.Property(t => t.HostName)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("HostName");
            builder.Property(t => t.UserName)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("UserName");
            builder.Property(t => t.Password)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("Password");
            builder.Property(t => t.ExchangeName)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("ExchangeName");
            builder.Property(t => t.ExchangeType)
                .HasMaxLength(200)
                .HasColumnName("ExchangeType");
            builder.Property(t => t.QueueName)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("QueueName");
            builder.Property(t => t.QueueDurable)
                .IsRequired()
                .HasColumnName("QueueDurable");
            builder.Property(t => t.RouteKey)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("RouteKey");
            base.PostConfigure(builder);

        }
    }
}
