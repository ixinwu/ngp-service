/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * AppDefaultFieldConfig Description:
 * 应用默认字段配置
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
    /// 应用默认字段配置
    /// </summary>
    public class AppDefaultFieldConfig
    {
        /// <summary>
        /// 字段名称
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// 字段类型
        /// </summary>
        public string FieldType { get; set; }

        /// <summary>
        /// 默认操作类型
        /// </summary>
        public List<string> DefaultType { get; set; }

        /// <summary>
        /// 最大长度
        /// </summary>
        public int? Maxlength { get; set; }

        /// <summary>
        /// 列类型
        /// </summary>
        public string ColumnType { get; set; }

        /// <summary>
        /// 列名
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// 精度
        /// </summary>
        public int? Precision { get; set; }
    }
}
