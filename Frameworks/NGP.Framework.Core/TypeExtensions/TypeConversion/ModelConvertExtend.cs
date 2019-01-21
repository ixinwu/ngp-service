/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * ModelConvertExtend Description:
 * 模型转换扩展
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-15   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using System;
using System.Linq;
using System.Reflection;

namespace NGP.Framework.Core
{
    /// <summary>
    /// 模型转换扩展
    /// </summary>
    public static class ModelConvertExtend
    {
        #region Methods
        #region 创建一个新对象实例
        /// <summary>
        /// 创建一个新对象实例
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="entity">源对象</param>
        /// <param name="action">回调</param>
        /// <returns>新对象</returns>
        public static T CopyItem<T>(this T entity, Action<T> action = null)
            where T : class, new()
        {
            var result = entity.CopyItem<T, T>();
            action?.Invoke(result);
            return result;
        }
        #endregion

        #region 复制一个对象到另一个对象
        ///// <summary>
        ///// 复制一个对象到另一个对象
        ///// </summary>
        ///// <typeparam LanguageName="TEntity">参数类型</typeparam>
        ///// <param name="entity">源对象</param>
        ///// <param name="target">目标对象</param>
        ///// <param name="action">回调</param>
        //public static void CopyItemTo<T>(this T entity, T target, Action<T> action = null)
        //    where T : class, new()
        //{
        //    if (target == null)
        //    {
        //        return;
        //    }
        //    entity.CopyItemByNames<T, T>(target);
        //    action(target);
        //}

        /// <summary>
        /// 复制一个对象到另一个对象
        /// </summary>
        /// <typeparam LanguageName="TFrom">源类型</typeparam>
        /// <typeparam LanguageName="TTo">目标类型</typeparam>
        /// <param name="entity"></param>
        /// <param name="target"></param>
        /// <param name="action"></param>
        public static void CopyItemTo<TFrom, TTo>(this TFrom entity, TTo target, Action<TFrom, TTo> action = null)
            where TFrom : class
            where TTo : class, new()
        {
            if (target == null)
            {
                return;
            }
            entity.CopyItemByNames<TFrom, TTo>(target);
            if (action != null)
            {
                action(entity, target);
            }
        }

        /// <summary>
        /// 拷贝一个新对象
        /// </summary>
        /// <typeparam LanguageName="TFrom">源类型</typeparam>
        /// <typeparam LanguageName="TTo">目标类型</typeparam>
        /// <param name="entity">源对象</param>
        /// <param name="action">回调</param>
        /// <returns>复制后的对象</returns>
        public static TTo CopyItem<TFrom, TTo>(this TFrom entity, Action<TFrom, TTo> action = null)
            where TFrom : class
            where TTo : class, new()
        {
            var newEntity = new TTo();
            entity.CopyItemByNames<TFrom, TTo>(newEntity);
            if (action != null)
            {
                action(entity, newEntity);
            }
            return newEntity;
        }

        /// <summary>
        /// 根据属性名称拷贝
        /// </summary>
        /// <typeparam LanguageName="TFrom">源类型</typeparam>
        /// <typeparam LanguageName="TTo">目标类型</typeparam>
        /// <param name="fromEntity">源对象</param>
        /// <param name="toEntity">目标对象</param>
        /// <param name="propertyNames">需要拷贝的属性名称</param>
        public static void CopyItemByNames<TFrom, TTo>(this TFrom fromEntity, TTo toEntity,
            params string[] propertyNames)
            where TFrom : class
            where TTo : class, new()
        {
            var fromType = typeof(TFrom);
            var toType = typeof(TTo);
            var properties = from fromProperty in fromType.GetProperties().Where(item => item.CanRead)
                             join toProperty in toType.GetProperties().Where(item => item.CanWrite)
                               on new { Name = fromProperty.Name.ToLower(), fromProperty.PropertyType }
                                equals new { Name = toProperty.Name.ToLower(), toProperty.PropertyType }
                             select new
                             {
                                 fromProperty,
                                 toProperty,
                                 Name = fromProperty.Name.ToLower()
                             };

            if (propertyNames.Length > 0)
            {
                var lowerNames = propertyNames.Select(s => s.ToLower()).ToList();
                properties = properties.Where(item => lowerNames.Contains(item.Name));
            }

            toEntity = toEntity ?? new TTo();
            foreach (var item in properties)
            {
                fromEntity.CopyValue(toEntity, item.fromProperty, item.toProperty);
            }
        }
        #endregion

        #region 设定值
        /// <summary>
        /// 设定值
        /// </summary>
        /// <typeparam LanguageName="TFrom">源类型</typeparam>
        /// <typeparam LanguageName="TTo">目标类型</typeparam>
        /// <param name="fromEntity">源对象</param>
        /// <param name="toEntity">目标对象</param>
        /// <param name="getProperty">获取属性</param>
        /// <param name="setProperty">设定属性</param>
        /// <param name="isCache">用于列表深度复制的参数</param>
        private static void CopyValue<TFrom, TTo>(this TFrom fromEntity, TTo toEntity,
            PropertyInfo getProperty, PropertyInfo setProperty, bool isCache = true)
            where TFrom : class
            where TTo : class
        {
            if (getProperty.PropertyType.Equals(typeof(string)))
            {
                fromEntity.GetAndSetProperty<TFrom, TTo, string>(toEntity, getProperty, setProperty, isCache);
            }
            else if (getProperty.PropertyType.Equals(typeof(int)))
            {
                fromEntity.GetAndSetProperty<TFrom, TTo, int>(toEntity, getProperty, setProperty, isCache);
            }
            else if (getProperty.PropertyType.Equals(typeof(int?)))
            {
                fromEntity.GetAndSetProperty<TFrom, TTo, int?>(toEntity, getProperty, setProperty, isCache);
            }
            else if (getProperty.PropertyType.Equals(typeof(Int16)))
            {
                fromEntity.GetAndSetProperty<TFrom, TTo, Int16>(toEntity, getProperty, setProperty, isCache);
            }
            else if (getProperty.PropertyType.Equals(typeof(Int16?)))
            {
                fromEntity.GetAndSetProperty<TFrom, TTo, Int16?>(toEntity, getProperty, setProperty, isCache);
            }
            else if (getProperty.PropertyType.Equals(typeof(byte)))
            {
                fromEntity.GetAndSetProperty<TFrom, TTo, byte>(toEntity, getProperty, setProperty, isCache);
            }
            else if (getProperty.PropertyType.Equals(typeof(byte?)))
            {
                fromEntity.GetAndSetProperty<TFrom, TTo, byte?>(toEntity, getProperty, setProperty, isCache);
            }
            else if (getProperty.PropertyType.Equals(typeof(long)))
            {
                fromEntity.GetAndSetProperty<TFrom, TTo, long>(toEntity, getProperty, setProperty, isCache);
            }
            else if (getProperty.PropertyType.Equals(typeof(long?)))
            {
                fromEntity.GetAndSetProperty<TFrom, TTo, long?>(toEntity, getProperty, setProperty, isCache);
            }
            else if (getProperty.PropertyType.Equals(typeof(decimal)))
            {
                fromEntity.GetAndSetProperty<TFrom, TTo, decimal>(toEntity, getProperty, setProperty, isCache);
            }
            else if (getProperty.PropertyType.Equals(typeof(decimal?)))
            {
                fromEntity.GetAndSetProperty<TFrom, TTo, decimal?>(toEntity, getProperty, setProperty, isCache);
            }
            else if (getProperty.PropertyType.Equals(typeof(double)))
            {
                fromEntity.GetAndSetProperty<TFrom, TTo, double>(toEntity, getProperty, setProperty, isCache);
            }
            else if (getProperty.PropertyType.Equals(typeof(double?)))
            {
                fromEntity.GetAndSetProperty<TFrom, TTo, double?>(toEntity, getProperty, setProperty, isCache);
            }
            else if (getProperty.PropertyType.Equals(typeof(DateTime)))
            {
                fromEntity.GetAndSetProperty<TFrom, TTo, DateTime>(toEntity, getProperty, setProperty, isCache);
            }
            else if (getProperty.PropertyType.Equals(typeof(DateTime?)))
            {
                fromEntity.GetAndSetProperty<TFrom, TTo, DateTime?>(toEntity, getProperty, setProperty, isCache);
            }
            else if (getProperty.PropertyType.Equals(typeof(bool)))
            {
                fromEntity.GetAndSetProperty<TFrom, TTo, bool>(toEntity, getProperty, setProperty, isCache);
            }
            else if (getProperty.PropertyType.Equals(typeof(bool?)))
            {
                fromEntity.GetAndSetProperty<TFrom, TTo, bool?>(toEntity, getProperty, setProperty, isCache);
            }
            else if (getProperty.PropertyType.Equals(typeof(byte[])))
            {
                fromEntity.GetAndSetProperty<TFrom, TTo, byte[]>(toEntity, getProperty, setProperty, isCache);
            }
            else
            {
                fromEntity.GetAndSetProperty<TFrom, TTo, object>(toEntity, getProperty, setProperty, isCache);
            }
        }
        #endregion
        #endregion
    }
}
