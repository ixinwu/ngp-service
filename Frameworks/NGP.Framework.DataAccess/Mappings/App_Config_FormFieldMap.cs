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
using Newtonsoft.Json;
using NGP.Framework.Core;
using System.Collections.Generic;

namespace NGP.Framework.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
	public class App_Config_FormFieldMap : BaseDBEntityMap<App_Config_FormField>
    {
        /// <summary>
        /// 关联映射
        /// </summary>
        /// <param name="builder"></param>
        protected override void PostConfigure(EntityTypeBuilder<App_Config_FormField> builder)
        {
            // Table & Column Mappings
            builder.ToTable("App_Config_FormField");
            builder.Property(t => t.AppKey)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(t => t.FormKey)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(t => t.FieldKey)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(t => t.FieldName)
               .IsRequired()
               .HasMaxLength(200);
            builder.Property(t => t.FieldType)
              .IsRequired()
              .HasMaxLength(100);

            builder.Property(t => t.DbConfig)
                .HasConversion(
                v => JsonConvert.SerializeObject(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
                v => JsonConvert.DeserializeObject<FieldDbConfig>(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));

            builder.Property(t => t.VerificationConfig)
                .HasConversion(
                v => JsonConvert.SerializeObject(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
                v => JsonConvert.DeserializeObject<List<FieldVerificationConfig>>(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));

            builder.Property(t => t.BusinessConfig)
                .HasConversion(
                v => JsonConvert.SerializeObject(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
                v => JsonConvert.DeserializeObject<FieldBusinessConfig>(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));

            base.PostConfigure(builder);
        }
    }
}
