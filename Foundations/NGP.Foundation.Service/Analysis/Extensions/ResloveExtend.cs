/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved.
 * 
 * ResloveExtend Description:
 * 解析扩展
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019/2/19  hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using NGP.Framework.Core;
using System;
using System.Collections.Generic;

namespace NGP.Foundation.Service.Analysis
{
    /// <summary>
    /// 解析扩展
    /// </summary>
    public static class ResloveExtend
    {
        /// <summary>
        /// 获取代码的类型
        /// </summary>
        /// <param name="columnType">字段类型</param>
        /// <returns>代码类型</returns>
        public static Type GetCodeType(this FieldColumnType columnType)
        {
            switch (columnType)
            {
                // 日期被格式化成字符串
                case FieldColumnType.DateTime:
                case FieldColumnType.Date:
                case FieldColumnType.Time:
                    return typeof(DateTime?);
                case FieldColumnType.Integer:
                    return typeof(int?);
                case FieldColumnType.Decimal:
                    return typeof(decimal?);
                case FieldColumnType.Bool:
                    return typeof(bool?);
                case FieldColumnType.Bits:
                    return typeof(byte[]);
                case FieldColumnType.String:
                default:
                    return typeof(string);
            }
        }
    }
}
