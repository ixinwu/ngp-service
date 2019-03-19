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
    public class QueryResolveOrderStep : StepBase<QueryResloveContext>
    {
        /// <summary>
        /// 执行上下文
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public override bool Process(QueryResloveContext ctx)
        {
            ctx.CommandContext.SortCommand = ctx.Request.SortExpression;
            if (string.IsNullOrWhiteSpace(ctx.CommandContext.SortCommand))
            {
                ctx.CommandContext.SortCommand = string.Format("{0}_UpdatedTime DESC", ctx.Request.MainFormKey);
            }

            return true;
        }
    }
}
