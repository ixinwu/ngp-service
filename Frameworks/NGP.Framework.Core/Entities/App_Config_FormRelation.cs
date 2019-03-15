/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * App_Config_FormRelation Description:
 * 应用表关联配置
 *
 * Comment 					        Revision	        Date                 Author
 * -----------------------------    --------         --------            -----------
 * Created							1.0		    2019/3/7 14:49:49   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

namespace NGP.Framework.Core
{
    /// <summary>
    /// 应用表关联配置
    /// </summary>
    public class App_Config_FormRelation : BaseDBEntity
    {
        /// <summary>
        /// 主应用key
        /// </summary>
        public string MasterAppKey { get; set; }
        /// <summary>
        /// 主表单key
        /// </summary>
        public string MasterFormKey { get; set; }
        /// <summary>
        /// 主字段key
        /// </summary>
        public string MasterFieldKey { get; set; }

        /// <summary>
        /// 从应用key
        /// </summary>
        public string SlaveAppKey { get; set; }
        /// <summary>
        /// 从表单key
        /// </summary>
        public string SlaveFormKey { get; set; }
        /// <summary>
        /// 从字段key
        /// </summary>
        public string SlaveFieldKey { get; set; }
    }
}