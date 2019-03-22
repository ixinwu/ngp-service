/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved.
 * 
 * ResolveExtend Description:
 * 解析扩展
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019/2/19  hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using NGP.Framework.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NGP.Foundation.Service.Analysis
{
    /// <summary>
    /// 解析扩展
    /// </summary>
    public static class ResolveExtend
    {
        /// <summary>
        /// 获取代码的类型
        /// </summary>
        /// <param name="columnType">字段类型</param>
        /// <returns>代码类型</returns>
        public static Type GetCodeType(this FieldColumnType columnType)
        {
            switch (columnType)
            {
                // 日期被格式化成字符串
                case FieldColumnType.DateTime:
                case FieldColumnType.Date:
                case FieldColumnType.Time:
                    return typeof(DateTime?);
                case FieldColumnType.Integer:
                    return typeof(int?);
                case FieldColumnType.Decimal:
                    return typeof(decimal?);
                case FieldColumnType.Bool:
                    return typeof(bool?);
                case FieldColumnType.Bits:
                    return typeof(byte[]);
                case FieldColumnType.String:
                default:
                    return typeof(string);
            }
        }

        /// <summary>
        /// 获取主表单key
        /// </summary>
        /// <param name="fieldKeys"></param>
        /// <param name="relations"></param>
        /// <returns></returns>
        public static string GetMainFormKey(List<string> fieldKeys, List<App_Config_FormRelation> relations)
        {
            // 分析当前解析的主表
            var formKeys = fieldKeys.Select(s => AppConfigExtend.GetFormKey(s)).Distinct();
            var findFormKeys = formKeys.ToList();
            var mainKey = string.Empty;
            foreach (var item in formKeys)
            {
                var sourceKey = FindSourceKey(item, relations, findFormKeys);
                if (findFormKeys.Contains(sourceKey))
                {
                    mainKey = sourceKey;
                    break;
                }
            }
            return mainKey;
        }

        /// <summary>
        /// find source key
        /// </summary>
        /// <param name="mainKey"></param>
        /// <param name="relations"></param>
        /// <param name="formKeys"></param>
        /// <returns></returns>
        private static string FindSourceKey(string mainKey, List<App_Config_FormRelation> relations, List<string> formKeys)
        {
            // 取第一个源
            var sourceKey = relations
                .Where(s => s.RelationFormKey == mainKey && formKeys.Contains(s.SourceFormKey))
                .Select(s => s.SourceFormKey)
                .Distinct()
                .FirstOrDefault();
            if (string.IsNullOrWhiteSpace(sourceKey))
            {
                return mainKey;
            }
            formKeys.Remove(mainKey);
            return FindSourceKey(sourceKey, relations, formKeys);
        }
    }
}
