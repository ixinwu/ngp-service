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

namespace NGP.Foundation.Service.Analysis
{
    /// <summary>
    /// 页面解析初始化处理
    /// </summary>
    public class QueryResolveInitializeStep : StepBase<QueryResloveContext>
    {
        /// <summary>
        /// 执行上下文
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public override bool Process(QueryResloveContext ctx)
        {
            // 配置信息读取服务
            var dataProvider = Singleton<IEngine>.Instance.Resolve<IResloveDataProvider>();

            // 处理参数上下文
            ctx.InitContext = dataProvider.InitResloveContext(ctx.Request);

            return true;
        }
    }
}
