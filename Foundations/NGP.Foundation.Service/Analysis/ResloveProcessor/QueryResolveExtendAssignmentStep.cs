/* ------------------------------------------------------------------------------
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * QueryResolveExtendAssignmentStep Description:
 * 解析扩展赋值步骤
 *
 * Comment 					        Revision	Date                  Author
 * -----------------------------    --------    ------------------    ----------------
 * Created							1.0		    2019/3/18 15:56:08    hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using NGP.Framework.Core;
using System;
using System.Collections;
using System.Linq;

namespace NGP.Foundation.Service.Analysis
{
    /// <summary>
    /// 解析扩展赋值步骤
    /// </summary>
    public class QueryResolveExtendAssignmentStep : StepBase<QueryResolveContext>
    {
        /// <summary>
        /// 执行上下文
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public override bool Process(QueryResolveContext ctx)
        {
            if (ctx.Response.Data == null)
            {
                return false;
            }

            // 没有数据需要扩展赋值
            if (ctx.GenerateContext.GenerateNameFields.IsNullOrEmpty())
            {
                return true;
            }

            // 扩展设定字段列表
            var getSetFields = ctx.GenerateContext.GenerateNameFields.Select(s => new
            {
                GetProperty = ctx.GenerateContext.GenerateType.GetProperty(s.FieldKey),
                SetProperty = ctx.GenerateContext.GenerateType.GetProperty(AppConfigExtend.GetFieldNameKey(s.FieldKey)),
                s.FieldKey,
                FieldType = s.FieldType.ToEnum<FieldType>()
            });

            // 设定名称处理
            Action<dynamic> setNameAction = (item) =>
            {
                // 单条数据赋值
                foreach (var field in getSetFields)
                {
                    // 获取key值
                    var key = field.GetProperty.GetValue(item);
                    var value = string.Empty;

                    // 根据类型筛选
                    switch (field.FieldType)
                    {
                        case FieldType.GroupType:
                            {
                                value = (ctx.AssociatedContext.GroupTypes.FirstOrDefault(s => s.TypeKey == key) ?? new App_Config_GroupType()).TypeValue;
                                break;
                            }
                        case FieldType.EmployeeType:
                            {
                                value = (ctx.AssociatedContext.Employees.FirstOrDefault(s => s.Id == key) ?? new Sys_Org_Employee()).EmplName;
                                break;
                            }
                        case FieldType.DeptType:
                            {
                                value = (ctx.AssociatedContext.Departments.FirstOrDefault(s => s.Id == key) ?? new Sys_Org_Department()).DeptShortName;
                                break;
                            }
                        case FieldType.FormType:
                        case FieldType.RelationType:
                        default:
                            break;
                    }
                    // 设定name值
                    field.SetProperty.SetValue(item, value, new object[0]);
                }
            };

            // 如果是列表
            if (ctx.Response.Data is IList && ctx.Response.Data.GetType().IsGenericType && ctx.Response.Data.Count > 0)
            {
                foreach (var item in ctx.Response.Data)
                {
                    setNameAction(item);
                }
                return true;
            }

            // 如果是单条
            setNameAction(ctx.Response.Data);

            return true;
        }
    }
}
