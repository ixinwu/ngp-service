/* ------------------------------------------------------------------------------
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * ResolveProcessorFactory Description:
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
    public static class ResolveProcessorFactory
    {
        /// <summary>
        /// 解析创建状态初始化步骤--分页查询
        /// </summary>
        public static IStep<QueryResolveContext> StepResolveQuery { get; private set; } = new QueryResolveInitializeStep();

        /// <summary>
        /// 解析创建状态初始化步骤--单条查询
        /// </summary>
        public static IStep<QueryResolveContext> StepResolveSingleQuery { get; private set; }= new QueryResolveInitializeStep();

        /// <summary>
        /// 解析创建状态初始化步骤--全部查询
        /// </summary>
        public static IStep<QueryResolveContext> StepResolveAllQuery { get; private set; } = new QueryResolveInitializeStep();

        /// <summary>
        /// ctor
        /// </summary>
        static ResolveProcessorFactory()
        {
            StepResolveQuery.AddNextStep(new QueryResolvePermissionStep())
                .AddNextStep(new QueryResolveWhereStep())
                .AddNextStep(new QueryResolveOrderStep())
                .AddNextStep(new QueryResolveJoinStep())
                .AddNextStep(new QueryResolveBuildPageCommandStep())
                .AddNextStep(new QueryResolveBuildTypeStep())
                .AddNextStep(new QueryResolveAssociatedQueryStep())
                .AddNextStep(new QueryResolvePageExcuteStep())
                .AddNextStep(new QueryResolveExtendAssignmentStep());

            StepResolveSingleQuery.AddNextStep(new QueryResolvePermissionStep())
               .AddNextStep(new QueryResolveWhereStep())
               .AddNextStep(new QueryResolveOrderStep())
               .AddNextStep(new QueryResolveJoinStep())
               .AddNextStep(new QueryResolveBuildSingleCommandStep())
               .AddNextStep(new QueryResolveBuildTypeStep())
               .AddNextStep(new QueryResolveAssociatedQueryStep())
               .AddNextStep(new QueryResolveSingleExcuteStep())
               .AddNextStep(new QueryResolveExtendAssignmentStep());

            StepResolveAllQuery.AddNextStep(new QueryResolvePermissionStep())
              .AddNextStep(new QueryResolveWhereStep())
              .AddNextStep(new QueryResolveOrderStep())
              .AddNextStep(new QueryResolveJoinStep())
              .AddNextStep(new QueryResolveBuildAllCommandStep())
              .AddNextStep(new QueryResolveBuildTypeStep())
              .AddNextStep(new QueryResolveAssociatedQueryStep())
              .AddNextStep(new QueryResolveAllExcuteStep())
              .AddNextStep(new QueryResolveExtendAssignmentStep());
        }
    }
}
