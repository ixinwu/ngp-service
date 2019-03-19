﻿/* ---------------------------------------------------------------------    
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
        public const string __ResloveJsons = "App_Data\\ResloveJsons\\";

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
    }
}