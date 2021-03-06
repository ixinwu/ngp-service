﻿/* ------------------------------------------------------------------------------
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * QueryResolveSingleExcuteStep Description:
 * 解析单条查询步骤
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
    /// 解析单条查询步骤
    /// </summary>
    public class QueryResolveSingleExcuteStep : StepBase<QueryResolveContext>
    {
        /// <summary>
        /// 执行上下文
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public override bool Process(QueryResolveContext ctx)
        {
            var unitRepository = Singleton<IEngine>.Instance.Resolve<IUnitRepository>();
            ctx.Response.Data = unitRepository.QuerySingleDynamic(ctx.CommandContext.ExcuteCommand.CommandText,
                    ctx.CommandContext.ExcuteCommand.ParameterCollection);
            return true;
        }
    }
}
