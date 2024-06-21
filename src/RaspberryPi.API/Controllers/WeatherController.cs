using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RaspberryPi.API.Models.ViewModels;
using RaspberryPi.Application.Interfaces;
using System.Net;

namespace RaspberryPi.API.Controllers
{
    /// <summary>
    /// AccuWeather service related methods 
    /// </summary>
    [ApiController]
    [Route("[controller]/[action]")]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherAppService _service;
        private readonly IMapper _mapper;

        public WeatherController(IWeatherAppService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns weather data from given IP address
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<WeatherViewModel> FromIpAddress(string ipAddress)
        {            
            var result = await _service.GetWeatherFromIpAddress(ipAddress);
            var viewModel = _mapper.Map<WeatherViewModel>(result);
            return viewModel;
        }

        /// <summary>
        /// Returns weather data from user IP address
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<WeatherViewModel> FromContextIpAddress()
        {
            var clientIp = HttpContext.Connection.RemoteIpAddress.ToString();
            string forwardedHeader = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrEmpty(forwardedHeader))
            {
                clientIp = forwardedHeader.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                          .Select(ip => ip.Trim())
                                          .FirstOrDefault(ip => !IPAddress.IsLoopback(IPAddress.Parse(ip)));
            }

            var result = await _service.GetWeatherFromIpAddress(clientIp);
            var viewModel = _mapper.Map<WeatherViewModel>(result);
            return viewModel;
        }

        /// <summary>
        /// Returns weather data from random IP address
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<WeatherViewModel> FromRandomIpAddress()
        {
            var result = await _service.GetWeatherFromRandomIpAddressAsync();
            var viewModel = _mapper.Map<WeatherViewModel>(result);
            return viewModel;
        }
    }
}