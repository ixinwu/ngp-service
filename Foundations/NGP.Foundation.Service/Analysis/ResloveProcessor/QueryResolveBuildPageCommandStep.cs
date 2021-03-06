﻿/* ------------------------------------------------------------------------------
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * QueryResolveBuildCommandStep Description:
 * 页面解析组装命令处理
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
    /// 页面解析组装命令处理
    /// </summary>
    public class QueryResolveBuildPageCommandStep : StepBase<QueryResolveContext>
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

            // 格式化分页内容
            ctx.PageQueryRequest.ResetPageQuery();

            // 设定where
            var whereString = string.Empty;
            if (!string.IsNullOrWhiteSpace(ctx.CommandContext.WhereCommand.CommandText))
            {
                whereString = parserCommand.WhereCommand(ctx.CommandContext.WhereCommand.CommandText);
            }

            // 起始页
            var startIndex = (ctx.PageQueryRequest.PageNumber - 1) * ctx.PageQueryRequest.PageSize;

            // 结束页
            var endIndex = ctx.PageQueryRequest.PageNumber * ctx.PageQueryRequest.PageSize;

            // 查询列
            var selectList = ctx.Request.QueryFieldKeys.Select(s => parserCommand.RenameCommand(AppConfigExtend.GetSqlFullName(s), s));

            // 查询列
            var selectString = parserCommand.JoinField(selectList);

            // 总条数命令
            var totalCommand = parserCommand.SelectTotalCountQuery(ctx.MainFormKey,
                ctx.CommandContext.JoinCommand,
                whereString);

            ctx.CommandContext.TotalCommand = new ExcuteSqlCommand(totalCommand);
            foreach (var param in ctx.CommandContext.WhereCommand.ParameterCollection)
            {
                ctx.CommandContext.TotalCommand.ParameterCollection[param.Key] = param.Value;
            }

            // 分页命令
            var pageCommand = parserCommand.SelectPageQuery(selectString,
                ctx.MainFormKey,
                ctx.CommandContext.JoinCommand,
                whereString,
                string.Empty,
                ctx.CommandContext.SortCommand,
                startIndex,
                endIndex);

            ctx.CommandContext.ExcuteCommand = new ExcuteSqlCommand(pageCommand);
            foreach (var param in ctx.CommandContext.WhereCommand.ParameterCollection)
            {
                ctx.CommandContext.ExcuteCommand.ParameterCollection[param.Key] = param.Value;
            }
            return true;
        }
    }
}
