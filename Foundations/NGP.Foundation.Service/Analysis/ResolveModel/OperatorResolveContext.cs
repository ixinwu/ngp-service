/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved.
 * 
 * DeleteResolveContext Description:
 * 删除解析上下文
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019/2/19  hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using NGP.Framework.Core;

namespace NGP.Foundation.Service.Analysis
{
    /// <summary>
    /// 删除解析上下文
    /// </summary>
    public class OperatorResolveContext<TRequest> where TRequest : DynamicBaseRequest
    {
        /// <summary>
        /// 请求对象
        /// </summary>
        public TRequest Request { get; set; }

        /// <summary>
        /// 初始化对象
        /// </summary>
        public ResolveInitContext InitContext { get; set; } = new ResolveInitContext();

        /// <summary>
        /// 执行linq命令
        /// </summary>
        public string ExcuteLinqText { get; set; }

        /// <summary>
        /// 执行结果
        /// </summary>
        public NGPResponse Response { get; set; }
    }
}
