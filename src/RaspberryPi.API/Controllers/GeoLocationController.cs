﻿using Microsoft.AspNetCore.Mvc;
using RaspberryPi.Application.Interfaces;
using RaspberryPi.Domain.Models.Entity;
using RaspberryPi.Infrastructure.Models.GeoLocation;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace RaspberryPi.API.Controllers
{
    /// <summary>
    /// API IP service related methods
    /// </summary>
    [ApiController]
    [Route("[controller]/[action]")]
    public class GeoLocationController : ControllerBase
    {
        private readonly IGeoLocationAppService _appService;

        public GeoLocationController(IGeoLocationAppService appService)
        {
            _appService = appService;
        }

        /// <summary>
        /// Returns geolocation data based on given IP address
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<LookUpInfraDto> LookUp([FromQuery][Required] string ipAddress)
        {
            return await _appService.LookUpAsync(ipAddress);
        }

        /// <summary>
        /// Returns geolocation data based on user IP address
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<LookUpInfraDto> LookUpFromContextIpAddress()
        {
            var clientIp = HttpContext.Connection.RemoteIpAddress.ToString();
            string forwardedHeader = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrEmpty(forwardedHeader))
            {
                clientIp = forwardedHeader.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                          .Select(ip => ip.Trim())
                                          .FirstOrDefault(ip => !IPAddress.IsLoopback(IPAddress.Parse(ip)));
            }
            return await _appService.LookUpAsync(clientIp);
        }

        /// <summary>
        /// Returns geolocation data for a random IP address
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<LookUpInfraDto> LookUpFromRandomIpAddress()
        {
            return await _appService.LookUpFromRandomIpAddressAsync();
        }

        /// <summary>
        /// Gets all geo location data from database
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<GeoLocation>> GetAllGeoLocationsFromDatabase()
        {
            return await _appService.GetAllGeoLocationsFromDatabaseAsync();
        }
    }
}