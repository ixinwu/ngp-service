/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * ReflectionExtend Description:
 * 设定属性值的扩展
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-15   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using System.Reflection;

namespace NGP.Framework.Core
{
    /// <summary>
    /// 设定属性值的扩展
    /// </summary>
    public static class ReflectionExtend
    {
        /// <summary>
        /// 设定属性值的扩展
        /// </summary>
        /// <typeparam name="TEntity">实例的类型</typeparam>
        /// <typeparam name="TValue">值类型</typeparam>
        /// <param name="property">需要设定的属性</param>
        /// <param name="instance">实例对象</param>
        /// <param name="value">需要设定的值</param>
        /// <param name="isCache">是否把属性的委托加载到缓存</param>
        public static void SetPropertyValue<TEntity, TValue>(this PropertyInfo property, TEntity instance,
            TValue value, bool isCache = true)
            where TEntity : class
        {
            var propertyInfo = ReflectionFactory.Create<TEntity, TEntity, TValue>(property, property, isCache);

            propertyInfo.SetValue(instance, value);
        }

        /// <summary>
        /// 获取属性值的扩展
        /// </summary>
        /// <typeparam name="TEntity">实例的类型</typeparam>
        /// <typeparam name="TValue">值的类型</typeparam>
        /// <param name="property">需要获取的属性</param>
        /// <param name="instance">实例对象</param>
        /// <param name="isCache">是否把属性的委托加载到缓存</param>
        /// <returns>获取的值</returns>
        public static TValue GetPropertyValue<TEntity, TValue>(this PropertyInfo property, TEntity instance, bool isCache = true)
             where TEntity : class
        {
            var propertyInfo = ReflectionFactory.Create<TEntity, TEntity, TValue>(property, property, isCache);

            return propertyInfo.GetValue(instance);
        }

        /// <summary>
        /// 获取值再设定值
        /// </summary>
        /// <typeparam name="TFrom">源实例的类型</typeparam>
        /// <typeparam name="TTo">目标实例的类型</typeparam>
        /// <typeparam name="TValue">值类型</typeparam>
        /// <param name="getProperty">获取属性</param>
        /// <param name="setProperty">设定属性</param>
        /// <param name="fromEntity">源实例</param>
        /// <param name="toEntity">目标实例</param>
        /// <param name="isCache">是否把属性的委托加载到缓存</param>
        public static void GetAndSetProperty<TFrom, TTo, TValue>(this TFrom fromEntity, TTo toEntity, PropertyInfo getProperty, PropertyInfo setProperty,
             bool isCache)
            where TFrom : class
            where TTo : class
        {
            var propertyAccessor = ReflectionFactory.Create<TFrom, TTo, TValue>(getProperty, setProperty, isCache);
            propertyAccessor.SetValue(toEntity, propertyAccessor.GetValue(fromEntity));
        }
    }
}
