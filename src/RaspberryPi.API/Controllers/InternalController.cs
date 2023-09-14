using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace RaspberryPi.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InternalController : ControllerBase
    {
        private readonly IWebHostEnvironment _hostEnv;

        public InternalController(IWebHostEnvironment hostEnv)
        {
            _hostEnv = hostEnv ?? throw new ArgumentNullException(nameof(hostEnv));
        }

        [HttpGet("settings")]
        public IActionResult Settings()
        {
            string? frameworkName = Assembly.GetEntryAssembly()?.GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkName;
            string? aspNetCoreEnv = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            string osDescription = $"{RuntimeInformation.OSDescription} [ {RuntimeInformation.OSArchitecture} ]";

            var settings = new
            {
                EnvironmentName = _hostEnv.EnvironmentName,
                UserName = Environment.UserName,
                ASPNetCore_Environment = aspNetCoreEnv,
                OSDescription = osDescription,
                FrameworkName = frameworkName,
                IsDevelopment = _hostEnv.IsDevelopment(),
                IsProduction = _hostEnv.IsProduction(),
            };

            return Ok(settings);
        }

        [HttpGet("timestamp")]
        public IActionResult Timestamp()
        {
            return Ok(DateTime.Now);
        }

        [HttpDelete("exception")]
        public IActionResult Exception(string? exceptionMessage)
        {
            throw new NotImplementedException(exceptionMessage);
        }
    }
}