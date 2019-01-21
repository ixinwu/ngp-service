/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * ExpressionExtend Description:
 * 表达式树扩展
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-15   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using System;
using System.Linq;
using System.Linq.Expressions;

namespace NGP.Framework.Core
{
    /// <summary>
    /// 表达式树扩展
    /// </summary>
    public static class ExpressionExtend
    {
        /// <summary>
        /// 扩展获取值format
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public static object Eval(this Expression expr)
        {
            return expr.Eval<object>();
        }

        /// <summary>
        /// 扩展获取值format(泛型)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expr"></param>
        /// <returns></returns>
        public static T Eval<T>(this Expression expr)
        {
            var constantExpr = expr as ConstantExpression;
            if (constantExpr != null)
            {
                return (T)constantExpr.Value;
            }
            var fun = Expression.Lambda<Func<object>>(Expression.Convert(expr, typeof(object))).Compile();

            return (T)(fun());
        }

        /// <summary>
        /// 整合两个表达式
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="left">左表达式</param>
        /// <param name="right">右表达式</param>
        /// <param name="method">合并方法</param>
        /// <returns>合并结果</returns>
        public static Expression<T> Splice<T>(this Expression<T> left, Expression<T> right, Func<Expression, Expression, Expression> method)
        {
            var dictionary = left.Parameters.Select((f, i) => new { f, s = right.Parameters[i] }).ToDictionary(p => p.s, p => p.f);
            //替换参数
            var secondBody = ParameterReset.ReplaceParameterList(dictionary, right.Body);
            return Expression.Lambda<T>(method(left.Body, secondBody), left.Parameters);
        }
    }
}
