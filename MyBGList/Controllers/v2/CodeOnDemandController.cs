using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace MyBGList.Controllers.v2
{
    [Route("v{version:apiVersion}/[controller]/[action]")]
    [ApiController]
    [ApiVersion("2.0")]
    public class CodeOnDemandController : ControllerBase
    {
        [HttpGet(Name = "GetCodTest")]
        [EnableCors("AnyOrigin_GetOnly")]
        [ResponseCache(NoStore = true)]
        public ContentResult Test()
        {
            return Content(
                "<script>" +
                "window.alert('Your client supports JavaScript!" +
                "\\r\\n\\r\\n" +
                $"Server time (UTC): {DateTime.UtcNow.ToString("o")}" +
                "\\r\\n" +
                "Client time (UTC): ' + new Date().toISOString());" +
                "</script>" +
                "<noscript>Your client does not support JavaScript</noscript>",
                "text/html");
        }

        [HttpGet(Name = "GetCodTest2")]
        [EnableCors("AnyOrigin_GetOnly")]
        [ResponseCache(NoStore = true)]
        public ContentResult Test2(int? addMinutes = null)
        {
            var serverTime = DateTime.UtcNow;
            if (addMinutes.HasValue)
                serverTime.AddMinutes(addMinutes.Value);

            return Content(
                "<script>" +
                "window.alert('Your client supports JavaScript!" +
                "\\r\\n\\r\\n" +
                $"Server time (UTC): {serverTime.ToString("o")}" +
                "\\r\\n" +
                "Client time (UTC): ' + new Date().toISOString());" +
                "</script>" +
                "<noscript>Your client does not support JavaScript</noscript>",
                "text/html");
        }
    }
}
