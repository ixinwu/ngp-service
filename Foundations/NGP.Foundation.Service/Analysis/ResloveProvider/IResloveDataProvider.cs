/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * IDynamicDataService Description:
 * 动态数据服务接口
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
    /// 动态数据服务接口
    /// </summary>
    public interface IResloveDataProvider
    {
        /// <summary>
        /// 初始化解析上下文
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ResloveInitContext InitResloveContext(DynamicBaseRequest request);
    }
}
