using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaspberryPi.API.Models.Requests;
using RaspberryPi.Application.Interfaces;
using RaspberryPi.Application.Services;

namespace RaspberryPi.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IdentityController : ControllerBase
    {
        private readonly IJwtAppService _jwtService;

        public IdentityController(IJwtAppService jwtService)
        {
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Post([FromBody] TokenGenerationRequest model)
        {
            var user = IdentityAppService.Get(model.UserName, model.Password);

            if (user is null) return BadRequest("Invalid user or password");
            
            var token = _jwtService.GenerateToken(user.Email, user.Role);

            return Ok(token);
        }

        [HttpGet]
        [Route("authenticated")]
        [Authorize]
        public string Authenticated() => $"Authenticated";

        [HttpGet]
        [Route("admin")]
        [Authorize(Roles = "admin")]
        public string Admin() => "Admin";

        [HttpGet]
        [Route("user")]
        [Authorize(Roles = "user")]
        public string User() => "User";
    }
}