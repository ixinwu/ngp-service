/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * App_Config_FormField Description:
 * 应用表单字段配置表
 *
 * Comment 					        Revision	        Date                 Author
 * -----------------------------    --------         --------            -----------
 * Created							1.0		    2019/3/7 14:49:49   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

namespace NGP.Framework.Core
{
    /// <summary>
    /// 应用表单字段配置表
    /// </summary>
    public class App_Config_FormField : BaseDBEntity
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
        /// 字段key
        /// </summary>
        public string FieldKey { get; set; }

        /// <summary>
        /// 字段名称
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// 字段类型
        /// </summary>
        public string FieldType { get; set; }

        /// <summary>
        /// 是否系统字段
        /// </summary>
        public bool? IsSystemField { get; set; }

        /// <summary>
        /// DB配置
        /// </summary>
        public string DbConfig { get; set; }

        /// <summary>
        /// 验证配置
        /// </summary>
        public string VerificationConfig { get; set; }

        /// <summary>
        /// 业务配置
        /// </summary>
        public string BusinessConfig { get; set; }
    }
}