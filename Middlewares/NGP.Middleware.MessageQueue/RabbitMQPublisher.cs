/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * RabbitMQPublisher Description:
 * RabbitMq消息发布者
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019/3/4 11:20:22 hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NGP.Framework.Core;

namespace NGP.Middleware.MessageQueue
{
    /// <summary>
    /// RabbitMq消息发布者
    /// </summary>
    public class RabbitMQPublisher : IMessagePublisher
    {
        /// <summary>
        /// 模式
        /// </summary>
        private static readonly Dictionary<string, IModel> model = new Dictionary<string, IModel>();

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <typeparam name="T">发送数据对象</typeparam>
        /// <param name="mqInfo">目标通道信息</param>
        /// <param name="data">数据</param>
        public void Send<T>(MessageRouteInfo mqInfo, T data)
        {
            try
            {
                // 创建链接
                var connectionFactory = new ConnectionFactory();
                connectionFactory.HostName = mqInfo.HostName;
                connectionFactory.UserName = mqInfo.UserName;
                connectionFactory.Password = mqInfo.Password;
                connectionFactory.AutomaticRecoveryEnabled = true;
                using (var connection = connectionFactory.CreateConnection())
                {
                    using (var model = connection.CreateModel())
                    {
                        // 注册交换机
                        model.ExchangeDeclare(mqInfo.ExchangeName, ExchangeType.Fanout, true);

                        // 注册队列
                        model.QueueDeclare(mqInfo.QueueName, mqInfo.QueueDurable, false, false, null);

                        // 对象转换成字节流
                        var json = JsonConvert.SerializeObject(data, new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.All
                        });
                        var messageBodyBytes = Encoding.UTF8.GetBytes(json);

                        // 发布数据
                        model.BasicPublish(mqInfo.ExchangeName,
                            mqInfo.RouteKey,
                            null, 
                            messageBodyBytes);
                    }
                }
            }
            catch(Exception ex)
            {
                var type = this.GetType();
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
