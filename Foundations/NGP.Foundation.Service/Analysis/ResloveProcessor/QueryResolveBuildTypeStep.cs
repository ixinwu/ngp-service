/* ------------------------------------------------------------------------------
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * QueryResolveBuildTypeStep Description:
 * 页面解析组装命令处理
 *
 * Comment 					        Revision	Date                  Author
 * -----------------------------    --------    ------------------    ----------------
 * Created							1.0		    2019/3/18 15:56:08    hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using NGP.Framework.Core;
using System.Collections.Generic;
using System.Linq;

namespace NGP.Foundation.Service.Analysis
{
    /// <summary>
    /// 页面解析组装命令处理
    /// </summary>
    public class QueryResolveBuildTypeStep : StepBase<QueryResolveContext>
    {
        /// <summary>
        /// 执行上下文
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public override bool Process(QueryResolveContext ctx)
        {
            // 获取字段列表
            var fieldList = ctx.InitContext.FormFields
                 .Where(s => ctx.Request.QueryFieldKeys.Contains(s.FieldKey));

            var generateList = new List<DynamicGenerateObject>();

            foreach (var field in fieldList)
            {
                // 转换字段类型
                generateList.Add(new DynamicGenerateObject
                {
                    CodeType = field.DbConfig.ColumnType.ToEnum<FieldColumnType>().GetCodeType(),
                    ObjectKey = field.FieldKey
                });

                // 添加名称key
                switch (field.FieldType.ToEnum<FieldType>())
                {
                    case FieldType.GroupType:
                    case FieldType.EmployeeType:
                    //case FieldType.DeptType:
                        {
                            if (field.DbConfig.IsMulti == true)
                            {
                                generateList.Add(new DynamicGenerateObject
                                {
                                    CodeType = typeof(List<string>),
                                    ObjectKey = AppConfigExtend.GetFieldNameKey(field.FieldKey)
                                });
                            }
                            else
                            {
                                generateList.Add(new DynamicGenerateObject
                                {
                                    CodeType = typeof(string),
                                    ObjectKey = AppConfigExtend.GetFieldNameKey(field.FieldKey)
                                });
                            }
                            ctx.GenerateContext.GenerateNameFields.Add(field);
                            break;
                        }
                       
                    case FieldType.FormType:
                    case FieldType.RelationType:
                    default:
                        break;
                }
            }

            generateList.AddRange(ctx.GenerateContext.ExtendTypes ?? new List<DynamicGenerateObject>());

            // 生成类型
            ctx.GenerateContext.GenerateType = generateList.CompileType();

            return true;
        }
    }
}
