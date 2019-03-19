/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * AppConfigExtend Description:
 * app配置扩展
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-3-15   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using System.Linq;
using System.Linq.Dynamic.Core;

namespace NGP.Framework.Core
{
    /// <summary>
    /// app配置扩展
    /// </summary>
    public static class AppConfigExtend
    {
        /// <summary>
        /// 获取字段名称key
        /// </summary>
        /// <param name="fieldKey"></param>
        /// <returns></returns>
        public static string GetFieldNameKey(string fieldKey)
        {
            return string.Format("{0}__Name", fieldKey);
        }

        /// <summary>
        /// 获取表单key
        /// </summary>
        /// <param name="fieldKey"></param>
        /// <returns></returns>
        public static string GetFormKey(string fieldKey)
        {
            var column = GetColumn(fieldKey);
            var formKey = fieldKey.Replace(string.Format("_{0}", column), "");
            return formKey;
        }

        /// <summary>
        /// 获取列名
        /// </summary>
        /// <param name="fieldKey"></param>
        /// <returns></returns>
        public static string GetColumn(string fieldKey)
        {
            var column = fieldKey.Split("_").LastOrDefault();
            return column;
        }

        /// <summary>
        /// 获取sql完整列名
        /// </summary>
        /// <param name="fieldKey"></param>
        /// <returns></returns>
        public static string GetSqlFullName(string fieldKey)
        {
            return string.Format("{0}.[{1}]", GetFormKey(fieldKey), GetColumn(fieldKey));
        }
    }
}
