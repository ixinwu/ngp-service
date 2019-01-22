/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * INGPResponse Description:
 * NGP返回参数接口
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019/3/20 13:13:18   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/


namespace NGP.Framework.Core
{
    /// <summary>
    /// NGP返回参数接口
    /// </summary>
    public interface INGPResponse
    {
        /// <summary>
        /// 操作状态
        /// </summary>
        OperateStatus Status { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        string Message { get; set; }

        /// <summary>
        /// 错误码
        /// </summary>
        ErrorCode ErrorCode { get; set; }
    }
}
