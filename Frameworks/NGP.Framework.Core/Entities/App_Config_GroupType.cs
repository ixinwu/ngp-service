/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * App_Config_GroupType Description:
 * 应用组类别配置
 *
 * Comment 					        Revision	        Date                 Author
 * -----------------------------    --------         --------            -----------
 * Created							1.0		    2019/3/7 14:49:49   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

namespace NGP.Framework.Core
{
    /// <summary>
    /// 应用组类别配置
    /// </summary>
    public class App_Config_GroupType : BaseDBEntity
    {
        /// <summary>
        /// 类别组key
        /// </summary>
        public string GroupKey { get; set; }
        /// <summary>
        /// 类别key
        /// </summary>
        public string TypeKey { get; set; }
        /// <summary>
        /// 类别值
        /// </summary>
        public string TypeValue { get; set; }
        /// <summary>
        /// 归属应用key
        /// </summary>
        public string AscriptionAppKey { get; set; }
    }
}