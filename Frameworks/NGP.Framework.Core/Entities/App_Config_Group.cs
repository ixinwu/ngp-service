/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * App_Config_Group Description:
 * 应用组配置
 *
 * Comment 					        Revision	        Date                 Author
 * -----------------------------    --------         --------            -----------
 * Created							1.0		    2019/3/7 14:49:49   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

namespace NGP.Framework.Core
{
    /// <summary>
    /// 应用组配置
    /// </summary>
    public class App_Config_Group : BaseDBEntity
    {
        /// <summary>
        /// 类别组key
        /// </summary>
        public string GroupKey { get; set; }
        /// <summary>
        /// 类别组值
        /// </summary>
        public string GroupValue { get; set; }
        /// <summary>
        /// 归属应用key
        /// </summary>
        public string AscriptionAppKey { get; set; }
    }
}