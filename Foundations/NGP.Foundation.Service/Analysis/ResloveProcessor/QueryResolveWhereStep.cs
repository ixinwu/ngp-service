/* ------------------------------------------------------------------------------
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * QueryResolveWhereStep Description:
 * 解析where条件步骤
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
    /// 解析where条件步骤
    /// </summary>
    public class QueryResolveWhereStep : StepBase<QueryResolveContext>
    {
        /// <summary>
        /// 执行上下文
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public override bool Process(QueryResolveContext ctx)
        {
            var parserCommand = Singleton<IEngine>.Instance.Resolve<ILinqParserCommand>();
            // 添加dsl
            var andDsl = new List<string>() { ctx.Request.WhereExpression };

            andDsl.Add(ctx.CommandContext.PermissionWhere);
            andDsl = andDsl.RemoveEmptyRepeat().ToList();
            var sourceDsl = parserCommand.JoinCondition(andDsl.Select(s => parserCommand.BracketCommand(s)));

            // 执行where解析
            var workContext = Singleton<IEngine>.Instance.Resolve<IWorkContext>();
            var engine = Singleton<IEngine>.Instance.Resolve<ILinqParserHandler>();
            var parserResult = engine.Resolve(new LinqParserRequest
            {
                Current = workContext.Current,
                DslContent = sourceDsl
            });

            // 设定结果
            ctx.CommandContext.WhereCommand = parserResult.Command;

            return true;
        }
    }
}
