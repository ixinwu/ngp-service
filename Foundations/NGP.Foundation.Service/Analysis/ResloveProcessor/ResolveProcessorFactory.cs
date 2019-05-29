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
        public static IStep<QueryResolveContext> PageQueryStep { get; private set; } = new QueryResolveInitializeStep();

        /// <summary>
        /// 解析创建状态初始化步骤--单条查询
        /// </summary>
        public static IStep<QueryResolveContext> SingleQueryStep { get; private set; } = new QueryResolveInitializeStep();

        /// <summary>
        /// 解析创建状态初始化步骤--全部查询
        /// </summary>
        public static IStep<QueryResolveContext> AllQueryStep { get; private set; } = new QueryResolveInitializeStep();

        /// <summary>
        /// 删除步骤
        /// </summary>
        public static IStep<OperatorResolveContext<DynamicDeleteRequest>> DeleteSetp { get; private set; }
        = new OperatorResolveInitializeStep<DynamicDeleteRequest>();

        /// <summary>
        /// 追加步骤
        /// </summary>
        public static IStep<OperatorResolveContext<DynamicInsertRequest>> InsertSetp { get; private set; }
        = new OperatorResolveInitializeStep<DynamicInsertRequest>();

        /// <summary>
        /// 更新步骤
        /// </summary>
        public static IStep<OperatorResolveContext<DynamicUpdateRequest>> UpdateSetp { get; private set; }
        = new OperatorResolveInitializeStep<DynamicUpdateRequest>();

        /// <summary>
        /// ctor
        /// </summary>
        static ResolveProcessorFactory()
        {
            PageQueryStep.AddNextStep(new QueryResolvePermissionStep())
                .AddNextStep(new QueryResolveWhereStep())
                .AddNextStep(new QueryResolveOrderStep())
                .AddNextStep(new QueryResolveJoinStep())
                .AddNextStep(new QueryResolveBuildPageCommandStep())
                .AddNextStep(new QueryResolveFindNameFieldStep())
                .AddNextStep(new QueryResolveAssociatedQueryStep())
                .AddNextStep(new QueryResolvePageExcuteStep())
                .AddNextStep(new QueryResolveExtendAssignmentStep());

            SingleQueryStep.AddNextStep(new QueryResolvePermissionStep())
               .AddNextStep(new QueryResolveWhereStep())
               .AddNextStep(new QueryResolveOrderStep())
               .AddNextStep(new QueryResolveJoinStep())
               .AddNextStep(new QueryResolveBuildSingleCommandStep())
               .AddNextStep(new QueryResolveFindNameFieldStep())
               .AddNextStep(new QueryResolveAssociatedQueryStep())
               .AddNextStep(new QueryResolveSingleExcuteStep())
               .AddNextStep(new QueryResolveExtendAssignmentStep());

            AllQueryStep.AddNextStep(new QueryResolvePermissionStep())
              .AddNextStep(new QueryResolveWhereStep())
              .AddNextStep(new QueryResolveOrderStep())
              .AddNextStep(new QueryResolveJoinStep())
              .AddNextStep(new QueryResolveBuildAllCommandStep())
              .AddNextStep(new QueryResolveFindNameFieldStep())
              .AddNextStep(new QueryResolveAssociatedQueryStep())
              .AddNextStep(new QueryResolveAllExcuteStep())
              .AddNextStep(new QueryResolveExtendAssignmentStep());

            DeleteSetp.AddNextStep(new OperatorResolvePermissionStep<DynamicDeleteRequest>())
                .AddNextStep(new OperatorResolveBuildDeleteStep())
                .AddNextStep(new OperatorResolveBuildExcuteStep<DynamicDeleteRequest>());

            InsertSetp.AddNextStep(new OperatorResolvePermissionStep<DynamicInsertRequest>())
                .AddNextStep(new OperatorResolveBuildInsertStep())
                .AddNextStep(new OperatorResolveBuildExcuteStep<DynamicInsertRequest>());

            UpdateSetp.AddNextStep(new OperatorResolvePermissionStep<DynamicUpdateRequest>())
                .AddNextStep(new OperatorResolveBuildUpdateStep())
                .AddNextStep(new OperatorResolveBuildExcuteStep<DynamicUpdateRequest>());
        }
    }
}
