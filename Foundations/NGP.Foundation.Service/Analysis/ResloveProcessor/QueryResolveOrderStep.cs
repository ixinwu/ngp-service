/* ------------------------------------------------------------------------------
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * QueryResolveOrderStep  Description:
 * 解析排序步骤
 *
 * Comment 					        Revision	Date                  Author
 * -----------------------------    --------    ------------------    ----------------
 * Created							1.0		    2019/3/18 15:56:08    hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using NGP.Framework.Core;

namespace NGP.Foundation.Service.Analysis
{
    /// <summary>
    /// 解析排序步骤
    /// </summary>
    public class QueryResolveOrderStep : StepBase<QueryResolveContext>
    {
        /// <summary>
        /// 执行上下文
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public override bool Process(QueryResolveContext ctx)
        {
            var parserCommand = Singleton<IEngine>.Instance.Resolve<ILinqParserCommand>();
            var sortExpression = ctx.Request.SortExpression;
            if (string.IsNullOrWhiteSpace(sortExpression))
            {
                sortExpression = string.Format("{0}_UpdatedTime DESC", ctx.InitContext.MainFormKey);
            }

            // 执行order解析
            var workContext = Singleton<IEngine>.Instance.Resolve<IWorkContext>();
            var engine = Singleton<IEngine>.Instance.Resolve<ILinqParserHandler>();
            var parserResult = engine.Resolve(new LinqParserRequest
            {
                Current = workContext.Current,
                DslContent = parserCommand.OrderCommand(sortExpression)
            });

            // 设定结果
            ctx.CommandContext.SortCommand = parserResult.Command.CommandText;

            return true;
        }
    }
}
