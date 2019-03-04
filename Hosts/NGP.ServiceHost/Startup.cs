/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * Startup Description:
 * 程序启动入口
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-2-28   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NGP.Framework.Core;
using NGP.Framework.DataAccess;
using NGP.Framework.DependencyInjection;
using System;
using System.IO;

namespace NGP.ServiceHost
{
    /// <summary>
    /// 启动类
    /// </summary>
    public class Startup
    {
        #region Properties

        /// <summary>
        /// 获取应用程序的配置
        /// </summary>
        public IConfiguration Configuration { get; set; }

        #endregion

        /// <summary>
        /// host启动执行的任务
        /// </summary>
        public void Start()
        {
            var services = new ServiceCollection();

            ////add object context
            //services.AddDbContext<UnitObjectContext>(optionsBuilder =>
            //{
            //    optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DbConnection"));
            //});

            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(AppContext.BaseDirectory))
                .AddJsonFile("appsettings.json", optional: true);

            Configuration = builder.Build();

            services.ConfigureApplicationServices(Configuration);
        }

        /// <summary>
        /// 释放全局的对象
        /// </summary>
        public void Shutdown()
        {
        }
    }
}
