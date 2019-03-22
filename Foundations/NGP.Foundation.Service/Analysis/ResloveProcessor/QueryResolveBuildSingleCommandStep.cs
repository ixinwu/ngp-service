/* ------------------------------------------------------------------------------
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * QueryResolveBuildSingleCommandStep Description:
 * 页面解析组装单条命令处理
 *
 * Comment 					        Revision	Date                  Author
 * -----------------------------    --------    ------------------    ----------------
 * Created							1.0		    2019/3/18 15:56:08    hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using NGP.Framework.Core;
using System.Linq;

namespace NGP.Foundation.Service.Analysis
{
    /// <summary>
    /// 页面解析组装单条命令处理
    /// </summary>
    public class QueryResolveBuildSingleCommandStep : StepBase<QueryResolveContext>
    {
        /// <summary>
        /// 执行上下文
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public override bool Process(QueryResolveContext ctx)
        {
            // 格式命令接口
            var parserCommand = Singleton<IEngine>.Instance.Resolve<ILinqParserCommand>();

            // 设定where
            var whereString = string.Empty;
            if (!string.IsNullOrWhiteSpace(ctx.CommandContext.WhereCommand.CommandText))
            {
                whereString = parserCommand.WhereCommand(ctx.CommandContext.WhereCommand.CommandText);
            }

            // 查询列
            var selectList = ctx.Request.QueryFieldKeys.Select(s => parserCommand.RenameCommand(AppConfigExtend.GetSqlFullName(s), s));

            // 查询列
            var selectString = parserCommand.JoinField(selectList);

            // 查询命令
            var singleCommand = parserCommand.SelectQuery(
                string.Empty,
                parserCommand.TopCommand(1),
                selectString,
                ctx.InitContext.MainFormKey,
                ctx.CommandContext.JoinCommand,
                whereString,
                ctx.CommandContext.SortCommand,
                string.Empty);

            ctx.CommandContext.ExcuteCommand = new ExcuteSqlCommand(singleCommand);
            foreach (var param in ctx.CommandContext.WhereCommand.ParameterCollection)
            {
                ctx.CommandContext.ExcuteCommand.ParameterCollection[param.Key] = param.Value;
            }
            return true;
        }
    }
}
