/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * GuidExtend Description:
 * Guid扩展
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
    /// Guid扩展
    /// </summary>
    public static class GuidExtend
    {
        /// <summary>
        /// 生成新的Guid
        /// </summary>
        /// <param name="removeLine"></param>
        /// <returns></returns>
        public static string NewGuid(bool removeLine = true)
        {
            if (removeLine)
            {
                // 去横杠再转换成大写
                return Guid.NewGuid().ToString("N").ToUpper();
            }
            return Guid.NewGuid().ToString();
        }

        /// <summary>
        /// 默认Guid去除-
        /// </summary>
        public static string EmptyGuid
        {
            get => Guid.Empty.ToString("N");
        }
    }
}
