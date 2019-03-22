/* ------------------------------------------------------------------------------
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * OperatorResolvePermissionStep Description:
 * 操作时权限判断初始化处理
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
    /// 操作时权限判断初始化处理
    /// </summary>
    public class OperatorResolvePermissionStep<TRequest> : StepBase<OperatorResolveContext<TRequest>> where TRequest : DynamicBaseRequest
    {
        /// <summary>
        /// 执行上下文
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public override bool Process(OperatorResolveContext<TRequest> ctx)
        {
            

            return true;
        }


    }
}
