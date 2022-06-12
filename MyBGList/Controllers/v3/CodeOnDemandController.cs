using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace MyBGList.Controllers.v3
{
    [Route("v{version:apiVersion}/[controller]/[action]")]
    [ApiController]
    [ApiVersion("3.0")]
    public class CodeOnDemandController : ControllerBase
    {

        [HttpGet(Name = "GetCodTest2")]
        [EnableCors("AnyOrigin_GetOnly")]
        [ResponseCache(NoStore = true)]
        public ContentResult Test2(int? minutesToAdd = null)
        {
            var serverTime = DateTime.UtcNow;
            if (minutesToAdd.HasValue)
                serverTime.AddMinutes(minutesToAdd.Value);

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
