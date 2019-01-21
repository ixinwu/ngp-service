/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * IStep Description:
 * 流程命令基类--每个步骤的输出都会是下个步骤的输入
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-15   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/


using System.Collections.Concurrent;

namespace NGP.Framework.Core
{
    /// <summary>
    /// 流程命令基类--每个步骤的输出都会是下个步骤的输入
    /// </summary>

    public abstract class ProcessCommandBase<TContext> : IProcessCommand<TContext>
    {
        /// <summary>
        /// 线程安全的步骤
        /// </summary>
        private readonly ConcurrentQueue<IProcessCommand<TContext>> _steps = new ConcurrentQueue<IProcessCommand<TContext>>();

        /// <summary>
        /// 添加下一步骤
        /// </summary>
        /// <param name="step">下一步骤</param>
        public void AddNextStep(IProcessCommand<TContext> step)
        {
            // 添加元素
            _steps.Enqueue(step);
        }

        /// <summary>
        /// 执行上下文
        /// </summary>
        /// <param name="request">请求参数</param>
        /// <param name="ctx">上下文参数</param>
        /// <returns>执行结果</returns>
        public abstract INGPResponse Process(INGPRequest request, TContext ctx);

        /// <summary>
        /// 处理操作
        /// </summary>
        /// <param name="request">请求参数</param>
        /// <param name="ctx">上下文参数</param>
        /// <returns>返回处理结果</returns>
        public virtual INGPResponse HandleProcess(INGPRequest request, TContext ctx)
        {
            INGPResponse result = null;
            do
            {
                // 当前执行结果
                result = Process(request, ctx);

                IProcessCommand<TContext> step = null;
                if (!_steps.TryDequeue(out step) || step == null)
                {
                    break;
                }

                var nextRequest = result as INGPRequest;
                step.HandleProcess(nextRequest, ctx);
            }
            while (_steps.Count > 0);

            return result;
        }
    }
}
