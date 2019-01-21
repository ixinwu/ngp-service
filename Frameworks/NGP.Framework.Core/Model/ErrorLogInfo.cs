/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * ErrorLogInfo Description:
 * 错误日志自定义对象
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
    /// 错误日志自定义对象
    /// </summary>
    public class ErrorLogInfo
    {
        /// <summary>
        /// api路径
        /// </summary>
        public string ApiUrl { get; set; }
        /// <summary>
        /// api提交参数
        /// </summary>
        public string ApiPostParameter { get; set; }
        /// <summary>
        /// 业务方法
        /// </summary>
        public string BusinessMethod { get; set; }
        /// <summary>
        /// 异常内容
        /// </summary>
        public string ExceptionContent { get; set; }

        /// <summary>
        /// 操作者
        /// </summary>
        public string OperateBy { get; set; }
        /// <summary>
        /// 操作部门
        /// </summary>
        public string OperateDept { get; set; }

        /// <summary>
        /// 异常内容
        /// </summary>
        public Exception Exception { get; set; }
    }
}
