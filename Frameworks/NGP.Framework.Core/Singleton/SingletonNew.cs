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
    public class SingletonNew<T> where T : class, new()
    {
        /// <summary>
        /// 加锁字段
        /// </summary>
        private static readonly object lockObject = new object();

        /// <summary>
        /// 当前实例
        /// </summary>
        private static T currentValue;

        /// <summary>
        /// 返回当前上下文的单例
        /// </summary>
        public static T Instance
        {
            get
            {
                // 如果对象已经创建，则直接返回
                if (currentValue != null)
                {
                    return currentValue;
                }

                // 如果没有创建，则让一个线程创建
                lock (lockObject)
                {
                    if (currentValue == null)
                    {
                        // 仍未创建，创建它
                        var temp = new T();

                        // 将引用保存到currentValue中
                        Interlocked.Exchange(ref currentValue, temp);
                    }
                }

                // 返回对象的引用
                return currentValue;
            }
            set
            {
                lock (lockObject)
                {
                    // 将引用保存到currentValue中
                    Interlocked.Exchange(ref currentValue, value);
                }
            }
        }
    }
}
