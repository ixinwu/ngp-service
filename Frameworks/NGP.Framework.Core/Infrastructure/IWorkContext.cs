/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * IWorkContext Description:
 * 当前工作上下文
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-15   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

namespace NGP.Framework.Core
{
    /// <summary>
    /// 当前工作上下文
    /// </summary>
    public interface IWorkContext
    {
        /// <summary>
        /// 当前人员信息
        /// </summary>
        WorkEmployee Current { get; set; }
        /// <summary>
        /// 当前Api请求信息
        /// </summary>
        WorkRequestInfo CurrentRequest { get; set; }

        /// <summary>
        /// 工作语言
        /// </summary>
        WorkLanguage WorkingLanguage { get; set; }
    }
}
