/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * BaseSingleton Description:
 * 基类单例
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
    /// 基类单例
    /// </summary>
    public class BaseSingleton
    {
        /// <summary>
        /// ctor
        /// </summary>
        static BaseSingleton()
        {
            AllSingletons = new Dictionary<Type, object>();
        }

        /// <summary>
        /// 类型为singleton实例的字典。
        /// </summary>
        public static IDictionary<Type, object> AllSingletons { get; }
    }
}