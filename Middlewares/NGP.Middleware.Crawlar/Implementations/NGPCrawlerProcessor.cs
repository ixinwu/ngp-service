/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * NGPCrawlerProcessor Description:
 * ngp爬虫执行器
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-6-3   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;
using NGP.Framework.Core;
using System.Collections.Generic;

namespace NGP.Middleware.Crawlar
{
    /// <summary>
    /// ngp爬虫执行器
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class NGPCrawlerProcessor<TEntity> : INGPCrawlerProcessor<TEntity> where TEntity : BaseEntity, new()
    {
        /// <summary>
        /// 执行解析
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> Process(HtmlDocument document)
        {
            var nameValueDictionary = GetColumnNameValuePairsFromHtml(document);

            var processorEntity = CrawlerParseExtend.CreateNewEntity<TEntity>();
            foreach (var pair in nameValueDictionary)
            {
                CrawlerParseExtend.TrySetProperty(processorEntity, pair.Key, pair.Value);
            }

            return new List<TEntity>
            {
                processorEntity as TEntity
            };
        }

        /// <summary>
        /// 获取列解析
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        private static Dictionary<string, object> GetColumnNameValuePairsFromHtml(HtmlDocument document)
        {
            var columnNameValueDictionary = new Dictionary<string, object>();

            var entityExpression = CrawlerParseExtend.GetEntityExpression<TEntity>();
            var propertyExpressions = CrawlerParseExtend.GetPropertyAttributes<TEntity>();

            var entityNode = document.DocumentNode.SelectSingleNode(entityExpression);

            foreach (var expression in propertyExpressions)
            {
                var columnName = expression.Key;
                object columnValue = null;
                var fieldExpression = expression.Value.Item2;

                switch (expression.Value.Item1)
                {
                    case CrawlerSelectorType.XPath:
                        var node = entityNode.SelectSingleNode(fieldExpression);
                        if (node != null)
                            columnValue = node.InnerText;
                        break;
                    case CrawlerSelectorType.CssSelector:
                        var nodeCss = entityNode.QuerySelector(fieldExpression);
                        if (nodeCss != null)
                            columnValue = nodeCss.InnerText;
                        break;
                    case CrawlerSelectorType.FixedValue:
                        if (int.TryParse(fieldExpression, out var result))
                        {
                            columnValue = result;
                        }
                        break;
                    default:
                        break;
                }
                columnNameValueDictionary.Add(columnName, columnValue);
            }

            return columnNameValueDictionary;
        }
    }
}
