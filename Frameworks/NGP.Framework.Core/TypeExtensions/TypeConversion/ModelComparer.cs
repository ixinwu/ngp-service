/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * ModelComparer Description:
 * 模型比较器
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-15   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;

namespace NGP.Framework.Core
{
    /// <summary>
    /// 模型比较器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TEqualValue"></typeparam>
    public class ModelComparer<T, TEqualValue> : IEqualityComparer<T> where T : class
    {
        /// <summary>
        /// 获取值委托
        /// </summary>
        private readonly Func<T, TEqualValue> _funcValue;

        /// <summary>
        ///  Ctor
        /// </summary>
        /// <param name="func"></param>
        public ModelComparer(Func<T,TEqualValue> func)
        {
            _funcValue = func;
        }

        /// <summary>
        /// 比较对象是否一致
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool Equals(T x, T y)
        {
            if (x == null || y == null)
            {
                return false;
            }
            return _funcValue(x).ConvertToString() == _funcValue(y).ConvertToString();
        }

        /// <summary>
        /// 重载hashcode
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int GetHashCode(T obj)
        {
            return _funcValue(obj).ConvertToString().GetHashCode();
        }
    }
}
