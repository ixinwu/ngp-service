/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * CustomTypeConverterHelper Description:
 * 自定义类型转换
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-15   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace NGP.Framework.Core
{
    /// <summary>
    /// 自定义类型转换
    /// </summary>
    public static class CustomTypeConverterHelper
    {
        /// <summary>
        /// 获取需要转换的类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static TypeConverter GetCustomTypeConverter(Type type)
        {
            if (type == typeof(List<int>))
            {
                return new GenericListTypeConverter<int>();
            }
            if (type == typeof(List<int?>))
            {
                return new GenericListTypeConverter<int?>();
            }
            if (type == typeof(List<decimal>))
            {
                return new GenericListTypeConverter<decimal>();
            }
            if (type == typeof(List<decimal?>))
            {
                return new GenericListTypeConverter<decimal?>();
            }
            if (type == typeof(List<double>))
            {
                return new GenericListTypeConverter<double>();
            }
            if (type == typeof(List<double?>))
            {
                return new GenericListTypeConverter<double?>();
            }
            if (type == typeof(List<DateTime>))
            {
                return new GenericListTypeConverter<DateTime>();
            }
            if (type == typeof(List<string>))
            {
                return new GenericListTypeConverter<string>();
            }
            return TypeDescriptor.GetConverter(type);
        }
    }
}
