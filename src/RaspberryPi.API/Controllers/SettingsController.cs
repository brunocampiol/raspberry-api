using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace RaspberryPi.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SettingsController : ControllerBase
    {
        private readonly IWebHostEnvironment _hostEnv;

        public SettingsController(IWebHostEnvironment hostEnv)
        {
            _hostEnv = hostEnv ?? throw new ArgumentNullException(nameof(hostEnv));
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            string? frameworkName = Assembly.GetEntryAssembly()?.GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkName;
            string? aspNetCoreEnv = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            string osDescription = $"{RuntimeInformation.OSDescription} [ {RuntimeInformation.OSArchitecture} ]";

            var settings = new
            {
                EnvironmentName = _hostEnv.EnvironmentName,
                ASPNetCore_Environment = aspNetCoreEnv,
                OSDescription = osDescription,
                FrameworkName = frameworkName,
                IsDevelopment = _hostEnv.IsDevelopment(),
                IsProduction = _hostEnv.IsProduction(),
            };

            return Ok(settings);
        }

        [HttpGet("datetime")]
        public async Task<IActionResult> DateAndTime()
        {
            return Ok(DateTime.Now);
        }
    }
}
