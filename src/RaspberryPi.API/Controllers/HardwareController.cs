using Microsoft.AspNetCore.Mvc;
using OpenHardwareMonitor.Hardware;
using RaspberryPi.Application.Interfaces;
using System.Text;

namespace RaspberryPi.API.Controllers
{
    /// <summary>
    /// Raspberry Pi 2 and hardware specific methods
    /// </summary>
    [ApiController]
    [Route("[controller]/[action]")]
    public class HardwareController : ControllerBase
    {
        // TODO: update to libre hardware monitor
        // https://github.com/LibreHardwareMonitor/LibreHardwareMonitor

        private readonly IHardwareAppService _service;
   
        public HardwareController(IHardwareAppService hardwareAppService)
        {
            _service = hardwareAppService;
        }

        /// <summary>
        /// Lists current hardware components
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult List()
        {
            var sb = new StringBuilder();
            var computer = new Computer()
            {
                CPUEnabled = true,
                HDDEnabled = true,
                GPUEnabled = true,
                FanControllerEnabled = true,
                NICEnabled = true,
                RAMEnabled = true,
                MainboardEnabled = true,
            };
            computer.Open();

            foreach (var hardwareItem in computer.Hardware)
            {
                sb.AppendLine($"{hardwareItem.HardwareType}: {hardwareItem.Name}");
            }

            computer.Close();

            return Ok(sb.ToString());
        }

        /// <summary>
        /// Returns the Open Hardware Monitor report
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Report()
        {
            var computer = new Computer()
            {
                CPUEnabled = true,
                HDDEnabled = true,
                GPUEnabled = true,
                FanControllerEnabled = true,
                NICEnabled = true,
                RAMEnabled = true,
                MainboardEnabled = true,
            };
            computer.Open();

            var report = computer.GetReport();
            computer.Close();
            return Ok(report);
        }

        /// <summary>
        /// Blinks a LED in GPIO 26
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult BlinkLedGpio26()
        {
            _service.BlinkLedGpio26();
            return NoContent();
        }

        /// <summary>
        /// Reads GPIO 26
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ReadGpio26()
        {
            var result = _service.ReadGpio26();
            return Ok(result);
        }
    }
}
