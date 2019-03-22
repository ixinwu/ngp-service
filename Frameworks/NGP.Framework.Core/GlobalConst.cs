/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * GlobalConst Description:
 * 全局常量
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2017/3/21 17:04:04   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/


namespace NGP.Framework.Core
{
    /// <summary>
    /// 全局常量
    /// </summary>
    public struct GlobalConst
    {
        /// <summary>
        /// 附件路径
        /// </summary>
        public const string __AttachmentFilesPath = "wwwroot\\attachments\\";

        /// <summary>
        /// 代码xml文档
        /// </summary>
        public const string __CodeXmlDocuments = "App_Data\\XmlDocuments\\";

        /// <summary>
        /// 解析json文档
        /// </summary>
        public const string __ResolveJsons = "App_Data\\ResolveJsons\\";

        /// <summary>
        /// swagger sjon
        /// </summary>
        public const string __SwaggerJson = "/swagger/v1/swagger.json";

        /// <summary>
        /// 系统api名称
        /// </summary>
        public const string __ApiName = "NGP API";

        /// <summary>
        /// 日期格式常量
        /// </summary>
        public struct DateFormatConst
        {
            /// <summary>
            /// 日期格式
            /// </summary>
            public const string __DateFormat = "yyyy-MM-dd";

            /// <summary>
            /// 时间格式
            /// </summary>
            public const string __DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            /// <summary>
            /// 时间格式
            /// </summary>
            public const string __DateTimeFormatNotBar = "yyyyMMddHHmmss";
        }

        /// <summary>
        /// 策略数据映射key
        /// </summary>
        public const string __ServiceDataMapKey = "ServiceDataMapKey";

        /// <summary>
        /// 正则表达式常量
        /// </summary>
        public struct RegexConst
        {
            /// <summary>
            /// 匹配字段key
            /// </summary>
            public const string __FieldKeyRule = @"(([a-zA-Z]([a-zA-Z]|\d)*)*[_]){2}[a-zA-Z]([a-zA-Z]|\d)*";
        }
    }
}
