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

using Newtonsoft.Json;
using NGP.Framework.Core;
using System;
using System.Collections;
using System.Collections.Generic;
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
            if (ctx.GenerateContext.GenerateNameFields.IsNullOrEmpty() && ctx.GenerateContext.AttachmentFields.IsNullOrEmpty())
            {
                return true;
            }

            // 扩展设定字段列表
            var getSetFields = ctx.GenerateContext.GenerateNameFields.Select(s => new
            {
                GetPropertyName = s.FieldKey,
                SetPropertyName = AppConfigExtend.GetFieldNameKey(s.FieldKey),
                s.FieldKey,
                s.DbConfig.IsMulti,
                FieldType = s.FieldType.ToEnum<FieldType>(),
                FieldColumnType  = s.DbConfig.ColumnType.ToEnum<FieldColumnType>()
            });

            // 设定名称处理
            Action<dynamic> setNameAction = (item) =>
            {
                var dicItem = item as IDictionary<string, object>;
                // 单条数据赋值
                foreach (var field in getSetFields)
                {
                    // 获取key值
                    dynamic getValue = dicItem[field.GetPropertyName];
                    var value = string.Empty;

                    // 根据类型筛选
                    switch (field.FieldType)
                    {
                        case FieldType.GroupType:
                            {
                                App_Config_GroupType groupItem = null;
                                // 多选的场景
                                if (field.IsMulti == true)
                                {
                                    var listValue = new List<string>();
                                    var getValues = getValue.Split(',');
                                    foreach (var keyItem in getValues)
                                    {
                                        groupItem = ctx.AssociatedContext.GroupTypes.FirstOrDefault(s => s.TypeKey == keyItem);
                                        if (groupItem != null)
                                        {
                                            listValue.Add(groupItem.TypeValue);
                                        }
                                    }
                                    // 设定name值
                                    dicItem[field.SetPropertyName] = listValue;
                                    break;
                                }
                                groupItem = ctx.AssociatedContext.GroupTypes.FirstOrDefault(s => s.TypeKey == getValue);
                                if (groupItem != null)
                                {
                                    value = groupItem.TypeValue;
                                }
                                // 设定name值
                                dicItem[field.SetPropertyName] = value;
                                break;
                            }
                        case FieldType.EmployeeType:
                            {
                                Sys_Org_Employee accountItem = null;
                                // 多选的场景
                                if (field.IsMulti == true)
                                {
                                    var listValue = new List<string>();
                                    var keys = getValue.Split(',');
                                    foreach (var keyItem in keys)
                                    {
                                        accountItem = ctx.AssociatedContext.Employees.FirstOrDefault(s => s.Id == keyItem);
                                        if (accountItem != null)
                                        {
                                            listValue.Add(accountItem.EmplName);
                                        }
                                    }
                                    // 设定name值
                                    dicItem[field.SetPropertyName] = listValue;
                                    break;
                                }
                                accountItem = ctx.AssociatedContext.Employees.FirstOrDefault(s => s.Id == getValue);
                                if (accountItem != null)
                                {
                                    value = accountItem.EmplName;
                                }
                                // 设定name值
                                dicItem[field.SetPropertyName] = value;
                                break;
                            }
                        case FieldType.DeptType:
                            {
                                Sys_Org_Department deptItem = null;
                                // 多选的场景
                                if (field.IsMulti == true)
                                {
                                    var listValue = new List<string>();
                                    var keys = getValue.Split(',');
                                    foreach (var keyItem in keys)
                                    {
                                        deptItem = ctx.AssociatedContext.Departments.FirstOrDefault(s => s.Id == keyItem);
                                        if (deptItem != null)
                                        {
                                            listValue.Add(deptItem.DeptName);
                                        }
                                    }
                                    // 设定name值
                                    dicItem[field.SetPropertyName] = listValue;
                                    break;
                                }
                                deptItem = ctx.AssociatedContext.Departments.FirstOrDefault(s => s.Id == getValue);
                                if (deptItem != null)
                                {
                                    value = deptItem.DeptName;
                                }
                                // 设定name值
                                dicItem[field.SetPropertyName] = value;
                                break;
                            }
                        case FieldType.FormType:
                            // 如果是附件
                            if (field.FieldColumnType == FieldColumnType.Attachment)
                            {
                                dicItem[field.SetPropertyName] = NGPFileExtensions.FileUrl(GlobalConst.__AttachmentFilesPath, getValue);
                            }
                            break;
                        case FieldType.RelationType:
                        default:
                            break;
                    }
                }
            };

            // 如果是列表
            if (ctx.Response.Data is IList && ctx.Response.Data.GetType().IsGenericType)
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
