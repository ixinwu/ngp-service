/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * CurrentRequestInfo Description:
 * 当前请求信息
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-15   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

namespace NGP.Framework.Core
{
    /// <summary>
    /// 当前请求信息
    /// </summary>
    public class WorkRequestInfo
    {
        /// <summary>
        /// api路径
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// api提交参数
        /// </summary>
        public string Parameter { get; set; }

        /// <summary>
        /// 绝对路径
        /// </summary>
        public string IpAddress { get; set; }
    }
}
