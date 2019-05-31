/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * MongoConfig Description:
 * mongo配置
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-5-30   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

namespace NGP.Framework.Core
{
    /// <summary>
    /// mongo配置
    /// </summary>
    public partial class MongoConfig
    {
        /// <summary>
        /// 连接串
        /// </summary>
        public string Connection { get; set; }
        /// <summary>
        /// 集合名称
        /// </summary>
        public string DatabaseName { get; set; }
    }
}