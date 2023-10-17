using Microsoft.AspNetCore.Mvc;
using OpenHardwareMonitor.Hardware;
using System.Text;

namespace RaspberryPi.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class HardwareController : ControllerBase
    {
        public HardwareController()
        {

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

        // TODO fix temperature
        [HttpGet("temperature")]
        public IActionResult Cpu()
        {
            var sb = new StringBuilder();

            var computer = new Computer()
            {
                CPUEnabled = true
            };
            computer.Open();

            foreach (var hardwareItem in computer.Hardware)
            {
                if (hardwareItem.HardwareType == HardwareType.CPU)
                {
                    sb.Append($"CPU {hardwareItem.Name}");
                    sb.Append(Environment.NewLine);

                    hardwareItem.Update();
                    foreach (var sensor in hardwareItem.Sensors)
                    {
                        if (sensor.SensorType == SensorType.Temperature)
                        {
                            sb.Append($"CPU ({sensor.Index}) Temperature: {sensor.Value} °C");
                            sb.Append(Environment.NewLine);
                        }
                    }
                }
            }

            computer.Close();

            return Ok(sb.ToString());
        }

        // TODO fix gpu temperature
        [HttpGet]
        public IActionResult GpuTemp()
        {
            var sb = new StringBuilder();

            var computer = new Computer()
            {
                GPUEnabled = true
            };
            computer.Open();

            foreach (var hardwareItem in computer.Hardware)
            {

                if (hardwareItem.HardwareType == HardwareType.GpuNvidia)
                {
                    sb.Append($"CPU {hardwareItem.Name}");
                    sb.Append(Environment.NewLine);

                    hardwareItem.Update();
                    foreach (var sensor in hardwareItem.Sensors)
                    {
                        if (sensor.SensorType == SensorType.Temperature)
                        {
                            sb.Append($"CPU ({sensor.Index}) Temperature: {sensor.Value} °C");
                            sb.Append(Environment.NewLine);
                        }
                    }
                }
            }

            computer.Close();

            return Ok(sb.ToString());
        }
    }
}
