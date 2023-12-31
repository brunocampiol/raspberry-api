﻿using Microsoft.AspNetCore.Mvc;

namespace RaspberryPi.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LogController : ControllerBase
    {
        private readonly ILogger _logger;

        public LogController(ILogger<LogController> logger)
        {
                _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("info/{message}")]
        public IActionResult Info(string message)
        {
            _logger.LogInformation(message);

            return NoContent();
        }

        [HttpGet("warn/{message}")]
        public IActionResult Warn(string message)
        {
            _logger.LogWarning(message);

            return NoContent();
        }

        [HttpGet("error/{message}")]
        public IActionResult Error(string message)
        {
            _logger.LogError(message);

            return NoContent();
        }
    }
}