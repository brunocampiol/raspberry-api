using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaspberryPi.API.Models.Requests;
using RaspberryPi.API.Repositories;
using RaspberryPi.API.Services;

namespace RaspberryPi.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IdentityController : ControllerBase
    {
        private readonly IJwtService _jwtService;

        public IdentityController(IJwtService jwtService)
        {
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Post([FromBody] TokenGenerationRequest model)
        {
            var user = UserRepository.Get(model.UserName, model.Password);

            if (user is null) return BadRequest("Invalid user or password");
            
            var token = _jwtService.GenerateToken(user);

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