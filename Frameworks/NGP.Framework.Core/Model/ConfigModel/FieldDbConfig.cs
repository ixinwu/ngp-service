/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * FieldDbConfig Description:
 * 字段DB配置
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-15   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/


namespace NGP.Framework.Core
{
    /// <summary>
    /// 字段DB配置
    /// </summary>
    public class FieldDbConfig
    {
        /// <summary>
        /// 最大长度
        /// </summary>
        public int? Maxlength { get; set; }

        /// <summary>
        /// 列名
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// 列类型
        /// </summary>
        public string ColumnType { get; set; }

        /// <summary>
        /// 是否复值
        /// </summary>
        public bool? IsMulti { get; set; }

        /// <summary>
        /// 精度
        /// </summary>
        public int? Precision { get; set; }

        /// <summary>
        /// 默认值(追加时如果为NULL填充的默认值)
        /// </summary>
        public object DefaultValue { get; set; }

        /// <summary>
        /// 初始化默认值（创建字段时对原有数据进行初始化填充）
        /// </summary>
        public object InitDefaultValue { get; set; }
    }
}
