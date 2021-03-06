﻿/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * ErrorCode Description:
 * 错误code
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-15   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

namespace NGP.Framework.Core
{
    /// <summary>
    /// 错误code
    /// </summary>
    public enum ErrorCode
    {
        /// <summary>
        /// 默认值
        /// </summary>
        None = 0,

        /// <summary>
        /// 参数为空
        /// </summary>
        ParamEmpty = 10,

        /// <summary>
        /// 不存在
        /// </summary>
        NonExistent = 100,

        /// <summary>
        /// 验证错误
        /// </summary>
        CheckError = 200,

        /// <summary>
        /// 系统异常
        /// </summary>
        SystemError = 500
    }
}
