/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * QueryCondition Description:
 * 查询对象包装
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-15   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using System;
using System.Linq.Expressions;

namespace NGP.Framework.Core
{
    /// <summary>
    /// 查询对象包装
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class QueryCondition<T>
    {
        /// <summary>
        /// 封装的条件
        /// </summary>
        public Expression<Func<T, bool>> Where { get; set; }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="condition"></param>
        public QueryCondition(Expression<Func<T, bool>> condition = null)
        {
            if (condition != null)
            {
                this.Where = condition;
            }
        }

        /// <summary>
        /// 且操作符
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static QueryCondition<T> operator &(QueryCondition<T> condition, Expression<Func<T, bool>> right)
        {
            if (condition != null && condition.Where != null)
            {
                condition.Where = condition.Where.Splice(right, Expression.AndAlso);
            }
            else
            {
                condition.Where = right;
            }
            return condition;
        }

        /// <summary>
        /// 或操作符
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static QueryCondition<T> operator |(QueryCondition<T> condition, Expression<Func<T, bool>> right)
        {
            if (condition != null && condition.Where != null)
            {
                condition.Where = condition.Where.Splice(right, Expression.OrElse);
            }
            else
            {
                condition.Where = right;
            }
            return condition;
        }

        /// <summary>
        /// 且操作符
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static QueryCondition<T> operator &(QueryCondition<T> condition, QueryCondition<T> right)
        {
            if (right != null || right.Where != null)
            {
                condition &= right.Where;
            }
            return condition;
        }

        /// <summary>
        /// 或操作符
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static QueryCondition<T> operator |(QueryCondition<T> condition, QueryCondition<T> right)
        {
            if (right != null && right.Where != null)
            {
                condition |= right.Where;
            }
            return condition;
        }
    }

    /// <summary>
    /// 查询对象包装
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    public class QueryCondition<T1,T2>
    {
        /// <summary>
        /// 封装的条件
        /// </summary>
        public Expression<Func<T1,T2, bool>> Where { get; set; }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="condition"></param>
        public QueryCondition(Expression<Func<T1, T2, bool>> condition = null)
        {
            if (condition != null)
            {
                this.Where = condition;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static QueryCondition<T1, T2> operator &(QueryCondition<T1, T2> condition, Expression<Func<T1, T2, bool>> right)
        {
            if (condition != null && condition.Where != null)
            {
                condition.Where = condition.Where.Splice(right, Expression.AndAlso);
            }
            else
            {
                condition.Where = right;
            }
            return condition;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static QueryCondition<T1, T2> operator |(QueryCondition<T1, T2> condition, Expression<Func<T1, T2, bool>> right)
        {
            if (condition != null && condition.Where != null)
            {
                condition.Where = condition.Where.Splice(right, Expression.OrElse);
            }
            else
            {
                condition.Where = right;
            }
            return condition;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static QueryCondition<T1, T2> operator &(QueryCondition<T1, T2> condition, QueryCondition<T1, T2> right)
        {
            if (right != null || right.Where != null)
            {
                condition &= right.Where;
            }
            return condition;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static QueryCondition<T1, T2> operator |(QueryCondition<T1, T2> condition, QueryCondition<T1, T2> right)
        {
            if (right != null && right.Where != null)
            {
                condition |= right.Where;
            }
            return condition;
        }
    }
}
