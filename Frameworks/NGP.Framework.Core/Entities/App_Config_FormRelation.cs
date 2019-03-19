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
        /// 源应用key
        /// </summary>
        public string SourceAppKey { get; set; }
        /// <summary>
        /// 源表单key
        /// </summary>
        public string SourceFormKey { get; set; }
        /// <summary>
        /// 源字段key
        /// </summary>
        public string SourceFieldKey { get; set; }

        /// <summary>
        /// 关联应用key
        /// </summary>
        public string RelationAppKey { get; set; }
        /// <summary>
        /// 关联表单key
        /// </summary>
        public string RelationFormKey { get; set; }
        /// <summary>
        /// 关联字段key
        /// </summary>
        public string RelationFieldKey { get; set; }
    }
}