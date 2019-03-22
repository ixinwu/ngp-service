/* ------------------------------------------------------------------------------
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * OperatorResolveBuildDeleteStep Description:
 * 操作解析构建删除command步骤
 *
 * Comment 					        Revision	Date                  Author
 * -----------------------------    --------    ------------------    ----------------
 * Created							1.0		    2019/3/18 15:56:08    hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using NGP.Framework.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NGP.Foundation.Service.Analysis
{
    /// <summary>
    /// 操作解析构建删除command步骤
    /// </summary>
    public class OperatorResolveBuildDeleteStep : StepBase<OperatorResolveContext<DynamicDeleteRequest>>
    {
        /// <summary>
        /// 执行上下文
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public override bool Process(OperatorResolveContext<DynamicDeleteRequest> ctx)
        {
            if (ctx.InitContext.App == null
                || ctx.InitContext.App.ExtendConfig == null
                || ctx.InitContext.App.ExtendConfig.DefaultFields.IsNullOrEmpty())
            {
                return false;
            }

            var defaultFields = ctx.InitContext.App.ExtendConfig.DefaultFields
                .Where(s => s.DefaultType.Contains(AppDefaultFieldType.Delete.ToString("G")))
                .ToList();
            if (defaultFields.IsNullOrEmpty())
            {
                return false;
            }

            // 获取解析对象
            var parserCommand = Singleton<IEngine>.Instance.Resolve<ILinqParserCommand>();
            var workContext = Singleton<IEngine>.Instance.Resolve<IWorkContext>();
            var commandList = new List<string>();

            // 解析where表达式
            foreach (var whereExpression in ctx.Request.WhereExpressions)
            {
                // 获取表达式的字段列表
                var whereFieldKeys = AppConfigExtend.MatchFieldKeys(whereExpression);

                // 表单key
                var formKey = AppConfigExtend.GetFormKey(whereFieldKeys.FirstOrDefault());
                var setList = new List<string>();
                foreach (var field in defaultFields)
                {
                    var fieldKey = AppConfigExtend.GenerateFieldKey(formKey, field.ColumnName);
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

                                    case FieldColumnType.Bool:
                                        {
                                            setList.Add(parserCommand.LinqSetCommand(fieldKey, 1));
                                            break;
                                        }
                                    case FieldColumnType.Time:
                                    case FieldColumnType.Date:
                                    case FieldColumnType.DateTime:
                                        {
                                            setList.Add(parserCommand.LinqSetCommand(fieldKey, parserCommand.DateCommand));
                                            break;
                                        }
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
                var whereCommand = parserCommand.WhereCommand(whereExpression);
                var updateCommand = parserCommand.UpdateCommand(formKey, setCommand, whereCommand);
                commandList.Add(updateCommand);
            }

            ctx.ExcuteLinqText = parserCommand.JoinUpdate(commandList);
            return true;
        }


    }
}
