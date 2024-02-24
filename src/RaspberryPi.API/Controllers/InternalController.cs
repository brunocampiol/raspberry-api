using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaspberryPi.Domain.Core;
using System.ComponentModel.DataAnnotations;
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
        private readonly IHttpClientFactory _httpClientFactory;

        public InternalController(IWebHostEnvironment hostEnv, IHttpClientFactory httpClientFactory)
        {
            _hostEnv = hostEnv ?? throw new ArgumentNullException(nameof(hostEnv));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
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

        [HttpGet]
        [Route("isRoot")]
        [Authorize(Roles = "root")]
        public async Task<Result<string>> WwwGetAsString([FromHeader][Required] string url,
                                                         [FromHeader][Required] int timeoutInSeconds = 15)
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.Timeout = TimeSpan.FromSeconds(timeoutInSeconds);

            var uri = new Uri(url);
            var httpResponse = await httpClient.GetAsync(uri);
            var httpContent = await httpResponse.Content.ReadAsStringAsync();

            if (!httpResponse.IsSuccessStatusCode)
            {
                var errorMessage = $"Response '{httpResponse.StatusCode}' " +
                                   $"is not in 2XX range: '{httpContent}'";

                return Result<string>.Failure(errorMessage);
            }

            return Result<string>.Success(httpContent);
        }
    }
}