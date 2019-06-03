/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * NGPCrawlerEntityAttribute Description:
 * 爬虫实体特性
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-6-4   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using System;

namespace NGP.Framework.Core
{
    /// <summary>
    /// 爬虫实体特性
    /// </summary>
    public class NGPCrawlerEntityAttribute : Attribute
    {
        /// <summary>
        /// xpath
        /// </summary>
        public string XPath { get; set; }
    }
}
