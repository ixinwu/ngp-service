/* ------------------------------------------------------------------------------
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * QueryResolveJoinStep Description:
 * 解析关联步骤
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
    /// 解析关联步骤
    /// </summary>
    public class QueryResolveJoinStep : StepBase<QueryResloveContext>
    {
        /// <summary>
        /// 执行上下文
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public override bool Process(QueryResloveContext ctx)
        {
            // 获取关联列表
            var relationList = ctx.InitContext.FormRelations.Where(s => s.SourceFormKey == ctx.Request.MainFormKey);

            var joinList = new List<string>();

            // 循环关联列表
            foreach (var relation in relationList)
            {
                var equals = string.Format("{0} = {1}",
                    AppConfigExtend.GetSqlFullName(relation.SourceFieldKey),
                    AppConfigExtend.GetSqlFullName(relation.RelationFieldKey));
                var command = string.Format("LEFT JOIN {0} ON {1}",
                    relation.RelationFormKey,
                    equals);
                joinList.Add(command);
            }

            if (joinList.Count > 0)
            {
                ctx.CommandContext.JoinCommand = string.Join(" \r\n ", joinList.Select(s => s).RemoveEmptyRepeat());
            }

            return true;
        }
    }
}
