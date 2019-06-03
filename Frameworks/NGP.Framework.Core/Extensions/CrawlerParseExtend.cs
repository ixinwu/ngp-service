/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * CrawlerParseExtend Description:
 * 爬虫解析扩展
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-6-4   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NGP.Framework.Core
{
    /// <summary>
    /// 爬虫解析扩展
    /// </summary>
    public class CrawlerParseExtend
    {
        /// <summary>
        /// 获取实体表达式
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public static string GetEntityExpression<TEntity>() where TEntity : BaseEntity, new()
        {
            var entityAttribute = (typeof(TEntity)).GetCustomAttribute<NGPCrawlerEntityAttribute>();
            if (entityAttribute == null || string.IsNullOrWhiteSpace(entityAttribute.XPath))
                throw new Exception("This entity should be xpath");

            return entityAttribute.XPath;
        }

        /// <summary>
        /// 获取属性特性
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public static Dictionary<string, Tuple<CrawlerSelectorType, string>> GetPropertyAttributes<TEntity>()
            where TEntity : BaseEntity, new()
        {
            var attributeDictionary = new Dictionary<string, Tuple<CrawlerSelectorType, string>>();

            PropertyInfo[] props = typeof(TEntity).GetProperties();
            var propList = props.Where(p => p.CustomAttributes.Count() > 0);

            foreach (PropertyInfo prop in propList)
            {
                var attr = prop.GetCustomAttribute<NGPCrawlerFieldAttribute>();
                if (attr != null)
                {
                    attributeDictionary.Add(prop.Name, Tuple.Create(attr.SelectorType, attr.Expression));
                }
            }
            return attributeDictionary;
        }

        /// <summary>
        /// 创建实体
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public static object CreateNewEntity<TEntity>() where TEntity : BaseEntity, new()
        {
            return new TEntity();
        }

        /// <summary>
        /// 设定属性
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="property"></param>
        /// <param name="value"></param>
        public static void TrySetProperty(object obj, string property, object value)
        {
            var prop = obj.GetType().GetProperty(property, BindingFlags.Public | BindingFlags.Instance);
            if (prop != null && prop.CanWrite)
                prop.SetValue(obj, value, null);
        }
    }
}
