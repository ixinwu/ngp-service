/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * IDynamicDataService Description:
 * 动态数据服务接口
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019/1/22 9:07:18   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/


using NGP.Framework.Core;
using System.Linq;

namespace NGP.Foundation.Service.Analysis
{
    /// <summary>
    /// 动态数据服务接口
    /// </summary>
    public class ResolveJsonDataProvider : IResolveDataProvider
    {
        /// <summary>
        /// 初始化解析上下文
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResolveInitContext InitResolveContext(DynamicBaseRequest request)
        {
            var dataSetFileName = string.Format("\\{0}.json", request.DataSetKey);
            var dataSetFilePath = CommonHelper.DefaultFileProvider.MapPath(GlobalConst.__ResolveJsons + dataSetFileName);

            var result = CommonHelper.DefaultFileProvider.GetFileContent<ResolveInitContext>(dataSetFilePath);

            var appFileName = string.Format("\\{0}.json", request.DataSetKey.Split('_').FirstOrDefault());

            var appPath = CommonHelper.DefaultFileProvider.MapPath(GlobalConst.__ResolveJsons + appFileName);
            var app = CommonHelper.DefaultFileProvider.GetFileContent<App_Config_BaseInfo>(appPath);

            result.App = app;
            return result;
        }
    }
}
