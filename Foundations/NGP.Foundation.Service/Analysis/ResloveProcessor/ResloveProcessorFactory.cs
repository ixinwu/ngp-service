/* ------------------------------------------------------------------------------
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * ResloveProcessorFactory Description:
 * 解析工厂
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
    /// 解析工厂
    /// </summary>
    public static class ResloveProcessorFactory
    {
        /// <summary>
        /// 解析创建状态初始化步骤--分页查询
        /// </summary>
        public static IStep<QueryResloveContext> StepResolveQuery { get; private set; } = null;

        static ResloveProcessorFactory()
        {
            StepResolveQuery = new QueryResolveInitializeStep();
            StepResolveQuery.AddNextStep(new QueryResolvePermissionStep())
                .AddNextStep(new QueryResolveWhereStep())
                .AddNextStep(new QueryResolveOrderStep())
                .AddNextStep(new QueryResolveJoinStep())
                .AddNextStep(new QueryResolveBuildCommandStep())
                .AddNextStep(new QueryResolveBuildTypeStep())
                .AddNextStep(new QueryResolvePageExcuteStep());
        }
    }
}
