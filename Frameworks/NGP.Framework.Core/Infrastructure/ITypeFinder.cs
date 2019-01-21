/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * ITypeFinder Description:
 * 类型查找接口
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-15   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Reflection;

namespace NGP.Framework.Core
{
    /// <summary>
    /// ngp类型查找器
    /// </summary>
    public interface ITypeFinder
    {
        /// <summary>
        /// 查找类型
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="onlyConcreteClasses">指示是否仅查找具体类的值</param>
        /// <returns>Result</returns>
        IEnumerable<Type> FindClassesOfType<T>(bool onlyConcreteClasses = true);

        /// <summary>
        /// 查找类型
        /// </summary>
        /// <param name="assignTypeFrom"></param>
        /// <param name="onlyConcreteClasses">指示是否仅查找具体类的值</param>
        /// <returns>Result</returns>
        /// <returns></returns>
        IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, bool onlyConcreteClasses = true);

        /// <summary>
        /// 查找类型
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="assemblies">Assemblies</param>
        /// <param name="onlyConcreteClasses">指示是否仅查找具体类的值</param>
        /// <returns>Result</returns>
        IEnumerable<Type> FindClassesOfType<T>(IEnumerable<Assembly> assemblies, bool onlyConcreteClasses = true);

        /// <summary>
        /// 查找类型
        /// </summary>
        /// <param name="assignTypeFrom">Assign type from</param>
        /// <param name="assemblies">Assemblies</param>
        /// <param name="onlyConcreteClasses">指示是否仅查找具体类的值</param>
        /// <returns>Result</returns>
        IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, IEnumerable<Assembly> assemblies, bool onlyConcreteClasses = true);

        /// <summary>
        /// 获取与当前实现相关的程序集。
        /// </summary>
        /// <returns></returns>
        IList<Assembly> GetAssemblies();
    }
}
