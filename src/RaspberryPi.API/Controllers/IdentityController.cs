using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaspberryPi.API.Models.Requests;
using RaspberryPi.Application.Interfaces;

namespace RaspberryPi.API.Controllers
{
    /// <summary>
    /// Authentication and identity related methods (Bearer token header)
    /// </summary>
    [ApiController]
    [Route("[controller]/[action]")]
    public class IdentityController : ControllerBase
    {
        private readonly IIdentityAppService _appService;

        public IdentityController(IIdentityAppService appService)
        {
            _appService = appService;
        }

        /// <summary>
        /// Given a user name and password generates the Bearer token
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetToken([FromBody] TokenGenerationRequest model)
        {
            var result = _appService.Authenticate(model.UserName, model.Password);

            if (!result.IsSuccess) return BadRequest(result);

            return Ok(result.Value);
        }

        /// <summary>
        /// Returns HTTP status 200 OK when user is authenticated
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public string IsAuthenticated() => $"Authenticated";

        /// <summary>
        /// Returns HTTP status 200 OK when user has root role
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "root")]
        public string IsRoot() => "Root";

        /// <summary>
        /// Returns HTTP status 200 OK when user has user role
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "user")]
        public string IsUser() => "User";
    }
}