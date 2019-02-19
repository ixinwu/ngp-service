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
using System.Runtime.Serialization;

namespace NGP.Framework.Core
{
    /// <summary>
    /// 树类型
    /// </summary>
    [DataContract]
    public class NGPTree
    {
        /// <summary>
        /// Key
        /// </summary>
        [DataMember(Name = "key")]
        public string Key { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        [DataMember(Name = "text")]
        public string Text { get; set; }

        /// <summary>
        /// 子列表
        /// </summary>
        [DataMember(Name = "children")]
        public List<NGPTree> Children { get; set; }

        /// <summary>
        /// 上级Id
        /// </summary>
        [DataMember(Name = "parentId")]
        public string ParentId { get; set; }
    }
}
