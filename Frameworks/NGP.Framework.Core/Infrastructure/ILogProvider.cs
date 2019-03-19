/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * ILogProvider Description:
 * 日志提供者
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-15   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using System.Collections.Generic;

namespace NGP.Framework.Core
{
    /// <summary>
    /// 日志提供者
    /// </summary>
    public interface ILogProvider
    {
        /// <summary>
        /// 插入错误日志
        /// </summary>
        /// <param name="info">日志对象</param>
        void InsertSysErrorLog(NGPExceptionLog info);

        /// <summary>
        /// 插入业务日志
        /// </summary>
        /// <param name="context">日志对象</param>
        void InsertBusinessLog(NGPBusinessLog context);
    }
}
