/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * SqlServerExtension Description:
 * 操作sqlserver数据库扩展
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-15   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using NGP.Framework.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace NGP.Framework.DataAccess
{
    /// <summary>
    /// 操作sqlserver数据库扩展
    /// </summary>
    public static class DbReaderExtension
    {
        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="dbCommand"></param>
        /// <param name="parameters"></param>
        public static void SetParameters(this IDbCommand dbCommand, IDictionary<string, object> parameters)
        {
            if (parameters == null)
            {
                return;
            }
            foreach (var parameter in parameters)
            {
                if (parameter.Value != null)
                {
                    dbCommand.Parameters.Add(new SqlParameter(parameter.Key, parameter.Value));
                    continue;
                }

                dbCommand.Parameters.Add(new SqlParameter(parameter.Key, DBNull.Value));
            }
        }

        /// <summary>
        /// 根据列名获取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rdr"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static T GetValue<T>(this IDataReader rdr, string columnName)
        {
            try
            {
                int index = rdr.GetOrdinal(columnName);
                if (rdr.IsDBNull(index))
                {
                    return default(T);
                }
                return (Convert.ToString(rdr[index]).To<T>());
            }
            catch
            {
                return default(T);
            }
        }

        /// <summary>
        /// 获取reader对应的实体
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="rdr"></param>
        /// <returns></returns>
        public static TEntity GetReaderData<TEntity>(this IDataReader rdr) where TEntity : class, new()
        {
            var item = new TEntity();
            var filedList = new List<string>();
            for (int i = 0; i < rdr.FieldCount; i++)
            {
                filedList.Add(rdr.GetName(i).ToUpper());
            }
            var properties = typeof(TEntity).GetProperties().Where(s => filedList.Contains(s.Name.ToUpper()));
            foreach (var property in properties)
            {
                item.SetValue(property, rdr);
            }
            return item;
        }

        /// <summary>
        /// 获取reader对应的实体
        /// </summary>
        /// <param name="rdr"></param>
        /// <param name="type">类型</param>
        /// <param name="valueFormatters">值格式化列表</param>
        /// <param name="propertyMapping">属性映射</param>
        /// <returns></returns>
        public static object GetReaderData(this IDataReader rdr, Type type,
            IDictionary<string, Func<object, object>> valueFormatters = null,
            IDictionary<string, string> propertyMapping = null)
        {
            var item = Activator.CreateInstance(type);
            var filedList = new List<string>();
            for (int i = 0; i < rdr.FieldCount; i++)
            {
                filedList.Add(rdr.GetName(i).ToUpper());
            }

            var properties = (from s in type.GetProperties()
                              let name = s.Name.ToUpper()
                              where filedList.Contains(name)
                              select new
                              {
                                  ColumnName = name,
                                  Property = s
                              }).ToList();
            if (propertyMapping != null)
            {
                var addProperties = (from s in type.GetProperties()
                                     let propertyName = s.Name
                                     let columnName = propertyMapping.GetVlaue(propertyName)
                                     where filedList.Contains(columnName)
                                     select new
                                     {
                                         ColumnName = columnName,
                                         Property = s
                                     }).ToList();
                properties.AddRange(addProperties);
                properties = properties.Distinct().ToList();
            }
            foreach (var property in properties)
            {
                item.SetValue(property.Property, rdr, valueFormatters, property.ColumnName);
            }
            return item;
        }

        /// <summary>
        /// 设定字典对象值
        /// </summary>
        /// <param name="result">设定值结果</param>
        /// <param name="rdr">读取源</param>
        public static void SetReaderValue(this IDictionary<string, object> result, IDataReader rdr)
        {
            for (int i = 0; i < rdr.FieldCount; i++)
            {
                result[rdr.GetName(i).ToUpper()] = rdr[i];
            }
        }

        /// <summary>
        /// 设定值
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="property"></param>
        /// <param name="rdr"></param>
        /// <param name="valueFormatters">值格式化列表</param>
        /// <param name="readName">读取名称</param>
        public static void SetValue<TEntity>(this TEntity entity, PropertyInfo property, IDataReader rdr,
            IDictionary<string, Func<object, object>> valueFormatters = null, string readName = null)
            where TEntity : class, new()
        {
            readName = readName ?? property.Name;

            // 如果有格式化对象则直接返回
            var valueFormatter = valueFormatters.GetVlaue(readName);
            if (valueFormatter != null)
            {
                var currentValue = rdr.GetValue<object>(readName);
                var formatterValue = valueFormatter(currentValue);
                property.SetValue(entity, formatterValue);
                return;
            }

            if (property.PropertyType.Equals(typeof(string)))
            {
                var value = rdr.GetValue<string>(readName);

                if (string.IsNullOrWhiteSpace(value))
                {
                    return;
                }
                if (typeof(TEntity) == typeof(object))
                {
                    property.SetValue(entity, value);
                    return;
                }
                property.SetPropertyValue(entity, value);

            }
            else if (property.PropertyType.Equals(typeof(int)))
            {
                var value = rdr.GetValue<int>(readName);
                if (value == 0)
                {
                    return;
                }
                if (typeof(TEntity) == typeof(object))
                {
                    property.SetValue(entity, value);
                    return;
                }
                property.SetPropertyValue(entity, value);
            }
            else if (property.PropertyType.Equals(typeof(int?)))
            {
                var value = rdr.GetValue<int?>(readName);
                if (value == null)
                {
                    return;
                }
                if (typeof(TEntity) == typeof(object))
                {
                    property.SetValue(entity, value);
                    return;
                }
                property.SetPropertyValue(entity, value);
            }
            else if (property.PropertyType.Equals(typeof(long)))
            {
                var value = rdr.GetValue<long>(readName);
                if (value == 0)
                {
                    return;
                }
                if (typeof(TEntity) == typeof(object))
                {
                    property.SetValue(entity, value);
                    return;
                }
                property.SetPropertyValue(entity, value);
            }
            else if (property.PropertyType.Equals(typeof(long?)))
            {
                var value = rdr.GetValue<long?>(readName);
                if (value == null)
                {
                    return;
                }
                if (typeof(TEntity) == typeof(object))
                {
                    property.SetValue(entity, value);
                    return;
                }
                property.SetPropertyValue(entity, value);
            }
            else if (property.PropertyType.Equals(typeof(decimal)))
            {
                var value = rdr.GetValue<decimal>(readName);
                if (value == 0)
                {
                    return;
                }
                if (typeof(TEntity) == typeof(object))
                {
                    property.SetValue(entity, value);
                    return;
                }
                property.SetPropertyValue(entity, value);
            }
            else if (property.PropertyType.Equals(typeof(decimal?)))
            {
                var value = rdr.GetValue<decimal?>(readName);
                if (value == null)
                {
                    return;
                }
                if (typeof(TEntity) == typeof(object))
                {
                    property.SetValue(entity, value);
                    return;
                }
                property.SetPropertyValue(entity, value);
            }
            else if (property.PropertyType.Equals(typeof(double)))
            {
                var value = rdr.GetValue<double>(readName);
                if (value == 0)
                {
                    return;
                }
                if (typeof(TEntity) == typeof(object))
                {
                    property.SetValue(entity, value);
                    return;
                }
                property.SetPropertyValue(entity, value);
            }
            else if (property.PropertyType.Equals(typeof(double?)))
            {
                var value = rdr.GetValue<double?>(readName);
                if (value == null)
                {
                    return;
                }
                property.SetPropertyValue(entity, value);
            }
            else if (property.PropertyType.Equals(typeof(DateTime)))
            {
                var value = rdr.GetValue<DateTime>(readName);
                if (typeof(TEntity) == typeof(object))
                {
                    property.SetValue(entity, value);
                    return;
                }
                property.SetPropertyValue(entity, value);
            }
            else if (property.PropertyType.Equals(typeof(DateTime?)))
            {
                var value = rdr.GetValue<DateTime?>(readName);
                if (value == null)
                {
                    return;
                }
                if (typeof(TEntity) == typeof(object))
                {
                    property.SetValue(entity, value);
                    return;
                }
                property.SetPropertyValue(entity, value);
            }
            else if (property.PropertyType.Equals(typeof(bool)))
            {
                var value = rdr.GetValue<bool>(readName);
                if (value == false)
                {
                    return;
                }
                if (typeof(TEntity) == typeof(object))
                {
                    property.SetValue(entity, value);
                    return;
                }
                property.SetPropertyValue(entity, value);
            }
            else if (property.PropertyType.Equals(typeof(bool?)))
            {
                var value = rdr.GetValue<bool?>(readName);
                if (value == null)
                {
                    return;
                }
                if (typeof(TEntity) == typeof(object))
                {
                    property.SetValue(entity, value);
                    return;
                }
                property.SetPropertyValue(entity, value);
            }
            else if (property.PropertyType.Equals(typeof(byte[])))
            {
                var value = rdr.GetValue<byte[]>(readName);
                if (value == null)
                {
                    return;
                }
                if (typeof(TEntity) == typeof(object))
                {
                    property.SetValue(entity, value);
                    return;
                }
                property.SetPropertyValue(entity, value);
            }
            else
            {
                var value = rdr.GetValue<object>(readName);
                if (value == null)
                {
                    return;
                }
                if (typeof(TEntity) == typeof(object))
                {
                    property.SetValue(entity, value);
                    return;
                }
                property.SetPropertyValue(entity, value);
            }
        }
    }
}
