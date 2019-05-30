/* ------------------------------------------------------------------------------
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * ResolveInitializeStep Description:
 * 解析初始化步骤
 *
 * Comment 					        Revision	Date                  Author
 * -----------------------------    --------    ------------------    ----------------
 * Created							1.0		    2019/3/18 15:56:08    hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using NGP.Framework.Core;
using System.Collections.Generic;

namespace NGP.Foundation.Service.Analysis
{
    /// <summary>
    /// 页面解析初始化处理
    /// </summary>
    public class QueryResolveInitializeStep : StepBase<QueryResolveContext>
    {
        /// <summary>
        /// 执行上下文
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public override bool Process(QueryResolveContext ctx)
        {
            // 配置信息读取服务
            var dataProvider = Singleton<IEngine>.Instance.Resolve<IResolveDataProvider>();

            // 处理参数上下文
            ctx.InitContext = dataProvider.InitResolveContext(ctx.Request);

            ctx.MainFormKey = ResolveExtend.GetMainFormKey(ctx.Request.QueryFieldKeys,
                ctx.InitContext.FormRelations ?? new List<App_Config_FormRelation>());
            return true;
        }


    }
}
