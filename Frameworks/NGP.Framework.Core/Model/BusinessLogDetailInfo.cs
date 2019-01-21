/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * BusinessLogDetailInfo Description:
 * 业务日志详情
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-15   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/


namespace NGP.Framework.Core
{
    /// <summary>
    /// 业务日志详情
    /// </summary>
    public class BusinessLogDetailInfo
    {
        /// <summary>
        /// 列名
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// 原始值
        /// </summary>
        public string OriginalValue { get; set; }

        /// <summary>
        /// 原始文本
        /// </summary>
        public string OriginalText { get; set; }

        /// <summary>
        /// 当前值
        /// </summary>
        public string CurrentValue { get; set; }

        /// <summary>
        /// 当前文本
        /// </summary>
        public string CurrentText { get; set; }

        /// <summary>
        /// 字段Key
        /// </summary>
        public string FieldKey { get; set; }
    }
}
