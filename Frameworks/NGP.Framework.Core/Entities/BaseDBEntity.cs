/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * BaseDBEntity Description:
 * DB模型基类
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-15   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using System;

namespace NGP.Framework.Core
{
    /// <summary>
    /// DB模型基类
    /// </summary>
    public abstract class BaseDBEntity : BaseEntity
    {
        #region Properties
        /// <summary>
        /// 删除标识
        /// </summary>
        public virtual bool IsDelete { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreatedTime { get; set; }
        /// <summary>
        /// 创建者
        /// </summary>
        public virtual string CreatedBy { get; set; }
        /// <summary>
        /// 创建部门
        /// </summary>
        public virtual string CreatedDept { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public virtual DateTime UpdatedTime { get; set; }
        /// <summary>
        /// 更新者
        /// </summary>
        public virtual string UpdatedBy { get; set; }
        /// <summary>
        /// 更新部门
        /// </summary>
        public virtual string UpdatedDept { get; set; }
        /// <summary>
        /// 排序字段
        /// </summary>
        public virtual int? OrderIndex { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }
        #endregion
    }
}
