using Microsoft.AspNetCore.Mvc;
using OpenHardwareMonitor.Hardware;
using RaspberryPi.Application.Interfaces;
using RaspberryPi.Domain.Models;
using RaspberryPi.Domain.Services;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RaspberryPi.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class HardwareController : ControllerBase
    {
        // TODO: update to libre hardware monitor
        // https://github.com/LibreHardwareMonitor/LibreHardwareMonitor

        private readonly IHardwareAppService _gpioAppService;

        public HardwareController(IHardwareAppService gpioAppService)
        {
            _gpioAppService = gpioAppService;
        }

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

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult BlinkLedGpio26()
        {
            _gpioAppService.BlinkLedGpio26();
            return NoContent();
        }

        [HttpGet]
        public IActionResult ReadGpio26()
        {
            var result = _gpioAppService.ReadGpio26();
            return Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult Play()
        {
            var service = new BuzzerService();
            service.Play();
            return NoContent();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult PlayTones()
        {
            var service = new BuzzerService();
            service.PlayTones();
            return NoContent();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult PlayMusic([FromBody][Required] Music music)
        {
            var service = new BuzzerService();
            service.PlayMusic(music);
            return NoContent();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult PlayStarWarsTheme()
        {
            var service = new BuzzerService();
            service.PlayStarWarsTheme();
            return NoContent();
        }

        //[HttpGet("temperature")]
        //public IActionResult Cpu()
        //{
        //    var sb = new StringBuilder();

        //    var computer = new Computer()
        //    {
        //        CPUEnabled = true
        //    };
        //    computer.Open();
        //    var report = computer.GetReport();

        //    foreach (var hardwareItem in computer.Hardware)
        //    {
        //        if (hardwareItem.HardwareType == HardwareType.CPU)
        //        {
        //            sb.Append($"CPU {hardwareItem.Name}");
        //            sb.Append(Environment.NewLine);

        //            hardwareItem.Update();
        //            foreach (var sensor in hardwareItem.Sensors)
        //            {
        //                if (sensor.SensorType == SensorType.Temperature)
        //                {
        //                    sb.Append($"CPU ({sensor.Index}) Temperature: {sensor.Value} °C");
        //                    sb.Append(Environment.NewLine);
        //                }
        //            }
        //        }
        //    }

        //    computer.Close();

        //    return Ok(sb.ToString());
        //}

        //[HttpGet("temperature")]
        //public IActionResult Gpu()
        //{
        //    var sb = new StringBuilder();

        //    var computer = new Computer()
        //    {
        //        GPUEnabled = true
        //    };
        //    computer.Open();

        //    if (computer.Hardware.Length == 0)
        //    {
        //        sb.Append("There are no GPUs to display data");
        //    }

        //    foreach (var hardwareItem in computer.Hardware)
        //    {
        //        if (hardwareItem.HardwareType == HardwareType.GpuNvidia ||
        //            hardwareItem.HardwareType == HardwareType.GpuAti)
        //        {
        //            sb.Append($"GPU {hardwareItem.Name}");
        //            sb.Append(Environment.NewLine);

        //            hardwareItem.Update();
        //            foreach (var sensor in hardwareItem.Sensors)
        //            {
        //                if (sensor.SensorType == SensorType.Temperature)
        //                {
        //                    sb.Append($"GPU ({sensor.Index}) Temperature: {sensor.Value} °C");
        //                    sb.Append(Environment.NewLine);
        //                }
        //            }
        //        }
        //    }

        //    computer.Close();

        //    return Ok(sb.ToString());
        //}
    }
}
