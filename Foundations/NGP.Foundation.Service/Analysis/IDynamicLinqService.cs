/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * IDynamicLinqService Description:
 * 动态linq执行服务接口
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019/1/22 9:07:18   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using NGP.Framework.Core;
using System;
using System.Collections.Generic;

namespace NGP.Foundation.Service.Analysis
{
    /// <summary>
    /// 动态linq执行服务接口
    /// </summary>
    public interface IDynamicLinqService
    {
        /// <summary>
        /// 动态linq执行
        /// </summary>
        /// <param name="request">请求对象</param>
        /// <returns>查询结果</returns>
        NGPResponse<dynamic> Extute(DynamicLinqRequest request);
    }
}
