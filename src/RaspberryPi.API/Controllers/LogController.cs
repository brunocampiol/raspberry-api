﻿using Microsoft.AspNetCore.Mvc;

namespace RaspberryPi.API.Controllers
{
    /// <summary>
    /// Log related methods
    /// </summary>
    [ApiController]
    [Route("[controller]/[action]")]
    public class LogController : ControllerBase
    {
        private readonly ILogger _logger;

        public LogController(ILogger<LogController> logger)
        {
                _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Logs message at information level
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Info(string message)
        {
            _logger.LogInformation(message);

            return NoContent();
        }

        /// <summary>
        /// Logs message at warning level
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Warn(string message)
        {
            _logger.LogWarning(message);

            return NoContent();
        }

        /// <summary>
        /// Logs message at error level
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Error(string message)
        {
            _logger.LogError(message);

            return NoContent();
        }
    }
}