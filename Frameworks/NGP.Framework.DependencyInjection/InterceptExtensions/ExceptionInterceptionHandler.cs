/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * PageQueryAttribute Description:
 * 分页查询特性
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-27   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using Castle.DynamicProxy;
using NGP.Foundation.Resources;
using NGP.Framework.Core;
using System;
using System.Reflection;

namespace NGP.Framework.DependencyInjection
{
    /// <summary>
    /// 异常特性
    /// </summary>
    public class ExceptionInterceptionHandler : IInterceptor
    {
        /// <summary>
        /// 工作上下文
        /// </summary>
        private readonly IWorkContext _workContext;

        /// <summary>
        /// 日志发布者
        /// </summary>
        private readonly ILogPublisher _logPublisher;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="workContext"></param>
        /// <param name="logPublisher"></param>
        public ExceptionInterceptionHandler(IWorkContext workContext, ILogPublisher logPublisher)
        {
            _workContext = workContext;
            _logPublisher = logPublisher;
        }

        /// <summary>
        /// 拦截器
        /// </summary>
        /// <param name="invocation"></param>
        public void Intercept(IInvocation invocation)
        {
            MethodInfo methodInfo = null;
            try
            {
                methodInfo = invocation.MethodInvocationTarget;
                if (methodInfo == null)
                {
                    methodInfo = invocation.Method;
                }

                // 执行方法
                invocation.Proceed();
            }
            catch (Exception ex)
            {
                // 书写异常日志
                ErrorLogInfo error = new ErrorLogInfo
                {
                    ApiUrl = _workContext.CurrentRequest.Url,
                    Parameters = _workContext.CurrentRequest.Parameter,
                    BusinessMethod = string.Format("{0}.{1}.{2}",
                                                    methodInfo.ReflectedType.Namespace,
                                                    methodInfo.ReflectedType.Name,
                                                    methodInfo.Name),
                    ExceptionContent = string.IsNullOrEmpty(ex.Message) ?
                                            (ex.InnerException != null ? ex.InnerException.Message : "Unknow Error")
                                            : ex.Message,
                    OperatedBy = _workContext.Current != null ? _workContext.Current.EmplId : string.Empty,
                    OperatedDept = _workContext.Current != null ? _workContext.Current.DeptId : string.Empty,
                    Exception = ex
                };
                _logPublisher.RegisterError(error);

                // 设定返回值
                INGPResponse response = invocation.ReturnValue as INGPResponse;
                if (response == null)
                {
                    response = Activator.CreateInstance(methodInfo.ReturnType) as INGPResponse;
                }
                response.Message = CommonResource.OperatorException;
                response.Status = OperateStatus.Error;
                response.ErrorCode = ErrorCode.SystemError;

                invocation.ReturnValue = response;
            }
        }
    }
}
