/* ------------------------------------------------------------------------------
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * OperatorResolveInitializeStep Description:
 * 操作解析初始化处理
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
    /// 操作解析初始化处理
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    public class OperatorResolveInitializeStep<TRequest> : StepBase<OperatorResolveContext<TRequest>> where TRequest : DynamicBaseRequest
    {
        /// <summary>
        /// 执行上下文
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public override bool Process(OperatorResolveContext<TRequest> ctx)
        {
            // 配置信息读取服务
            var dataProvider = Singleton<IEngine>.Instance.Resolve<IResolveDataProvider>();

            // 处理参数上下文
            ctx.InitContext = dataProvider.InitResolveContext(ctx.Request);

            if (ctx.InitContext.App == null
                || ctx.InitContext.App.ExtendConfig == null)
            {
                return false;
            }

            return true;
        }


    }
}
