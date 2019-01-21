using Microsoft.AspNetCore.Mvc;
using NGP.Framework.Core;
using NGP.Framework.DependencyInjection;
using NGP.Framework.WebApi.Core;
using System.Collections.Generic;

namespace NGP.WebApi.Controllers
{
    public class HomeController : ApiController
    {
        /// <summary>
        /// config
        /// </summary>
        private readonly NGPConfig _ngpConfig;

        private readonly IWorkContext _workContext;

        private readonly IEngine _engine;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="ngpConfig"></param>
        public HomeController(NGPConfig ngpConfig, IWorkContext workContext, IEngine engine)
        {
            _ngpConfig = ngpConfig;
            _workContext = workContext;
            _engine = engine;
        }


        [HttpGet("test")]
        public ActionResult<IEnumerable<string>> test()
        {
            return Ok(new string[] { _ngpConfig.Secret,
                _workContext.Current.EmplName,
                _workContext.CurrentRequest.IpAddress,
                _workContext.CurrentRequest.Url,
                _engine.GetType().FullName});
        }

        [HttpGet("test1")]
        public ActionResult<IEnumerable<string>> test1()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("test2")]
        public ActionResult<IEnumerable<string>> test2()
        {
            return new string[] { "value1", "value2" };
        }
    }
}
