/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * IEngine Description:
 * 引擎接口
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-20   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace NGP.Framework.Core
{
    /// <summary>
    /// 引擎接口
    /// 实现这个接口的类可以作为组成ngp引擎的各种服务的门户。编辑功能、模块和实现通过此接口访问大多数ngp功能。
    /// </summary>
    public interface IEngine
    {
        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="services"></param>
        /// <param name="typeFinder"></param>
        /// <param name="registerList"></param>
        /// <returns></returns>
        IServiceProvider RegisterDependencies(IServiceCollection services, ITypeFinder typeFinder, params NGPKeyValuePair<Type, object>[] registerList);

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns>Service provider</returns>
        IServiceProvider Initialize(IServiceCollection services, IConfiguration configuration);

        /// <summary>
        /// 获取实例
        /// </summary>
        /// <typeparam name="T">Type of resolved service</typeparam>
        /// <returns>Resolved service</returns>
        T Resolve<T>() where T : class;

        /// <summary>
        /// 通过类型获取实例
        /// </summary>
        /// <param name="type">Type of resolved service</param>
        /// <returns>Resolved service</returns>
        object Resolve(Type type);

        /// <summary>
        /// 获取实例列表
        /// </summary>
        /// <typeparam name="T">Type of resolved services</typeparam>
        /// <returns>Collection of resolved services</returns>
        IEnumerable<T> ResolveAll<T>();

        /// <summary>
        /// 解析未注册的类型
        /// </summary>
        /// <param name="type">Type of service</param>
        /// <returns>Resolved service</returns>
        object ResolveUnregistered(Type type);
    }
}
