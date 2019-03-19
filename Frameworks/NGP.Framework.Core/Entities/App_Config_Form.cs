/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * App_Config_Form Description:
 * 应用表配置
 *
 * Comment 					        Revision	        Date                 Author
 * -----------------------------    --------         --------            -----------
 * Created							1.0		    2019/3/7 14:49:49   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

namespace NGP.Framework.Core
{
    /// <summary>
    /// 应用表配置
    /// </summary>
    public class App_Config_Form : BaseDBEntity
    {
        /// <summary>
        /// 应用key
        /// </summary>
        public string AppKey { get; set; }
        /// <summary>
        /// 表单key
        /// </summary>
        public string FormKey { get; set; }
        /// <summary>
        /// 表单名称
        /// </summary>
        public string FormName { get; set; }

        /// <summary>
        /// 表单验证配置
        /// </summary>
        public FormExtendConfig ExtendConfig { get; set; }
    }
}