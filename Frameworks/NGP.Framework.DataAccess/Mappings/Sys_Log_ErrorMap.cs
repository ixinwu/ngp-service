/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * Class Description:
 * 
 *
 * Comment 					        Revision	        Date                 Author
 * -----------------------------    --------         --------            -----------
 * Created							1.0		    2019/3/4 15:37:43   hulei@ixinwu.com
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
	public class Sys_Log_ErrorMap : BaseDBEntityMap<Sys_Log_Error>
    {
        /// <summary>
        /// 系统异常日志关联映射
        /// </summary>
        /// <param name="builder"></param>
        protected override void PostConfigure(EntityTypeBuilder<Sys_Log_Error> builder)
        {

            // Table & Column Mappings
            builder.ToTable("Sys_Log_Error");

            builder.Property(t => t.ApiUrl)
                .HasMaxLength(200)
                .HasColumnName("ApiUrl");
            builder.Property(t => t.Parameters)
                .HasColumnName("Parameters");
            builder.Property(t => t.BusinessMethod)
                .HasMaxLength(255)
                .HasColumnName("BusinessMethod");
            builder.Property(t => t.ExceptionContent)
                .HasColumnName("ExceptionContent");

            base.PostConfigure(builder);
        }
    }
}
