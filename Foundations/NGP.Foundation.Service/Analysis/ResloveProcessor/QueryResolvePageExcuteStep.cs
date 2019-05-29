/* ------------------------------------------------------------------------------
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * QueryResolvePageExcuteStep Description:
 * 解析分页查询步骤
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
    /// 解析分页查询步骤
    /// </summary>
    public class QueryResolvePageExcuteStep : StepBase<QueryResolveContext>
    {
        /// <summary>
        /// 执行上下文
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public override bool Process(QueryResolveContext ctx)
        {
            var unitRepository = Singleton<IEngine>.Instance.Resolve<IUnitRepository>();

            // 先查询总条数
            ctx.Response.TotalCount = unitRepository.ExecuteScalar<int>(ctx.CommandContext.TotalCommand.CommandText,
                ctx.CommandContext.TotalCommand.ParameterCollection);
            if (ctx.Response.TotalCount > 0)
            {
                ctx.Response.Data = unitRepository.QueryListDynamic(ctx.CommandContext.ExcuteCommand.CommandText,
                    ctx.CommandContext.ExcuteCommand.ParameterCollection);
            }

            return true;
        }
    }
}
