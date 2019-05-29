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
    public class QueryResolveFindNameFieldStep : StepBase<QueryResolveContext>
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

            foreach (var field in fieldList)
            {
                // 添加名称key
                switch (field.FieldType.ToEnum<FieldType>())
                {
                    case FieldType.GroupType:
                    case FieldType.EmployeeType:
                    //case FieldType.DeptType:
                        {
                            ctx.GenerateContext.GenerateNameFields.Add(field);
                            break;
                        }
                       
                    case FieldType.FormType:
                    case FieldType.RelationType:
                    default:
                        break;
                }
            }

            return true;
        }
    }
}
