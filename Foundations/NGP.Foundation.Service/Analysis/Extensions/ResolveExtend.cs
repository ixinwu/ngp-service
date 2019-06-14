/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved.
 * 
 * ResolveExtend Description:
 * 解析扩展
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019/2/19  hulei@ixinwu.com
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
    /// 解析扩展
    /// </summary>
    public static class ResolveExtend
    {
        /// <summary>
        /// 获取代码的类型
        /// </summary>
        /// <param name="columnType">字段类型</param>
        /// <returns>代码类型</returns>
        public static Type GetCodeType(this FieldColumnType columnType)
        {
            switch (columnType)
            {
                // 日期被格式化成字符串
                case FieldColumnType.DateTime:
                case FieldColumnType.Date:
                case FieldColumnType.Time:
                    return typeof(DateTime?);
                case FieldColumnType.Integer:
                    return typeof(int?);
                case FieldColumnType.Decimal:
                    return typeof(decimal?);
                case FieldColumnType.Bool:
                    return typeof(bool?);
                case FieldColumnType.Bits:
                    return typeof(byte[]);
                case FieldColumnType.String:
                default:
                    return typeof(string);
            }
        }

        /// <summary>
        /// 获取主表单key
        /// </summary>
        /// <param name="fieldKeys"></param>
        /// <param name="relations"></param>
        /// <returns></returns>
        public static string GetMainFormKey(List<string> fieldKeys, List<App_Config_FormRelation> relations)
        {
            // 分析当前解析的主表
            var formKeys = fieldKeys.Select(s => AppConfigExtend.GetFormKey(s)).Distinct();
            var findFormKeys = formKeys.ToList();
            var mainKey = string.Empty;
            foreach (var item in formKeys)
            {
                var sourceKey = FindSourceKey(item, relations, findFormKeys);
                if (findFormKeys.Contains(sourceKey))
                {
                    mainKey = sourceKey;
                    break;
                }
            }
            return mainKey;
        }

        /// <summary>
        /// find source key
        /// </summary>
        /// <param name="mainKey"></param>
        /// <param name="relations"></param>
        /// <param name="formKeys"></param>
        /// <returns></returns>
        private static string FindSourceKey(string mainKey, List<App_Config_FormRelation> relations, List<string> formKeys)
        {
            // 取第一个源
            var sourceKey = relations
                .Where(s => s.RelationFormKey == mainKey && formKeys.Contains(s.SourceFormKey))
                .Select(s => s.SourceFormKey)
                .Distinct()
                .FirstOrDefault();
            if (string.IsNullOrWhiteSpace(sourceKey))
            {
                return mainKey;
            }
            formKeys.Remove(mainKey);
            return FindSourceKey(sourceKey, relations, formKeys);
        }

        /// <summary>
        /// 构造插入command
        /// </summary>
        /// <param name="operateFields"></param>
        /// <param name="resolveInitContext"></param>
        /// <param name="defaultFields"></param>
        /// <returns></returns>
        public static (List<string> CommandList, List<NGPKeyValuePair> PrimaryKeys, NGPResponse Response) BuildInsertCommand(
            List<DynamicOperateFieldRequest> operateFields,
            ResolveInitContext resolveInitContext,
            List<AppDefaultFieldConfig> defaultFields)
        {
            // 获取解析对象
            var engine = Singleton<IEngine>.Instance.Resolve<ILinqParserHandler>();
            var parserCommand = Singleton<IEngine>.Instance.Resolve<ILinqParserCommand>();
            var workContext = Singleton<IEngine>.Instance.Resolve<IWorkContext>();
            var unitRepository = Singleton<IEngine>.Instance.Resolve<IUnitRepository>();
            var commandList = new List<string>();
            var primaryKeys = new List<NGPKeyValuePair>();

            // 根据字段进行分组
            var operatorGroupList = (
                from field in (
                from operateField in operateFields
                join formField in resolveInitContext.FormFields
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
                var formItem = resolveInitContext.Forms.FirstOrDefault(s => s.FormKey == operatorForm.FormKey);
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
                primaryKeys.Add(new NGPKeyValuePair
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
                        var defaultField = resolveInitContext.App.ExtendConfig.DefaultFields.FirstOrDefault(s =>
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
                            return (null, null, new NGPResponse
                            {
                                AffectedRows = 0,
                                Message = string.Format(CommonResource.Exist, operatorField.FormField.FieldName),
                                Status = OperateStatus.Error
                            });
                        }
                    }

                    // 如果值为null
                    if (string.IsNullOrWhiteSpace(Convert.ToString(operatorField.OperateField.Value)))
                    {
                        // 不为空验证
                        if (operatorField.FormField.ExtendConfig.IsRequired == true)
                        {
                            return (null, null, new NGPResponse
                            {
                                AffectedRows = 0,
                                Message = string.Format(CommonResource.NotEmpty, operatorField.FormField.FieldName),
                                Status = OperateStatus.Error
                            });
                        }
                        insertElementList.Add(new NGPKeyValuePair<object>
                        {
                            Key = operatorField.FieldKey,
                            Value = parserCommand.NullCommandKey
                        });
                    }

                    // 添加操作值
                    if (operatorField.FormField.DbConfig.ColumnType.ToEnum<FieldColumnType>() == FieldColumnType.String ||
                        operatorField.FormField.DbConfig.ColumnType.ToEnum<FieldColumnType>() == FieldColumnType.Attachment)
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
            return (commandList, primaryKeys, null);
        }
    }
}
