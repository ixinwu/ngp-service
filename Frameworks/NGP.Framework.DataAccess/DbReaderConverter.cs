/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * DbReaderConverter Description:
 * DbReaderConverter
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-15   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace NGP.Framework.DataAccess
{
    /// <summary>
    /// DbReaderConverter
    /// </summary>
    internal class DbReaderConverter<T> where T : class, new()
    {
        /// <summary>
        /// dataReader
        /// </summary>
        private readonly IDataReader dataReader;
        /// <summary>
        /// 转换action
        /// </summary>
        private readonly Action<IDataReader, T> _convertAction;
        /// <summary>
        /// 缓存转换表达式树
        /// </summary>
        private static readonly ConcurrentDictionary<Type, object> _convertActionMap = new ConcurrentDictionary<Type, object>();

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="dataReader"></param>
        internal DbReaderConverter(IDataReader dataReader)
        {
            this.dataReader = dataReader;
            _convertAction = (Action<IDataReader, T>)_convertActionMap.GetOrAdd(
                typeof(T), (t) => GetMapFunc());
        }

        /// <summary>
        /// 构建映射表达式树
        /// </summary>
        /// <returns></returns>
        private Action<IDataReader, T> GetMapFunc()
        {
            var exps = new List<Expression>();

            var paramExp = Expression.Parameter(typeof(IDataRecord), "o7thDR");
            var targetExp = Expression.Parameter(typeof(T), "o7thTarget");

            var getPropInfo = typeof(IDataRecord).GetProperty("Item", new[] { typeof(string) });

            var columnNames = Enumerable.Range(0, dataReader.FieldCount)
                                        .Select(x => dataReader.GetName(x));
            foreach (var columnName in columnNames)
            {
                var property = typeof(T).GetProperty(columnName);
                if (property == null)
                    continue;

                var columnNameExp = Expression.Constant(columnName);
                var getPropExp = Expression.MakeIndex(
                    paramExp, getPropInfo, new[] { columnNameExp });
                var castExp = Expression.TypeAs(getPropExp, property.PropertyType);
                var bindExp = Expression.Assign(
                    Expression.Property(targetExp, property), castExp);
                exps.Add(bindExp);
            }

            return Expression.Lambda<Action<IDataReader, T>>(
                Expression.Block(exps), paramExp, targetExp).Compile();
        }

        /// <summary>
        /// 转换调用
        /// </summary>
        /// <returns></returns>
        internal T CreateItemFromRow()
        {
            T result = new T();
            _convertAction(dataReader, result);
            return result;
        }
    }
}
