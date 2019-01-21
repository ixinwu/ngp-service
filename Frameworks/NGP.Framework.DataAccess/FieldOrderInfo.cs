/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * FieldOrderInfo Description:
 * 数据库字段列顺序
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-27   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

namespace NGP.Framework.DataAccess
{
    /// <summary>
    /// 数据库字段列顺序
    /// </summary>
    public class FieldOrderInfo
    {
        /// <summary>
        /// 数据库真实列序号
        /// </summary>
        public int ColumnIndex { get; set; }

        /// <summary>
        /// 数据库真实列名称
        /// </summary>
        public string FieldKey { get; set; }

    }
}
