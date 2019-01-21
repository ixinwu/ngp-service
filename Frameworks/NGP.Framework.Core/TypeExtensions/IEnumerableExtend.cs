/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * IEnumerableExtend Description:
 * 列表接口扩展
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-15   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Linq;

namespace NGP.Framework.Core
{
    /// <summary>
    /// 列表接口扩展
    /// </summary>
    public static class IEnumerableExtend
    {
        /// <summary>
        /// 列表深度拷贝（相同对象）
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="sourceList">源列表</param>
        /// <param name="action">自定义赋值</param>
        /// <returns>深度拷贝结果</returns>
        public static List<T> DeepCopy<T>(this IEnumerable<T> sourceList, Action<T> action = null)
            where T : class, new()
        {
            var result = new List<T>();

            if (sourceList.IsNullOrEmpty())
            {
                return result;
            }

            foreach (var item in sourceList)
            {
                var addItem = item.CopyItem(action);
                result.Add(addItem);
            }

            return result;
        }

        /// <summary>
        /// 列表深度拷贝（不同对象）
        /// </summary>
        /// <typeparam name="TFrom">源对象类型</typeparam>
        /// <typeparam name="TTo">目标对象类型</typeparam>
        /// <param name="sourceList">源列表</param>
        /// <param name="action">自定义赋值</param>
        /// <returns>深度拷贝结果</returns>
        public static List<TTo> DeepCopy<TFrom, TTo>(this IEnumerable<TFrom> sourceList, Action<TFrom, TTo> action = null)
            where TFrom : class, new()
            where TTo : class, new()
        {
            var result = new List<TTo>();
            if (sourceList.IsNullOrEmpty())
            {
                return result;
            }
            foreach (var item in sourceList)
            {
                var addItem = item.CopyItem<TFrom, TTo>(action);
                result.Add(addItem);
            }
            return result;
        }

        /// <summary>
        /// 判断列表是否为null或者空
        /// </summary>
        /// <typeparam name="T">列表类型</typeparam>
        /// <param name="list">判断源</param>
        /// <returns>判断结果</returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> list)
        {
            return list == null || list.Count() == 0;
        }

        /// <summary>
        /// 返回列表默认值
        /// </summary>
        /// <typeparam name="TEntity">泛型类型</typeparam>
        /// <param name="source">源列表</param>
        /// <returns>列表的第一个对象</returns>
        public static TEntity FirstOrDefaultWithNullList<TEntity>(this IEnumerable<TEntity> source) where TEntity : new()
        {
            if (!source.IsNullOrEmpty())
            {
                return source.FirstOrDefault();
            }

            return new TEntity();
        }

        /// <summary>
        /// 递归查找子集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="criteria"></param>
        /// <param name="selectCurrent"></param>
        /// <param name="parentKey"></param>
        /// <param name="childList"></param>
        public static void RecursionFindChild<T>(this IEnumerable<T> source,
            Func<T, string, bool> criteria,
            Func<T, string> selectCurrent,
            string parentKey,
            List<T> childList)
        {
            var tempList = source.Where(s => criteria(s, parentKey));
            childList.AddRange(tempList);
            foreach (var item in tempList)
            {
                var currentParent = selectCurrent(item);
                RecursionFindChild(source, criteria, selectCurrent, currentParent, childList);
            }
        }

        /// <summary>
        /// 递归初始化子类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static List<T> RecursionInitChild<T>(this IEnumerable<T> source)
            where T : BaseTreeInfo
        {
            var result = new List<T>();

            var firstParents = source.Where(s => string.IsNullOrWhiteSpace(s.ParentId)).ToList();
            result.AddRange(firstParents);
            foreach (var firstParent in firstParents)
            {
                RecursionParentChild(source, firstParent);
            }
            return result;
        }

        /// <summary>
        /// 持续递归查找子类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="parentItem"></param>
        private static void RecursionParentChild<T>(IEnumerable<T> source,T parentItem) where T : BaseTreeInfo
        {
            var childList = source.Where(s => s.ParentId == parentItem.Id).ToList();
            if (childList.IsNullOrEmpty())
            {
                return;
            }
            if (parentItem.Children == null)
            {
                parentItem.Children = new List<BaseTreeInfo>();
            }
            parentItem.Children.AddRange(childList);

            foreach (var child in childList)
            {
                RecursionParentChild(source, child);
            }
        }

        /// <summary>
        /// 比较两个数组
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="a1">Array 1</param>
        /// <param name="a2">Array 2</param>
        /// <returns>Result</returns>
        public static bool ArraysEqual<T>(this T[] a1, T[] a2)
        {
            //also see Enumerable.SequenceEqual(a1, a2);
            if (ReferenceEquals(a1, a2))
                return true;

            if (a1 == null || a2 == null)
                return false;

            if (a1.Length != a2.Length)
                return false;

            var comparer = EqualityComparer<T>.Default;
            return !a1.Where((t, i) => !comparer.Equals(t, a2[i])).Any();
        }
    }
}
