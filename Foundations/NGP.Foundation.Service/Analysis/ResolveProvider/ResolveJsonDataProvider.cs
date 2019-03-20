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
            var fileName = string.Format("\\{0}.json", request.DataSetKey);
            var filePath = CommonHelper.DefaultFileProvider.MapPath(GlobalConst.__ResolveJsons + fileName);

            var result = CommonHelper.DefaultFileProvider.GetFileContent<ResolveInitContext>(filePath);

            return result;
        }
    }
}
