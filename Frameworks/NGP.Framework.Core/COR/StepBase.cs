/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * IStep Description:
 * 职责链模式中每个职责（处理步骤）需要继承的基类
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-15   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/


using System;
using System.Collections.Generic;

namespace NGP.Framework.Core
{
    /// <summary>
    /// 职责链模式中每个职责（处理步骤）需要继承的基类
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public abstract class StepBase<TContext> : IStep<TContext>
    {
        private readonly List<IStep<TContext>> _trueSteps = new List<IStep<TContext>>();

        private readonly List<IStep<TContext>> _falseSteps = new List<IStep<TContext>>();
        /// <summary>
        /// 添加下一步骤
        /// </summary>
        /// <param name="step">下一步骤</param>
        /// <param name="resultFor">定义的下一步骤是在上一步成功还失败后执行</param>
        /// <returns>下一步骤</returns>
        public IStep<TContext> AddNextStep(IStep<TContext> step, bool resultFor = true)
        {
            if (resultFor)
            {
                _trueSteps.Add(step);
            }
            else
            {
                _falseSteps.Add(step);
            }
            return step;
        }
        /// <summary>
        /// 步骤处理结束回调
        /// </summary>
        public EventHandler<Tuple<TContext, bool>> OnStepComplete { get; set; }
        /// <summary>
        /// 获取下一步骤列表
        /// </summary>
        /// <param name="resultFor"></param>
        /// <returns></returns>
        public List<IStep<TContext>> GetNextSteps(bool resultFor)
        {
            return resultFor ? _trueSteps : _falseSteps;
        }

        /// <summary>
        /// 执行上下文
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public abstract bool Process(TContext ctx);
        /// <summary>
        /// 处理操作
        /// </summary>
        /// <param name="ctx">处理上下文</param>
        public virtual void HandleProcess(TContext ctx)
        {
            bool result = Process(ctx);

            OnStepComplete?.Invoke(this, new Tuple<TContext, bool>(ctx, result));

            var nextSteps = GetNextSteps(result);

            if (nextSteps != null && nextSteps.Count > 0)
            {
                foreach (IStep<TContext> step in nextSteps)
                {
                    step.HandleProcess(ctx);
                }
            }
        }
    }
}
