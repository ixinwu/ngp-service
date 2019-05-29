/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * ColumnType Description:
 * 数据库列类型
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019/3/11 14:53:01 hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/


namespace NGP.Framework.Core
{
    /// <summary>
    /// 数据库列类型
    /// </summary>
    public enum FieldColumnType
    {
        /// <summary>
        /// 字符串
        /// </summary>
        String = 0,

        /// <summary>
        /// bool类型
        /// </summary>
        Bool,

        /// <summary>
        /// 整形
        /// </summary>
        Integer,

        /// <summary>
        /// 时间
        /// </summary>
        Time,

        /// <summary>
        /// 日期
        /// </summary>
        Date,

        /// <summary>
        /// 日期时间
        /// </summary>
        DateTime,

        /// <summary>
        /// 浮点数
        /// </summary>
        Decimal,

        /// <summary>
        /// 字节流
        /// </summary>
        Bits,

        /// <summary>
        /// mongo图片
        /// </summary>
        MongoImage
    }
}
