using Microsoft.AspNetCore.Mvc;

namespace RaspberryPi.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SettingsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _hostEnv;

        public SettingsController(IConfiguration configuration, IWebHostEnvironment hostEnv)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _hostEnv = hostEnv ?? throw new ArgumentNullException(nameof(hostEnv));
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var configuration = _configuration.AsEnumerable();

            var settings = new
            {
                EnvironmentName = _hostEnv.EnvironmentName,
                IsDevelopment = _hostEnv.IsDevelopment(),
                IsProduction = _hostEnv.IsProduction(),
                Configuration = configuration
            };

            return Ok(settings);
        }
    }
}
