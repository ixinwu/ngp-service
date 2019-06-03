/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * NGPCrawlerFieldAttribute Description:
 * 
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
    /// 爬虫字段特性
    /// </summary>
    public class NGPCrawlerFieldAttribute : Attribute
    {
        /// <summary>
        /// 选择表达式
        /// </summary>
        public string Expression { get; set; }

        /// <summary>
        /// 选择类型
        /// </summary>
        public CrawlerSelectorType SelectorType { get; set; }
    }
}
