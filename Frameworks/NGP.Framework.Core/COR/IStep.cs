/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * IStep Description:
 * 职责链模式中每个职责（处理步骤）需要继承的接口
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-15   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using System;

namespace NGP.Framework.Core
{
    /// <summary>
    /// 职责链模式中每个职责（处理步骤）需要继承的接口
    /// </summary>
    /// <typeparam name="TContext">传递上下文</typeparam>
    public interface IStep<TContext>
    {
        /// <summary>
        /// 添加下一步骤
        /// </summary>
        /// <param name="step">下一步骤</param>
        /// <param name="resultFor">定义的下一步骤是在上一步成功还失败后执行</param>
        /// <returns>下一步骤</returns>
        IStep<TContext> AddNextStep(IStep<TContext> step, bool resultFor = true);
        /// <summary>
        /// 处理操作
        /// </summary>
        /// <param name="ctx">处理上下文</param>
        void HandleProcess(TContext ctx);

        /// <summary>
        /// 步骤处理结束回调
        /// </summary>
        EventHandler<Tuple<TContext, bool>> OnStepComplete { get; set; }
    }
}
