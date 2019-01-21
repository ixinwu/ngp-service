/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * TransactionInterceptorHandler Description:
 * 事务拦截器特性
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-27   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using Castle.DynamicProxy;
using NGP.Framework.Core;
using System;
using System.Linq;
using System.Reflection;
using System.Transactions;

namespace NGP.Framework.DependencyInjection
{
    /// <summary>
    /// 事务拦截器
    /// </summary>
    public class TransactionInterceptorHandler : IInterceptor
    {
        /// <summary>
        /// 工作单元
        /// </summary>
        private readonly IUnitRepository _unitRepository;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="unitRepository"></param>
        public TransactionInterceptorHandler(IUnitRepository unitRepository)
        {
            _unitRepository = unitRepository;
        }

        /// <summary>
        /// 拦截器
        /// </summary>
        /// <param name="invocation"></param>
        public void Intercept(IInvocation invocation)
        {
            MethodInfo methodInfo = null;
            methodInfo = invocation.MethodInvocationTarget;
            if (methodInfo == null)
            {
                methodInfo = invocation.Method;
            }

            // 获取事务特性
            var transaction = methodInfo.GetCustomAttributes<TransactionCallHandlerAttribute>(true).FirstOrDefault();
            if (transaction == null)
            {
                // 执行方法
                invocation.Proceed();
                return;
            }

            if (transaction.IsTransactionScope)
            {
                var transactionOptions = new TransactionOptions();
                //设置事务隔离级别
                transactionOptions.IsolationLevel = transaction.IsolationLevel;
                //设置事务超时时间
                transactionOptions.Timeout = new TimeSpan(0, 0, transaction.Timeout);
                using (var ts = new TransactionScope(transaction.ScopeOption, transactionOptions))
                {
                    // 执行方法
                    invocation.Proceed();

                    if (transaction.IsContextCommit)
                    {
                        _unitRepository.SaveChanges();
                    }
                    ts.Complete();
                }
                return;
            }
            // 执行方法
            invocation.Proceed();

            if (transaction.IsContextCommit)
            {
                _unitRepository.SaveChanges();
            }
        }
    }
}
