/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * CrawlerSelectorType Description:
 * 
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-6-4   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

namespace NGP.Framework.Core
{
    /// <summary>
    /// 爬虫选择器类型
    /// </summary>
    public enum CrawlerSelectorType
    {
        /// <summary>
        /// xpath
        /// </summary>
        XPath,
        /// <summary>
        /// css
        /// </summary>
        CssSelector,
        /// <summary>
        /// fixed
        /// </summary>
        FixedValue
    }
}
