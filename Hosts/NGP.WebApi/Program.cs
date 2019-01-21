/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * Program Description:
 * 程序入口
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-2-28   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace NGP.WebApi
{
    /// <summary>
    /// 程序入口
    /// </summary>
    public class Program
    {
        /// <summary>
        /// main 函数
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            var host = WebHost.CreateDefaultBuilder(args)
               .UseKestrel(options => options.AddServerHeader = false)
               .UseStartup<Startup>()
               .Build();

            host.Run();
        }
    }
}
