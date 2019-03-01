using NGP.Framework.Core;
using System;
using System.Data;
using System.Data.Common;
using Topshelf;

namespace NGP.ServiceHost
{
    class Program
    {
        static void Main(string[] args)
        {            
            HostFactory.Run(x =>
            {
                // 注册服务
                x.Service<Startup>(s =>
                {
                    s.ConstructUsing(tc => SingletonNew<Startup>.Instance);
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Shutdown());
                });
                x.RunAsLocalSystem();   //服务使用NETWORK_SERVICE内置账户运行
                x.SetDescription("NGPServiceHost");
                x.SetDisplayName("NGPServiceHost");
                x.SetServiceName("NGPServiceHost");
            });
        }
    }
}
