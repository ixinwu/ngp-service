/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * WorkLanguage Description:
 * 工作语言
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-15   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

namespace NGP.Framework.Core
{
    /// <summary>
    /// 工作语言
    /// </summary>
    public partial class WorkLanguage : BaseEntity
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 语言文化
        /// </summary>
        public string LanguageCulture { get; set; }

        /// <summary>
        /// 语言文件
        /// </summary>
        public string FlagImageFileName { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int DisplayOrder { get; set; }
    }
}
