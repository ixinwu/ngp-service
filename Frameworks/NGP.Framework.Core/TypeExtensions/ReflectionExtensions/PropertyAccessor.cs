/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * PropertyAccessor Description:
 * 属性访问器(object通过构造表达式树，其他类型通过构造委托)
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-15   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace NGP.Framework.Core
{
    /// <summary>
    /// 属性访问器(object通过构造表达式树，其他类型通过构造委托)
    /// </summary>
    /// <typeparam name="TFrom"></typeparam>
    /// <typeparam name="TTo"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class PropertyAccessor<TFrom, TTo, TValue> : IPropertyAccessor<TFrom, TTo, TValue>
        where TFrom : class
        where TTo : class
    {
        #region Fields
        /// <summary>
        /// get方法委托
        /// </summary>
        private Func<TFrom, TValue> _getterFunc;

        /// <summary>
        /// set方法委托
        /// </summary>
        private Action<TTo, TValue> _setterAction;

        /// <summary>
        /// 当前Get属性
        /// </summary>
        private readonly PropertyInfo _getProperty;

        /// <summary>
        /// 当前Set属性
        /// </summary>
        private readonly PropertyInfo _setProperty;
        #endregion

        #region Ctor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="getProperty"></param>
        /// <param name="setProperty"></param>
        public PropertyAccessor(PropertyInfo getProperty, PropertyInfo setProperty)
        {
            this._getProperty = getProperty;
            this._setProperty = setProperty;
        }
        #endregion

        #region Methods
        #region Inner Methods
        /// <summary>
        /// 加载Get委托
        /// </summary>
        private void InitializeGet()
        {
            if (!_getProperty.CanRead)
            {
                return;
            }
            if (typeof(TValue) != typeof(object))
            {
                // 如果可以确定类型则直接创建委托
                this._getterFunc = Delegate.CreateDelegate(typeof(Func<TFrom, TValue>), _getProperty.GetGetMethod())
                    as Func<TFrom, TValue>;
            }
            else
            {
                // 添加实例对象表达式
                var instance = Expression.Parameter(typeof(TFrom), "instance");

                // 实例属性的访问表达式
                var instanceCast = Expression.Convert(instance, _getProperty.ReflectedType);

                // 属性的访问表达式
                var propertyAccess = Expression.Property(instanceCast, _getProperty);

                // 属性值的访问表达式
                var castPropertyValue = Expression.Convert(propertyAccess, typeof(TValue));

                // 生成表达式树
                var lambda = Expression.Lambda<Func<TFrom, TValue>>(castPropertyValue, instance);

                // 编译表达式树
                this._getterFunc = lambda.Compile();
            }
        }

        /// <summary>
        /// 加载Set委托
        /// </summary>
        private void InitializeSet()
        {
            if (!_setProperty.CanWrite)
            {
                return;
            }

            if (typeof(TValue) != typeof(object))
            {
                // 如果可以确定类型则直接创建委托
                this._setterAction = Delegate.CreateDelegate(typeof(Action<TTo, TValue>), _setProperty.GetSetMethod())
                    as Action<TTo, TValue>;
            }
            else
            {
                // 获取Set方法
                var methodInfo = _setProperty.GetSetMethod();

                // 加载对象
                var instanceParameter = Expression.Parameter(typeof(TTo), "instance");
                var parametersParameter = Expression.Parameter(typeof(TValue), "parameters");

                // 添加参数
                var paramInfos = methodInfo.GetParameters().FirstOrDefault();
                UnaryExpression valueCast = Expression.Convert(parametersParameter, paramInfos.ParameterType);

                // 添加对象的访问
                var instanceCast = Expression.Convert(instanceParameter, methodInfo.ReflectedType);

                // Set方法的访问
                var methodCall = Expression.Call(instanceCast, methodInfo, valueCast);

                // 创建表达式树
                var lambda = Expression.Lambda<Action<TTo, TValue>>(
                            methodCall, instanceParameter, parametersParameter);

                // 编译表达式树
                this._setterAction = lambda.Compile();
            }
        }
        #endregion

        #region Interface Methods
        /// <summary>
        /// 取值方法
        /// </summary>
        /// <param name="instance">当前对象</param>
        /// <returns>获取的值</returns>
        public TValue GetValue(TFrom instance)
        {
            if (this._getterFunc == null)
            {
                this.InitializeGet();
                if (this._getterFunc == null)
                {
                    throw new ArgumentNullException("Get method is not defined for this property.");
                }
            }
            return this._getterFunc(instance);
        }

        /// <summary>
        /// 设定值方法
        /// </summary>
        /// <param name="instance">当前对象</param>
        /// <param name="value">需要设定的值</param>
        public void SetValue(TTo instance, TValue value)
        {
            if (this._setterAction == null)
            {
                this.InitializeSet();
                if (this._setterAction == null)
                {
                    throw new ArgumentNullException("Set method is not defined for this property.");
                }
            }
            this._setterAction(instance, value);
        }
        #endregion
        #endregion
    }
}
