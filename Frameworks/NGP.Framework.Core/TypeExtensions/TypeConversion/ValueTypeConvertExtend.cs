/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * ValueTypeConvertExtend Description:
 * 类型转换扩展
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-15   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace NGP.Framework.Core
{
    /// <summary>
    /// 类型转换扩展
    /// </summary>
    public static class ValueTypeConvertExtend
    {
        #region static ctor
        /// <summary>
        /// static ctor
        /// </summary>
        static ValueTypeConvertExtend()
        {
            SingletonNew<ConcurrentDictionary<Type, IChangeValue>>.Instance.TryAdd(typeof(bool), new BooleanChangeValue());
            SingletonNew<ConcurrentDictionary<Type, IChangeValue>>.Instance.TryAdd(typeof(int), new IntChangeValue());
            SingletonNew<ConcurrentDictionary<Type, IChangeValue>>.Instance.TryAdd(typeof(byte), new ByteChangeValue());
            SingletonNew<ConcurrentDictionary<Type, IChangeValue>>.Instance.TryAdd(typeof(DateTime), new DateTimeChangeValue());
            SingletonNew<ConcurrentDictionary<Type, IChangeValue>>.Instance.TryAdd(typeof(decimal), new DecimalChangeValue());
            SingletonNew<ConcurrentDictionary<Type, IChangeValue>>.Instance.TryAdd(typeof(float), new FloatChangeValue());
            SingletonNew<ConcurrentDictionary<Type, IChangeValue>>.Instance.TryAdd(typeof(double), new DoubleChangeValue());
            SingletonNew<ConcurrentDictionary<Type, IChangeValue>>.Instance.TryAdd(typeof(long), new LongChangeValue());
            SingletonNew<ConcurrentDictionary<Type, IChangeValue>>.Instance.TryAdd(typeof(short), new ShortChangeValue());
            SingletonNew<ConcurrentDictionary<Type, IChangeValue>>.Instance.TryAdd(typeof(bool?), new BooleanChangeValue());
            SingletonNew<ConcurrentDictionary<Type, IChangeValue>>.Instance.TryAdd(typeof(int?), new IntChangeValue());
            SingletonNew<ConcurrentDictionary<Type, IChangeValue>>.Instance.TryAdd(typeof(byte?), new ByteChangeValue());
            SingletonNew<ConcurrentDictionary<Type, IChangeValue>>.Instance.TryAdd(typeof(DateTime?), new DateTimeChangeValue());
            SingletonNew<ConcurrentDictionary<Type, IChangeValue>>.Instance.TryAdd(typeof(decimal?), new DecimalChangeValue());
            SingletonNew<ConcurrentDictionary<Type, IChangeValue>>.Instance.TryAdd(typeof(float?), new FloatChangeValue());
            SingletonNew<ConcurrentDictionary<Type, IChangeValue>>.Instance.TryAdd(typeof(double?), new DoubleChangeValue());
            SingletonNew<ConcurrentDictionary<Type, IChangeValue>>.Instance.TryAdd(typeof(long?), new LongChangeValue());
            SingletonNew<ConcurrentDictionary<Type, IChangeValue>>.Instance.TryAdd(typeof(short?), new ShortChangeValue());
        }
        #endregion

        #region 类型判断
        /// <summary>
        /// 类型判断
        /// </summary>
        /// <typeparam LanguageName="TEntity">需要判断的类型</typeparam>
        /// <param name="value">判断的对象</param>
        /// <returns>判断的结果</returns>
        public static bool IsCheckType<T>(this string value)
        {
            value = (value ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }
            var type = typeof(T);

            IChangeValue changeType = null;
            if (!SingletonNew<ConcurrentDictionary<Type, IChangeValue>>.Instance.TryGetValue(type, out changeType))
            {
                return false;
            }

            if (changeType != null)
            {
                dynamic changeValue = changeType;
                return changeValue.CheckAndChange(value).HasChange;
            }

            return false;
        }
        #endregion

        #region 类型转换
        /// <summary>
        /// 类型转换
        /// </summary>
        /// <typeparam name="TInput">输入类型</typeparam>
        /// <typeparam name="TOutput">输出类型</typeparam>
        /// <param name="value">源值</param>
        /// <param name="defaultValue">转换失败时的默认值</param>
        /// <param name="isEnumDefined">是否枚举定义</param>
        /// <returns>转换结果</returns>
        public static TOutput To<TInput, TOutput>(this TInput value, TOutput defaultValue = default(TOutput), bool isEnumDefined = true)
        {
            try
            {
                var inputType = typeof(TInput);
                var outputType = typeof(TOutput);

                if (inputType.IsValueType && outputType.IsValueType)
                {
                    // 判断泛型值是否为默认值
                    if (value is Nullable && EqualityComparer<TInput>.Default.Equals(value, default(TInput)))
                    {
                        return defaultValue;
                    }
                    if (outputType.IsGenericType)
                    {
                        outputType = outputType.GenericTypeArguments.FirstOrDefault();
                    }
                    TypeConverter outputConverter = CustomTypeConverterHelper.GetCustomTypeConverter(outputType);
                    TypeConverter inputConverter = CustomTypeConverterHelper.GetCustomTypeConverter(inputType);
                    if (outputConverter != null && outputConverter.CanConvertFrom(inputType))
                    {
                        return (TOutput)outputConverter.ConvertFrom(null, CultureInfo.InvariantCulture, value);
                    }
                    if (inputConverter != null && inputConverter.CanConvertTo(outputType))
                    {
                        return (TOutput)inputConverter.ConvertTo(null, CultureInfo.InvariantCulture, value, outputType);
                    }
                }
                if ((inputType.IsValueType && !inputType.IsEnum && !inputType.IsGenericType) ||
                    (inputType.IsGenericType && inputType.IsValueType && !inputType.GenericTypeArguments.FirstOrDefault().IsEnum))
                {
                    // 如果目标是枚举类型
                    if (outputType.IsEnum)
                    {
                        if ((isEnumDefined && Enum.IsDefined(outputType, value.To<TInput, int>())) || !isEnumDefined)
                        {
                            defaultValue = (TOutput)Enum.Parse(outputType, value.ConvertToString(), false);
                        }
                        return defaultValue;
                    }
                    else if (typeof(bool).Equals(inputType) && typeof(System.Int32).Equals(outputType))
                    {
                        return Convert.ToInt32(value).To<int, TOutput>();

                    }
                    else if (typeof(bool?).Equals(inputType) && typeof(System.Int32).Equals(outputType))
                    {
                        if ((value as bool?).HasValue)
                        {
                            return Convert.ToInt32(value).To<int, TOutput>();
                        }
                        else
                        {
                            return default(TOutput);
                        }

                    }

                    return value.ConvertToString().To<TOutput>();
                }
                return (TOutput)value.ToObject(outputType, CultureInfo.InvariantCulture);
            }
            catch
            {
                return defaultValue;
            }
        }
        #endregion

        #region 类型转换（string转换，如果转换失败则返回类型的默认值）
        /// <summary>
        /// 类型转换（string转换，如果转换失败则返回类型的默认值）
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="value">需要转换的值</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>转换结果</returns>
        public static T To<T>(this string value, T defaultValue = default(T))
        {
            try
            {
                value = (value ?? string.Empty).Trim();
                dynamic returnValue = defaultValue;
                if (string.IsNullOrWhiteSpace(value))
                {
                    return returnValue;
                }

                var type = typeof(T);
                IChangeValue changeType = null;
                if (SingletonNew<ConcurrentDictionary<Type, IChangeValue>>.Instance.TryGetValue(type, out changeType))
                {
                    if (changeType != null)
                    {
                        dynamic changeValue = changeType;
                        var change = changeValue.CheckAndChange(value);
                        if (change.HasChange)
                        {
                            return change.ChangeResult;
                        }
                        return returnValue;
                    }
                }
                if (type.IsEnum)
                {
                    if (value.IsCheckType<int>())
                    {
                        returnValue = Enum.ToObject(type, int.Parse(value));
                    }
                    return returnValue;
                }

                return (T)value.ToObject(typeof(T), CultureInfo.InvariantCulture);
            }
            catch
            {
                return defaultValue;
            }
        }
        #endregion

        #region 类型判断 object
        /// <summary>
        /// 类型判断
        /// </summary>
        /// <typeparam LanguageName="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsCheckType<T>(this object value)
        {
            if (value == null)
            {
                return false;
            }
            return value.ToString().IsCheckType<T>();
        }
        #endregion



        #region 类型转换 object
        /// <summary>
        /// Converts a value to a destination type.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="destinationType">The type to convert the value to.</param>
        /// <returns>The converted value.</returns>
        public static object ToObject(object value, Type destinationType)
        {
            return ToObject(value, destinationType, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts a value to a destination type.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <typeparam name="T">The type to convert the value to.</typeparam>
        /// <returns>The converted value.</returns>
        public static T To<T>(this object value)
        {
            return (T)ToObject(value, typeof(T));
        }

        /// <summary>
        /// 类型转换
        /// </summary>
        /// <param name="value"></param>
        /// <param name="destinationType"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static object ToObject(this object value, Type destinationType, CultureInfo culture)
        {
            if (value == null)
                return null;

            var sourceType = value.GetType();

            var destinationConverter = TypeDescriptor.GetConverter(destinationType);
            if (destinationConverter.CanConvertFrom(value.GetType()))
                return destinationConverter.ConvertFrom(null, culture, value);

            var sourceConverter = TypeDescriptor.GetConverter(sourceType);
            if (sourceConverter.CanConvertTo(destinationType))
                return sourceConverter.ConvertTo(null, culture, value, destinationType);

            if (destinationType.IsEnum && value is int)
                return Enum.ToObject(destinationType, (int)value);

            if (!destinationType.IsInstanceOfType(value))
                return Convert.ChangeType(value, destinationType, culture);

            return value;
        }
        #endregion

        #region 类型转换（object 转换，如果转换失败则有可能会异常）
        /// <summary>
        /// 类型转换（object 转换，如果转换失败则有可能会异常）
        /// 例如："" -> int?  null
        ///       double类型的 1.5 -> int 2
        /// </summary>
        /// <typeparam LanguageName="T">目标类型</typeparam>
        /// <param name="value">需要转换的值</param>
        /// <returns>转换结果</returns>
        public static T ToObject<T>(this object value)
        {
            if (value == null)
            {
                return default(T);
            }

            try
            {
                var valueType = value.GetType();

                if (valueType.Equals(typeof(string)))
                {
                    return ((string)value).To<T>();
                }

                if (valueType.IsValueType && !valueType.IsEnum)
                {
                    if (!valueType.IsGenericType)
                    {
                        return value.ConvertToString().To<T>();
                    }
                    if (valueType.IsGenericType && valueType.IsValueType && !valueType.GenericTypeArguments.FirstOrDefault().IsEnum)
                    {
                        return value.ConvertToString().To<T>();
                    }
                }

                var ttype = typeof(T);
                if (valueType.IsEnum || (valueType.IsGenericType && valueType.GenericTypeArguments.FirstOrDefault().IsEnum))
                {
                    if (ttype.IsGenericType && ttype.IsValueType && ttype.GenericTypeArguments.FirstOrDefault().IsValueType)
                    {
                        ttype = ttype.GenericTypeArguments.FirstOrDefault();
                    }
                }
                return (T)value.ToObject(ttype, CultureInfo.InvariantCulture);
            }
            catch
            {
                return default(T);
            }
        }
        #endregion

        #region 类型转换为string
        /// <summary>
        /// 类型转换为string
        /// </summary>
        /// <typeparam name="T">具体类型</typeparam>
        /// <param name="value">具体值</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>返回结果</returns>
        public static string ConvertToString<T>(this T value, string defaultValue = null)
        {
            var type = typeof(T);

            if (!type.IsGenericType && type.IsValueType)
            {
                return Convert.ToString(value);
            }

            // 判断泛型值是否为默认值
            if (EqualityComparer<T>.Default.Equals(value, default(T)))
            {
                return defaultValue;
            }

            if (type.IsEnum || (type.IsGenericType && type.IsValueType && type.GenericTypeArguments.FirstOrDefault().IsEnum))
            {
                return Convert.ToString(value.ToObject<long>());
            }
            return value.ToString();
        }
        #endregion

        #region 字符串转换成枚举
        /// <summary>
        /// 字符串转换成枚举
        /// </summary>
        /// <typeparam name="TEnum">枚举类型</typeparam>
        /// <param name="value">字符串值</param>
        /// <param name="defaultValue">枚举默认值</param>
        /// <returns></returns>
        public static TEnum ToEnum<TEnum>(this string value, TEnum defaultValue = default(TEnum)) where TEnum : struct
        {
            Enum.TryParse(value, true, out defaultValue);
            return defaultValue;
        }
        #endregion

    }
}
