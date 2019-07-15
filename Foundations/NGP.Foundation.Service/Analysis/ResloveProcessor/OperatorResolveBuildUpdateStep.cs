/* ------------------------------------------------------------------------------
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * OperatorResolveBuildUpdateStep Description:
 * 操作解析构建更新command步骤
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
    /// 操作解析构建更新command步骤
    /// </summary>
    public class OperatorResolveBuildUpdateStep : StepBase<OperatorResolveContext<DynamicUpdateRequest>>
    {
        /// <summary>
        /// 执行上下文
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public override bool Process(OperatorResolveContext<DynamicUpdateRequest> ctx)
        {
            var defaultFields = new List<AppDefaultFieldConfig>();

            if (ctx.InitContext.App != null
               && ctx.InitContext.App.ExtendConfig != null
               && !ctx.InitContext.App.ExtendConfig.DefaultFields.IsNullOrEmpty())
            {
                defaultFields = ctx.InitContext.App.ExtendConfig.DefaultFields
                 .Where(s => s.DefaultType.Contains(AppDefaultFieldType.Update.ToString("G")))
                 .ToList();
            }

            // 获取解析对象
            var engine = Singleton<IEngine>.Instance.Resolve<ILinqParserHandler>();
            var parserCommand = Singleton<IEngine>.Instance.Resolve<ILinqParserCommand>();
            var workContext = Singleton<IEngine>.Instance.Resolve<IWorkContext>();
            var unitRepository = Singleton<IEngine>.Instance.Resolve<IUnitRepository>();
            var commandList = new List<string>();

            // 解析where表达式
            foreach (var whereExpression in ctx.Request.WhereExpressions)
            {
                // 获取表达式的字段列表
                var whereFieldKeys = AppConfigExtend.MatchFieldKeys(whereExpression);

                // 表单key
                var formKey = AppConfigExtend.GetFormKey(whereFieldKeys.FirstOrDefault());

                // 获取form的配置
                var formItem = ctx.InitContext.Forms.FirstOrDefault(s => s.FormKey == formKey);
                if (formItem == null)
                {
                    continue;
                }

                var setList = new List<string>();

                var operatorFields = from formField in ctx.InitContext.FormFields.Where(s => s.FormKey.Equals(formKey))
                                     join operateField in ctx.Request.OperateFields.Where(s => AppConfigExtend.GetFormKey(s.FieldKey).Equals(formKey))
                                     on formField.FieldKey equals operateField.FieldKey
                                     select new
                                     {
                                         formField.FieldKey,
                                         FormField = formField,
                                         OperateField = operateField
                                     };

                // 没有操作的数据
                if (operatorFields.IsNullOrEmpty())
                {
                    continue;
                }

                // 获取验证字段
                var operatorFieldKeys = operatorFields.Select(s => s.FieldKey).ToList();
                var uniqueFields = new List<FormUniqueConfig>();
                if (formItem.ExtendConfig != null && !formItem.ExtendConfig.UniqueFields.IsNullOrEmpty())
                {
                    uniqueFields = formItem.ExtendConfig.UniqueFields.Where(s => operatorFieldKeys.Contains(s.FieldKey)).ToList();
                }

                IDictionary<string, object> dbValue = new Dictionary<string, object>();

                // 存在验证字段
                if (!uniqueFields.IsNullOrEmpty())
                {
                    // 执行解析where
                    var parserResult = engine.Resolve(new LinqParserRequest
                    {
                        Current = workContext.Current,
                        DslContent = whereExpression
                    });

                    // 查询列
                    var selectList = ctx.InitContext.FormFields
                        .Where(s => s.FormKey.Equals(formKey))
                        .Select(s => parserCommand.RenameCommand(AppConfigExtend.GetSqlFullName(s.FieldKey), s.FieldKey));

                    var whereString = parserCommand.WhereCommand(parserResult.Command.CommandText);

                    // 查询列
                    var selectString = parserCommand.JoinField(selectList);

                    var singleCommandText = parserCommand.SelectQuery(
                    string.Empty,
                    parserCommand.TopCommand(1),
                    selectString,
                    formKey,
                    string.Empty,
                    whereString,
                    string.Empty,
                    string.Empty);

                    // 执行查询
                    dbValue = unitRepository.QuerySingleDictionary(singleCommandText, parserResult.Command.ParameterCollection);
                }

                foreach (var operatorField in operatorFields)
                {
                    // 需要验证
                    var uniqueField = uniqueFields.FirstOrDefault(s => s.FieldKey == operatorField.FieldKey);
                    if (uniqueField != null)
                    {
                        // 组装验证逻辑
                        var andDsls = new List<string>();

                        // 添加当前条件的取反
                        andDsls.Add(parserCommand.NotCommand(whereExpression));

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
                        andDsls.Add(parserCommand.EqualCommand(AppConfigExtend.GenerateFieldKey(formKey, defaultField.ColumnName), 0));

                        // 约束验证
                        if (!uniqueField.ScopeFieldKeys.IsNullOrEmpty())
                        {
                            foreach (var scopeFieldKey in uniqueField.ScopeFieldKeys)
                            {
                                // 先从操作字段获取约束验证的值
                                object scopeValue = string.Empty;
                                var scopeItem = operatorFields.FirstOrDefault(s => s.FieldKey == scopeFieldKey);
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

                                // 没有就从DB获取
                                var scopeDbValue = dbValue.GetVlaue(scopeFieldKey.ToUpper());
                                if (scopeDbValue != null)
                                {
                                    if (scopeDbValue.GetType() == typeof(string))
                                    {
                                        andDsls.Add(parserCommand.EqualCommand(scopeFieldKey,
                                           parserCommand.LinqStringFormatter(Convert.ToString(scopeDbValue))));
                                        continue;
                                    }
                                    andDsls.Add(parserCommand.EqualCommand(scopeFieldKey, scopeDbValue));
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
                        var uniqueCommand = parserCommand.SelectTotalCountQuery(formKey, string.Empty, uniqueWhereCommand);
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
                        setList.Add(parserCommand.LinqSetCommand(operatorField.FieldKey, parserCommand.NullCommandKey));
                        continue;
                    }

                    // 添加设定字段
                    if (operatorField.FormField.DbConfig.ColumnType.ToEnum<FieldColumnType>() == FieldColumnType.String
                        || operatorField.FormField.DbConfig.ColumnType.ToEnum<FieldColumnType>() == FieldColumnType.Attachment)
                    {
                        setList.Add(parserCommand.LinqSetCommand(operatorField.FieldKey,
                            parserCommand.LinqStringFormatter(Convert.ToString(operatorField.OperateField.Value))));
                        continue;
                    }
                    setList.Add(parserCommand.LinqSetCommand(operatorField.FieldKey, operatorField.OperateField.Value));
                }

                foreach (var field in defaultFields)
                {
                    var fieldKey = AppConfigExtend.GenerateFieldKey(formKey, field.ColumnName);
                    var paramKey = parserCommand.ParamCommand(fieldKey);
                    switch (field.FieldType.ToEnum<FieldType>())
                    {
                        case FieldType.EmployeeType:
                            {
                                var value = parserCommand.LinqStringFormatter(workContext.Current.EmplId);
                                setList.Add(parserCommand.LinqSetCommand(fieldKey, value));
                                break;
                            }
                        case FieldType.DeptType:
                            {
                                var value = parserCommand.LinqStringFormatter(workContext.Current.DeptId);
                                setList.Add(parserCommand.LinqSetCommand(fieldKey, value));
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
                                            setList.Add(parserCommand.LinqSetCommand(fieldKey, parserCommand.LinqDateCommand));
                                            break;
                                        }
                                    case FieldColumnType.Bool:
                                    case FieldColumnType.Decimal:
                                    case FieldColumnType.String:
                                    case FieldColumnType.Integer:
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

                var setCommand = parserCommand.JoinSet(setList);
                var whereExpressionString = parserCommand.WhereCommand(whereExpression);
                var updateCommand = parserCommand.UpdateCommand(formKey, setCommand, whereExpressionString);
                commandList.Add(updateCommand);
            }

            ctx.ExcuteLinqText = parserCommand.JoinUpdate(commandList);
            return true;
        }


    }
}
