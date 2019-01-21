/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * IProcessCommand Description:
 * 流程命令接口--每个步骤的输出都会是下个步骤的输入
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-3-20   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/


using System;

namespace NGP.Framework.Core
{
    /// <summary>
    /// 流程命令接口--每个步骤的输出都会是下个步骤的输入
    /// </summary>
    /// <typeparam name="TContext">通用参数</typeparam>
    public interface IProcessCommand<TContext>
    {
        /// <summary>
        /// 添加下一步骤
        /// </summary>
        /// <param name="step">下一步骤</param>
        void AddNextStep(IProcessCommand<TContext> step);

        /// <summary>
        /// 处理操作
        /// </summary>
        /// <param name="request">请求参数</param>
        /// <param name="ctx">上下文参数</param>
        /// <returns>返回处理结果</returns>
        INGPResponse HandleProcess(INGPRequest request,TContext ctx);
    }
}
