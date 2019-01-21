/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * GenericListTypeConverter Description:
 * 列表类型转换器
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-15   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace NGP.Framework.Core
{
    /// <summary>
    /// 列表类型转换器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class GenericListTypeConverter<T> : TypeConverter
    {
        #region Fields
        /// <summary>
        /// 统一转换容器
        /// </summary>
        protected readonly TypeConverter typeConverter;
        #endregion

        #region Ctor
        /// <summary>
        /// 构造函数
        /// </summary>
        public GenericListTypeConverter()
        {
            typeConverter = TypeDescriptor.GetConverter(typeof(T));
            if (typeConverter == null)
            {
                throw new InvalidOperationException("No Type converter exists for Type " + typeof(T).FullName);
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// 从逗号分隔字符串中获取字符串数组
        /// </summary>
        /// <param name="input">Input</param>
        /// <returns>Array</returns>
        protected virtual string[] GetStringArray(string input)
        {
            return string.IsNullOrEmpty(input) ? new string[0] : input.SplitAndRemoveEmptyRepeat(",").ToArray();
        }

        /// <summary>
        /// 能否转换到目标类型
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                string[] items = GetStringArray(sourceType.ToString());
                return items.Any();
            }

            return base.CanConvertFrom(context, sourceType);
        }

        /// <summary>
        /// 转换类型
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                string[] items = GetStringArray((string)value);
                var result = new List<T>();
                Array.ForEach(items, s =>
                {
                    object item = typeConverter.ConvertFromInvariantString(s);
                    if (item != null)
                    {
                        result.Add((T)item);
                    }
                });

                return result;
            }
            return base.ConvertFrom(context, culture, value);
        }

        /// <summary>
        /// 转换类型
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                string result = string.Empty;
                if (value != null)
                {
                    //we don't use string.Join() because it doesn't support invariant culture
                    for (int i = 0; i < ((IList<T>)value).Count; i++)
                    {
                        var str1 = Convert.ToString(((IList<T>)value)[i], CultureInfo.InvariantCulture);
                        result += str1;
                        //don't add comma after the last element
                        if (i != ((IList<T>)value).Count - 1)
                            result += ",";
                    }
                }
                return result;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
        #endregion
    }
}
