using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RaspberryPi.API.Models.Data;
using RaspberryPi.API.Repositories;
using RaspberryPi.API.Services;

namespace RaspberryPi.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IdentityController : ControllerBase
    {
        public IdentityController()
        {
            
        }

        [HttpPost("login")]
        public async Task<IActionResult> Post([FromBody] User model)
        {
            var user = UserRepository.Get(model.Username, model.Password);

            if (user is null) return BadRequest("Invalid user or password");
            
            var token = TokenService.GenerateToken(user);

            return Ok(new { user = user.Username, token = token });
        }

        [HttpGet]
        [Route("authenticated")]
        [Authorize]
        public string Authenticated() => string.Format("Authenticated - {0}", User.Identity.Name);

        [HttpGet]
        [Route("admin")]
        [Authorize(Roles = "admin")]
        public string Admin() => "Admin";
    }
}