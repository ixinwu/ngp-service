/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * BaseDBEntityMap Description:
 * DB映射基类
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-2   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NGP.Framework.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace NGP.Framework.DataAccess
{
    /// <summary>
    /// DB映射基类
    /// </summary>
    public class BaseDBEntityMap<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : BaseDBEntity
    {
        #region Utilities

        /// <summary>
        /// 开发人员可以在自定义分部类中重写此方法，以便添加一些自定义配置代码
        /// </summary>
        /// <param name="builder">用于配置实体的生成器</param>
        protected virtual void PostConfigure(EntityTypeBuilder<TEntity> builder)
        {
            // 主键
            builder.HasKey(s => s.Id);

            // 表id
            builder.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(50);

            // 删除标志
            builder.Property(t => t.IsDelete)
                .IsRequired();

            // 创建时间
            builder.Property(t => t.CreatedTime)
                .IsRequired();

            // 创建者
            builder.Property(t => t.CreatedBy)
                .IsRequired()
                .HasMaxLength(50);

            // 创建部门
            builder.Property(t => t.CreatedDept)
                .IsRequired()
                .HasMaxLength(50);

            // 更新时间
            builder.Property(t => t.UpdatedTime)
               .IsRequired();

            // 更新者
            builder.Property(t => t.UpdatedBy)
                 .IsRequired()
                 .HasMaxLength(50);

            // 更新部门
            builder.Property(t => t.UpdatedDept)
                .IsRequired()
                .HasMaxLength(50);
        }

        #endregion

        #region Methods

        /// <summary>
        /// 配置实体
        /// </summary>
        /// <param name="builder">用于配置实体的生成器</param>
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            //add custom configuration
            this.PostConfigure(builder);
        }

        /// <summary>
        /// 应用此映射配置
        /// </summary>
        /// <param name="modelBuilder">用于为数据库上下文构造模型的生成器</param>
        public virtual void ApplyConfiguration(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(this);
        }

        #endregion
    }
}
