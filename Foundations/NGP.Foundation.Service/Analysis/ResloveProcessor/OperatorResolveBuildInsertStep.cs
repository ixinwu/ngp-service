/* ------------------------------------------------------------------------------
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * OperatorResolveBuildAddCommandStep Description:
 * 操作解析构建追加command步骤
 *
 * Comment 					        Revision	Date                  Author
 * -----------------------------    --------    ------------------    ----------------
 * Created							1.0		    2019/3/18 15:56:08    hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using NGP.Foundation.Resources;
using NGP.Framework.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NGP.Foundation.Service.Analysis
{
    /// <summary>
    /// 操作解析构建追加command步骤
    /// </summary>
    public class OperatorResolveBuildInsertStep : StepBase<OperatorResolveContext<DynamicInsertRequest>>
    {
        /// <summary>
        /// 执行上下文
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public override bool Process(OperatorResolveContext<DynamicInsertRequest> ctx)
        {
            var defaultFields = new List<AppDefaultFieldConfig>();

            if (ctx.InitContext.App != null
               && ctx.InitContext.App.ExtendConfig != null
               && !ctx.InitContext.App.ExtendConfig.DefaultFields.IsNullOrEmpty())
            {
                defaultFields = ctx.InitContext.App.ExtendConfig.DefaultFields
                 .Where(s => s.DefaultType.Contains(AppDefaultFieldType.Insert.ToString("G")))
                 .ToList();
            }

            // 获取解析对象
            var engine = Singleton<IEngine>.Instance.Resolve<ILinqParserHandler>();
            var parserCommand = Singleton<IEngine>.Instance.Resolve<ILinqParserCommand>();
            var workContext = Singleton<IEngine>.Instance.Resolve<IWorkContext>();
            var unitRepository = Singleton<IEngine>.Instance.Resolve<IUnitRepository>();
            var commandList = new List<string>();

            // 根据字段进行分组
            var operatorGroupList = (
                from field in (
                from operateField in ctx.Request.OperateFields
                join formField in ctx.InitContext.FormFields
                on operateField.FieldKey equals formField.FieldKey
                select new
                {
                    formField.FieldKey,
                    FormField = formField,
                    OperateField = operateField
                })
                group field by AppConfigExtend.GetFormKey(field.FieldKey)
                into g
                select new
                {
                    FormKey = g.Key,
                    FieldList = g.ToList()
                }).ToList();


            foreach (var operatorForm in operatorGroupList)
            {
                // 获取form的配置
                var formItem = ctx.InitContext.Forms.FirstOrDefault(s => s.FormKey == operatorForm.FormKey);
                if (formItem == null)
                {
                    continue;
                }
                // 获取验证字段
                var operatorFieldKeys = operatorForm.FieldList.Select(s => s.FieldKey).ToList();
                var uniqueFields = new List<FormUniqueConfig>();
                if (formItem.ExtendConfig != null && !formItem.ExtendConfig.UniqueFields.IsNullOrEmpty())
                {
                    uniqueFields = formItem.ExtendConfig.UniqueFields.Where(s => operatorFieldKeys.Contains(s.FieldKey)).ToList();
                }

                var insertElementList = new List<NGPKeyValuePair<object>>();

                // 添加主键
                var primaryKey = AppConfigExtend.GetFormPrimaryKey(operatorForm.FormKey);
                var primaryKeyValue = GuidExtend.NewGuid();
                ctx.InsertPrimaryKeys.Add(new NGPKeyValuePair
                {
                    Key = primaryKey,
                    Value = primaryKeyValue
                });
                insertElementList.Add(new NGPKeyValuePair<object>
                {
                    Key = primaryKey,
                    Value = parserCommand.LinqStringFormatter(primaryKeyValue)
                });

                foreach (var operatorField in operatorForm.FieldList)
                {
                    // 需要验证
                    var uniqueField = uniqueFields.FirstOrDefault(s => s.FieldKey == operatorField.FieldKey);
                    if (uniqueField != null)
                    {
                        // 组装验证逻辑
                        var andDsls = new List<string>();

                        // 添加当前字段的值
                        var currentUniqueDsl = parserCommand.EqualCommand(operatorField.FieldKey, operatorField.OperateField.Value);
                        if (operatorField.FormField.DbConfig.ColumnType.ToEnum<FieldColumnType>() == FieldColumnType.String)
                        {
                            currentUniqueDsl = parserCommand.EqualCommand(operatorField.FieldKey,
                                parserCommand.LinqStringFormatter(Convert.ToString(operatorField.OperateField.Value)));
                        }
                        andDsls.Add(currentUniqueDsl);

                        // 获取应用配置里是删除标记的字段
                        var defaultField = ctx.InitContext.App.ExtendConfig.DefaultFields.FirstOrDefault(s =>
                        s.DefaultType.Contains(AppDefaultFieldType.Delete.ToString("G")) &&
                        s.ColumnType.ToEnum<FieldColumnType>() == FieldColumnType.Bool);
                        andDsls.Add(parserCommand.EqualCommand(AppConfigExtend.GenerateFieldKey(operatorForm.FormKey, defaultField.ColumnName), 0));

                        // 约束验证
                        if (!uniqueField.ScopeFieldKeys.IsNullOrEmpty())
                        {
                            foreach (var scopeFieldKey in uniqueField.ScopeFieldKeys)
                            {
                                // 先从操作字段获取约束验证的值
                                object scopeValue = string.Empty;
                                var scopeItem = operatorForm.FieldList.FirstOrDefault(s => s.FieldKey == scopeFieldKey);
                                if (scopeItem != null && scopeItem.OperateField.Value != null)
                                {
                                    if (scopeItem.OperateField.Value.GetType() == typeof(string))
                                    {
                                        andDsls.Add(parserCommand.EqualCommand(scopeItem.FieldKey,
                                           parserCommand.LinqStringFormatter(Convert.ToString(scopeItem.OperateField.Value))));
                                        continue;
                                    }
                                    andDsls.Add(parserCommand.EqualCommand(scopeItem.FieldKey, scopeItem.OperateField.Value));
                                    continue;
                                }
                            }
                        }

                        // 验证where
                        var uniqueWhere = parserCommand.JoinCondition(andDsls);

                        var uniqueParserResult = engine.Resolve(new LinqParserRequest
                        {
                            Current = workContext.Current,
                            DslContent = uniqueWhere
                        });
                        var uniqueWhereCommand = parserCommand.WhereCommand(uniqueParserResult.Command.CommandText);
                        var uniqueCommand = parserCommand.SelectTotalCountQuery(operatorForm.FormKey, string.Empty, uniqueWhereCommand);
                        var count = unitRepository.ExecuteScalar<int>(uniqueCommand, uniqueParserResult.Command.ParameterCollection);
                        if (count > 0)
                        {
                            ctx.Response = new NGPResponse
                            {
                                AffectedRows = 0,
                                Message = string.Format(CommonResource.Exist, operatorField.FormField.FieldName),
                                Status = OperateStatus.Error
                            };
                            return false;
                        }
                    }

                    // 如果值为null
                    if (string.IsNullOrWhiteSpace(Convert.ToString(operatorField.OperateField.Value)))
                    {
                        // 不为空验证
                        if (operatorField.FormField.ExtendConfig.IsRequired == true)
                        {
                            ctx.Response = new NGPResponse
                            {
                                AffectedRows = 0,
                                Message = string.Format(CommonResource.NotEmpty, operatorField.FormField.FieldName),
                                Status = OperateStatus.Error
                            };
                            return false;
                        }
                        insertElementList.Add(new NGPKeyValuePair<object>
                        {
                            Key = operatorField.FieldKey,
                            Value = parserCommand.NullCommandKey
                        });
                    }

                    // 添加操作值
                    if (operatorField.FormField.DbConfig.ColumnType.ToEnum<FieldColumnType>() == FieldColumnType.String)
                    {
                        insertElementList.Add(new NGPKeyValuePair<object>
                        {
                            Key = operatorField.FieldKey,
                            Value = parserCommand.LinqStringFormatter(Convert.ToString(operatorField.OperateField.Value))
                        });
                        continue;
                    }
                    insertElementList.Add(new NGPKeyValuePair<object>
                    {
                        Key = operatorField.FieldKey,
                        Value = operatorField.OperateField.Value
                    });
                }

                foreach (var field in defaultFields)
                {
                    var fieldKey = AppConfigExtend.GenerateFieldKey(operatorForm.FormKey, field.ColumnName);
                    var paramKey = parserCommand.ParamCommand(fieldKey);
                    switch (field.FieldType.ToEnum<FieldType>())
                    {
                        case FieldType.EmployeeType:
                            {
                                var value = parserCommand.LinqStringFormatter(workContext.Current.EmplId);
                                insertElementList.Add(new NGPKeyValuePair<object>
                                {
                                    Key = fieldKey,
                                    Value = value
                                });
                                break;
                            }
                        case FieldType.DeptType:
                            {
                                var value = parserCommand.LinqStringFormatter(workContext.Current.DeptId);
                                insertElementList.Add(new NGPKeyValuePair<object>
                                {
                                    Key = fieldKey,
                                    Value = value
                                });
                                break;
                            }
                        case FieldType.FormType:
                            {
                                switch (field.ColumnType.ToEnum<FieldColumnType>())
                                {
                                    case FieldColumnType.Time:
                                    case FieldColumnType.Date:
                                    case FieldColumnType.DateTime:
                                        {
                                            insertElementList.Add(new NGPKeyValuePair<object>
                                            {
                                                Key = fieldKey,
                                                Value = parserCommand.LinqDateCommand
                                            });
                                            break;
                                        }
                                    case FieldColumnType.Bool:
                                        {
                                            insertElementList.Add(new NGPKeyValuePair<object>
                                            {
                                                Key = fieldKey,
                                                Value = 0
                                            });
                                            break;
                                        }
                                    case FieldColumnType.Decimal:
                                    case FieldColumnType.Integer:
                                        insertElementList.Add(new NGPKeyValuePair<object>
                                        {
                                            Key = fieldKey,
                                            Value = 0
                                        });
                                        break;
                                    case FieldColumnType.String:
                                    case FieldColumnType.Bits:
                                    default:
                                        break;
                                }
                                break;
                            }
                        case FieldType.GroupType:
                        case FieldType.RelationType:
                        default:
                            break;
                    }
                }

                var selectElementCommand = parserCommand.JoinSelect(insertElementList.Select(s => s.Key));
                var paramElementCommand = parserCommand.JoinParam(insertElementList.Select(s => s.Value));

                var insertCommand = parserCommand.LinqInsertCommand(operatorForm.FormKey, selectElementCommand, paramElementCommand);
                commandList.Add(insertCommand);
            }
            ctx.ExcuteLinqText = parserCommand.JoinInsert(commandList);
            return true;

        }
    }
}
