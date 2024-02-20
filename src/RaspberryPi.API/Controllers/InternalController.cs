using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace RaspberryPi.API.Controllers
{
    /// <summary>
    /// Developer related methods
    /// </summary>
    [ApiController]
    [Route("[controller]/[action]")]
    public class InternalController : ControllerBase
    {
        private readonly IWebHostEnvironment _hostEnv;

        public InternalController(IWebHostEnvironment hostEnv)
        {
            _hostEnv = hostEnv ?? throw new ArgumentNullException(nameof(hostEnv));
        }

        [HttpGet]
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

        [HttpGet]
        public IActionResult Timestamp()
        {
            return Ok(DateTime.Now);
        }

        [HttpDelete]
        public IActionResult Exception(string? exceptionMessage)
        {
            throw new NotImplementedException(exceptionMessage);
        }

        [HttpGet]
        public IActionResult EchoHeaders()
        {
            var headers = Request.Headers.ToDictionary(x => x.Key, x => x.Value.ToString())
                                         .OrderBy(x => x.Key);

            return Ok(headers);
        }

        [HttpGet]
        public IActionResult EchoIpAddress()
        {
            return Ok(HttpContext.Connection.RemoteIpAddress?.ToString());
        }

        [HttpGet]
        public IActionResult EchoCookies()
        {
            var cookies = Request.Cookies.ToDictionary(x => x.Key, x => x.Value.ToString())
                                         .OrderBy(x => x.Key);

            return Ok(cookies);
        }
    }
}