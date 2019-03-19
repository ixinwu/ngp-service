/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved.
 * 
 * DynamicBaseRequest Description:
 * 动态请求基类
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019/2/19  hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using NGP.Framework.Core;

namespace NGP.Foundation.Service.Analysis
{
    /// <summary>
    /// 动态请求基类
    /// </summary>
    public abstract class DynamicBaseRequest : INGPRequest
    {
        /// <summary>
        /// data set key
        /// </summary>
        public string DataSetKey { get; set; }

        /// <summary>
        /// 资源key(用于鉴权)
        /// </summary>
        public string ResourceKey { get; set; }

        /// <summary>
        /// 主表key
        /// </summary>
        public string MainFormKey { get; set; }
    }
}
