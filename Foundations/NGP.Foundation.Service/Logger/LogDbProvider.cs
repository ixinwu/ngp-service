/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * LogDbProvider Description:
 * 日志DB处理
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2017-3-9   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/
using NGP.Framework.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NGP.Foundation.Service.Logger
{
    /// <summary>
    /// 日志DB处理
    /// </summary>
    public class LogDbProvider : ILogProvider
    {
        /// <summary>
        /// 仓储接口
        /// </summary>
        private readonly IUnitRepository _unitRepository;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="unitRepository"></param>
        public LogDbProvider(IUnitRepository unitRepository)
        {
            _unitRepository = unitRepository;
        }

        #region IErrorLogManager Methods
        /// <summary>
        /// 插入错误日志
        /// </summary>
        /// <param name="info">错误信息</param>
        public void InsertSysErrorLog(ErrorLogInfo info)
        {
            try
            {
                //// 注册仓储接口依赖项
                //var unitRepository = UnityContainerFactory.Resolve<IUnitRepository>();
                //var errorLog = info.CopyItem<ErrorLogInfo, ZM_SYS_ErrorLog>((from, to) =>
                //{
                //    to.Id = GuidExtend.NewGuid();
                //    to.IsDelete = false;
                //    to.UpdatedBy = from.OperateBy;
                //    to.UpdatedDept = from.OperateDept;
                //    to.UpdatedTime = DateTime.Now;
                //});
                //unitRepository.Insert(errorLog);
                //UnityContainerFactory.Resolve<IUnitOfWork>().Commit();
            }
            catch { }
        }

        /// <summary>
        /// 插入业务日志
        /// </summary>
        /// <param name="context">日志对象</param>
        public void InsertBusinessLog(BusinessLogContext context)
        {
            try { 
            //{
            //    using (var unitRepository = UnityContainerFactory.Resolve<IUnitRepository>())
            //    {
            //        var addInfoList = new List<ZM_SYS_BusinessLog>();
            //        var detailList = new List<ZM_SYS_BusinessLogDetail>();
            //        foreach (var info in context.LogInfos)
            //        {
            //            var logItem = new ZM_SYS_BusinessLog
            //            {
            //                ApiPostParameter = context.ApiPostParameter,
            //                ApiUrl = context.ApiUrl,
            //                AppKey = info.AppKey,
            //                BusinessMethod = context.BusinessMethod,
            //                Content = info.Content,
            //                CreatedBy = context.OperateBy,
            //                CreatedDept = context.OperateDept,
            //                CreatedTime = DateTime.Now,
            //                FormKey = info.FormKey,
            //                Id = GuidExtend.NewGuid(),
            //                IsDelete = false,
            //                NameValue = info.NameValue,
            //                OperateBy = context.OperateBy,
            //                OperateDept = context.OperateDept,
            //                OperateType = context.OperateType,
            //                PrimaryKeyValue = info.PrimaryKeyValue,
            //                StatisticsDay = DateTime.Now.Day,
            //                StatisticsMonth = DateTime.Now.Month,
            //                StatisticsYear = DateTime.Now.Year,
            //                TableName = info.FormKey,
            //                UpdatedBy = context.OperateBy,
            //                UpdatedDept = context.OperateDept,
            //                UpdatedTime = DateTime.Now
            //            };
            //            addInfoList.Add(logItem);

            //            var currentDetialList = info.LogDetails.Select(s => new ZM_SYS_BusinessLogDetail
            //            {
            //                AppKey = logItem.AppKey,
            //                ColumnName = s.ColumnName,
            //                CreatedBy = context.OperateBy,
            //                CreatedDept = context.OperateDept,
            //                CreatedTime = DateTime.Now,
            //                UpdatedBy = context.OperateBy,
            //                UpdatedDept = context.OperateDept,
            //                UpdatedTime = DateTime.Now,
            //                CurrentText = s.CurrentText,
            //                CurrentValue = s.CurrentValue,
            //                FieldKey = s.FieldKey,
            //                FormKey = logItem.FormKey,
            //                Id = GuidExtend.NewGuid(),
            //                LogId = logItem.Id,
            //                OriginalText = s.OriginalText,
            //                OriginalValue = s.OriginalValue,
            //                PrimaryKeyValue = logItem.PrimaryKeyValue,
            //                TableName = logItem.TableName,
            //                IsDelete = false
            //            });
            //            detailList.AddRange(currentDetialList);
            //        }

            //        unitRepository.BulkInsert(addInfoList);
            //        unitRepository.BulkInsert(detailList);
            //    }
            }
            catch{ }
        }
        #endregion
    }
}
