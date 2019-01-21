/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * Singleton Description:
 * 单件管理(泛型)
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-15   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using System.Threading;

namespace NGP.Framework.Core
{
    /// <summary>
    /// 单件管理(泛型)
    /// </summary>
    public class Singleton<T> : BaseSingleton
    {
        /// <summary>
        /// 实例
        /// </summary>
        private static T instance;

        /// <summary>
        /// 指定类型t的单一实例。对于每种类型的t，仅此对象的一个实例（每次）。
        /// </summary>
        public static T Instance
        {
            get => instance;
            set
            {
                instance = value;
                AllSingletons[typeof(T)] = value;
            }
        }
    }
}
