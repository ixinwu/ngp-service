/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * BaseTreeInfo Description:
 * 基本树类型
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
    /// 基本树类型
    /// </summary>
    public class BaseTreeInfo
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 子列表
        /// </summary>
        public List<BaseTreeInfo> Children { get; set; }

        /// <summary>
        /// 上级Id
        /// </summary>
        public string ParentId { get; set; }
    }
}
