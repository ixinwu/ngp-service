/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * RabbitMQSubscriber Description:
 * 消息订阅者实现
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019/3/4 11:20:22 hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/
using Newtonsoft.Json;
using NGP.Framework.Core;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.MessagePatterns;
using System;
using System.Text;
using System.Threading;

namespace NGP.Middleware.MessageQueue
{
    /// <summary>
    /// 消息订阅者实现
    /// </summary>
    public class RabbitMQSubscriber: IMessageSubscriber
    {
        /// <summary>
        /// 注册监听
        /// </summary>
        /// <typeparam name="T">监听数据类型</typeparam>
        /// <param name="mqInfo">监听通道对象</param>
        /// <param name="action">监听回调</param>
        public void Monitor<T>(MessageRouteInfo mqInfo, Action<T> action)
        {
            // 传输对象
            var paramInfo = new MonitorMessageRouteInfo<T>
            {
                Action = action,
                MessageRoute = mqInfo
            };

            // 后台线程
            var thr = new Thread((obj) => {
                var param = obj as MonitorMessageRouteInfo<T>;
                var mqRouteInfo = param.MessageRoute;
                var factory = new ConnectionFactory();
                factory.HostName = mqRouteInfo.HostName;
                factory.UserName = mqRouteInfo.UserName;
                factory.Password = mqRouteInfo.Password;
                factory.AutomaticRecoveryEnabled = true;
                using (var connection = factory.CreateConnection())
                {
                    using (var model = connection.CreateModel())
                    {
                        // 注册交换机
                        model.ExchangeDeclare(mqRouteInfo.ExchangeName, ExchangeType.Fanout, true);

                        // 注册队列
                        model.QueueDeclare(mqRouteInfo.QueueName, mqRouteInfo.QueueDurable, false, false, null);

                        // 绑定队列
                        model.QueueBind(mqRouteInfo.QueueName, mqRouteInfo.ExchangeName, mqRouteInfo.RouteKey);

                        var subscription = new Subscription(model, mqRouteInfo.QueueName, false);
                        while (true)
                        {
                            try
                            {
                                BasicDeliverEventArgs basicDeliveryEventArgs = subscription.Next();

                                var json = Encoding.UTF8.GetString(basicDeliveryEventArgs.Body);

                                var data = JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings
                                {
                                    TypeNameHandling = TypeNameHandling.All,
                                });

                                param.Action(data);

                                subscription.Ack(basicDeliveryEventArgs);

                            }
                            catch (Exception ex)
                            {
                                var type = GetType();
                                // 写日志
                                var info = new ErrorLogInfo
                                {
                                    BusinessMethod = string.Format("{0}.{1}", type.FullName, "Excute"),
                                    ExceptionContent = string.IsNullOrEmpty(ex.Message) ? (ex.InnerException != null ? ex.InnerException.Message : "Unknow Error") : ex.Message,
                                    Exception = ex
                                };
                                Singleton<IEngine>.Instance.Resolve<ILogPublisher>().RegisterError(info);
                            }
                        }
                    }
                }
            })
            {
                IsBackground = true
            };
            thr.Start(paramInfo);
        }
    }
}
