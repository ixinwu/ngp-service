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
    public class QueryResolveJoinStep : StepBase<QueryResolveContext>
    {
        /// <summary>
        /// 执行上下文
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public override bool Process(QueryResolveContext ctx)
        {
            var parserCommand = Singleton<IEngine>.Instance.Resolve<ILinqParserCommand>();

            var allFormKeys = new List<string>();
            var whereFieldKeys = AppConfigExtend.MatchFieldKeys(ctx.Request.WhereExpression);
            allFormKeys.AddRange(whereFieldKeys.Select(s => AppConfigExtend.GetFormKey(s)).Distinct());
            allFormKeys.AddRange(ctx.Request.QueryFieldKeys.Select(s => AppConfigExtend.GetFormKey(s)).Distinct());

            // 获取关联列表
            var relationList = ctx.InitContext.FormRelations.Where(s => s.SourceFormKey == ctx.MainFormKey);

            var joinList = new List<string>();

            // 循环关联列表
            foreach (var relation in relationList)
            {
                var equals = parserCommand.EqualCommand(
                    AppConfigExtend.GetSqlFullName(relation.SourceFieldKey),
                    AppConfigExtend.GetSqlFullName(relation.RelationFieldKey));
                var command = parserCommand.LeftJoinCommand(relation.RelationFormKey, equals);
                joinList.Add(command);
            }

            // 循环额外关联列表
            var extendRelationList = ctx.InitContext.FormRelations.Where(s =>
                allFormKeys.Contains(s.SourceFormKey) && allFormKeys.Contains(s.RelationFormKey) &&
                !relationList.Contains(s));
            foreach (var relation in extendRelationList)
            {
                var equals = parserCommand.EqualCommand(
                    AppConfigExtend.GetSqlFullName(relation.RelationFieldKey),
                    AppConfigExtend.GetSqlFullName(relation.SourceFieldKey));
                var command = parserCommand.LeftJoinCommand(relation.SourceFormKey, equals);
                joinList.Add(command);
            }

            if (joinList.Count > 0)
            {
                ctx.CommandContext.JoinCommand = parserCommand.JoinLine(joinList.Select(s => s).RemoveEmptyRepeat());
            }

            return true;
        }
    }
}
