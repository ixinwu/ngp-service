/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * PageQueryExtend Description:
 * 分页扩展
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-2-15   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using System;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace NGP.Framework.Core
{
    /// <summary>
    /// 分页扩展
    /// </summary>
    public static class BaseDbEntityExtend
    {
        /// <summary>
        /// 初始化追加时默认字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="workContext"></param>
        public static void InitAddDefaultFields<T>(this T entity,IWorkContext workContext)
            where T : BaseDBEntity
        {
            entity.CreatedBy = workContext.Current.EmplId;
            entity.CreatedDept = workContext.Current.DeptId;
            entity.CreatedTime = DateTime.Now;
            entity.InitUpdateDefaultFields(workContext);
        }

        /// <summary>
        /// 初始化更新时默认字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="workContext"></param>
        public static void InitUpdateDefaultFields<T>(this T entity, IWorkContext workContext)
            where T : BaseDBEntity
        {
            entity.UpdatedBy = workContext.Current.EmplId;
            entity.UpdatedDept = workContext.Current.DeptId;
            entity.UpdatedTime = DateTime.Now;
        }
    }
}
