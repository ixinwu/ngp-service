/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * IPropertyAccessor Description:
 * 属性访问接口
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-15   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/


namespace NGP.Framework.Core
{
    /// <summary>
    /// 属性访问接口
    /// </summary>
    /// <typeparam name="TFrom"></typeparam>
    /// <typeparam name="TTo"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public interface IPropertyAccessor<TFrom, TTo, TValue> 
        where TFrom : class
        where TTo : class
    {
        /// <summary>
        /// 获取值方法
        /// </summary>
        /// <param name="instance">取值对象</param>
        /// <returns>获取的值</returns>
        TValue GetValue(TFrom instance);

        /// <summary>
        /// 设定值方法
        /// </summary>
        /// <param name="instance">取值对象</param>
        /// <param name="value">需要设定的值</param>
        void SetValue(TTo instance, TValue value);
    }
}
