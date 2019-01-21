/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * ExceptionCallHandlerAttribute Description:
 * 异常特性拦截
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-15   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using System;

namespace NGP.Framework.Core
{
    /// <summary>
    /// 异常特性拦截
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class ExceptionCallHandlerAttribute : Attribute
    {
    }
}
