/* ------------------------------------------------------------------------------
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * QueryResolveAssociatedQueryStep Description:
 * 解析关联查询步骤
 *
 * Comment 					        Revision	Date                  Author
 * -----------------------------    --------    ------------------    ----------------
 * Created							1.0		    2019/3/18 15:56:08    hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using NGP.Framework.Core;
using System.Linq;

namespace NGP.Foundation.Service.Analysis
{
    /// <summary>
    /// 解析关联查询步骤
    /// </summary>
    public class QueryResolveAssociatedQueryStep : StepBase<QueryResolveContext>
    {
        /// <summary>
        /// 执行上下文
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public override bool Process(QueryResolveContext ctx)
        {
            var unitRepository = Singleton<IEngine>.Instance.Resolve<IUnitRepository>();
            // 是否有人员名称字段
            var hasEmployee = ctx.GenerateContext.GenerateNameFields.Any(s => s.FieldType.ToEnum<FieldType>() == FieldType.EmployeeType);
            if (hasEmployee)
            {
                ctx.AssociatedContext.Employees = unitRepository.AllNoTracking<Sys_Org_Employee>(s => !s.IsDelete).ToList();
            }

            // 是否有部门名称字段
            var hasDept = ctx.GenerateContext.GenerateNameFields.Any(s => s.FieldType.ToEnum<FieldType>() == FieldType.DeptType);
            if (hasDept)
            {
                ctx.AssociatedContext.Departments = unitRepository.AllNoTracking<Sys_Org_Department>(s => !s.IsDelete).ToList();
            }

            // 是否有类别字段
            var groupKeys = ctx.GenerateContext.GenerateNameFields
                .Where(s => s.FieldType.ToEnum<FieldType>() == FieldType.GroupType &&
                s.ExtendConfig != null && !string.IsNullOrWhiteSpace(s.ExtendConfig.GroupKey))
                .Select(s=> s.ExtendConfig.GroupKey)
                .Distinct()
                .ToList();
            if (!groupKeys.IsNullOrEmpty())
            {
                ctx.AssociatedContext.GroupTypes = unitRepository.AllNoTracking<App_Config_GroupType>(s => groupKeys.Contains(s.GroupKey)).ToList();
            }

            return true;
        }
    }
}
