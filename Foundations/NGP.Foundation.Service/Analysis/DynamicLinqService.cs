/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * IDynamicLinqExcuteService Description:
 * 动态linq执行服务接口
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019/1/22 9:07:18   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using NGP.Foundation.Resources;
using NGP.Framework.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NGP.Foundation.Service.Analysis
{
    /// <summary>
    /// 动态linq执行服务接口
    /// </summary>
    public class DynamicLinqService : IDynamicLinqService
    {
        /// <summary>
        /// 工作单元执行仓储
        /// </summary>
        private readonly IUnitRepository _unitRepository;

        /// <summary>
        /// linq解析器
        /// </summary>
        private readonly ILinqParserHandler _linqParserHandler;

        /// <summary>
        /// 工作上下文
        /// </summary>
        private readonly IWorkContext _workContext;

        /// <summary>
        /// 解析数据提供者
        /// </summary>
        private readonly IResolveDataProvider _resolveDataProvider;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="unitRepository"></param>
        /// <param name="linqParserHandler"></param>
        /// <param name="workContext"></param>
        /// <param name="resolveDataProvider"></param>
        public DynamicLinqService(IUnitRepository unitRepository, 
            ILinqParserHandler linqParserHandler, 
            IWorkContext workContext,
            IResolveDataProvider resolveDataProvider)
        {
            _unitRepository = unitRepository;
            _linqParserHandler = linqParserHandler;
            _workContext = workContext;
            _resolveDataProvider = resolveDataProvider;
        }

        /// <summary>
        /// 动态linq执行
        /// </summary>
        /// <param name="request">请求对象</param>
        /// <returns>查询结果</returns>
        public NGPResponse<dynamic> Extute(DynamicLinqRequest request)
        {
            if (request == null)
            {
                return new NGPResponse<dynamic>
                {
                    AffectedRows = 0,
                    ErrorCode = ErrorCode.ParamEmpty,
                    Message = CommonResource.ParameterError,
                    Status = OperateStatus.Error
                };
            }

            var parserResult = _linqParserHandler.Resolve(new LinqParserRequest
            {
                Current = _workContext.Current,
                DslContent = request.Dsl
            });

            switch (parserResult.ParserType)
            {
                
                case LinqParserType.Query:
                    {
                        var initContext = _resolveDataProvider.InitResolveContext(request);

                        // 获取查询字段列表
                        var generateList = initContext.FormFields
                             .Where(s => parserResult.SelectFieldKeys.Contains(s.FieldKey))
                             .Select(s => new DynamicGenerateObject
                             {
                                 CodeType = s.DbConfig.ColumnType.ToEnum<FieldColumnType>().GetCodeType(),
                                 ObjectKey = s.FieldKey
                             }).ToList(); ;

                        generateList.AddRange(parserResult.GenerateObjects ?? new List<DynamicGenerateObject>());

                        // 生成类型
                        var generateType = generateList.CompileType();

                        var data = _unitRepository.ReadValues(parserResult.Command.CommandText, generateType, parserResult.Command.ParameterCollection);
                        return new NGPResponse<dynamic>
                        {
                            Message = CommonResource.OperatorSuccess,
                            Status = OperateStatus.Success,
                            Data = data
                        };
                    }
                case LinqParserType.ExecuteNonQuery:
                    {
                        var count = _unitRepository.ExecuteNonQuery(parserResult.Command.CommandText, parserResult.Command.ParameterCollection);
                        return new NGPResponse<dynamic>
                        {
                            AffectedRows = count,
                            Message = CommonResource.OperatorSuccess,
                            Status = OperateStatus.Success
                        };
                    }
                case LinqParserType.None:                    
                default:
                    {
                        return new NGPResponse<dynamic>
                        {
                            AffectedRows = 0,
                            ErrorCode = ErrorCode.CheckError,
                            Message = CommonResource.ParameterError,
                            Status = OperateStatus.Error
                        };
                    }
            }
        }
    }
}
