using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaspberryPi.API.Models.Requests;
using RaspberryPi.Application.Interfaces;
using RaspberryPi.Application.Services;

namespace RaspberryPi.API.Controllers
{
    /// <summary>
    /// Authentication and identity related methods (Bearer token header)
    /// </summary>
    [ApiController]
    [Route("[controller]")]
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
        [HttpPost("login")]
        public async Task<IActionResult> Post([FromBody] TokenGenerationRequest model)
        {
            var result = _appService.Authenticate(model.UserName, model.Password);

            if (!result.IsSuccess) return BadRequest(result.Errors);

            return Ok(result.Value);
        }

        /// <summary>
        /// Returns HTTP status 200 OK when user is authenticated
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("authenticated")]
        [Authorize]
        public string Authenticated() => $"Authenticated";

        /// <summary>
        /// Returns HTTP status 200 OK when user is admin
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("isAdmin")]
        [Authorize(Roles = "admin")]
        public string Admin() => "Admin";

        [HttpGet]
        [Route("isUser")]
        [Authorize(Roles = "user")]
        public string User() => "User";
    }
}