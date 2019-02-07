using AspNet.Security.OpenIdConnect.Primitives;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Poc.Authentication.OpenIdConnect.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GreetingsController : ControllerBase
    {
        [HttpGet("helloworld")]
        public ActionResult<string> SayHelloWorld()
        {
            return "Hello World!";
        }

        [HttpGet("hi")]
        public ActionResult<string> SayHi()
        {
            var userPrincipal = HttpContext.User;
            var userName = userPrincipal.FindFirst(OpenIdConnectConstants.Claims.Subject);
            return $"Hi, {userName.Value}!";
        }                
    }
}
