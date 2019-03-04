/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * EngineContext Description:
 * 引擎上下文
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-28   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using NGP.Framework.Core;
using NGP.Framework.DependencyInjection;
using System.Runtime.CompilerServices;

namespace NGP.Framework.WebApi.Core
{
    /// <summary>
    /// 引擎上下文
    /// </summary>
    public class EngineWebFactory
    {
        #region Methods

        /// <summary>
        /// 创建线程安全的单例引擎
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void Create()
        {
            if (Singleton<IEngine>.Instance == null)
            {
                Singleton<IEngine>.Instance = new NGPWebEngine();
            }
        }
        
        #endregion
    }
}
