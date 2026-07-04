using MethodTimer;
using Microsoft.AspNetCore.Mvc;
using RaspberryPi.API.Extensions;
using RaspberryPi.Application.Interfaces;
using RaspberryPi.Domain.Models;
using RaspberryPi.Domain.Models.Entity;
using System.ComponentModel.DataAnnotations;

namespace RaspberryPi.API.Controllers;

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
    [Time]
    [HttpGet]
    public async Task<GeoLocationResult> LookUp([FromQuery][Required] string ipAddress)
    {
        return await _appService.LookUpAsync(ipAddress);
    }

    /// <summary>
    /// Returns geolocation data based on given IP address using ApiIp service
    /// </summary>
    /// <param name="ipAddress"></param>
    /// <returns></returns>
    [Time]
    [HttpGet]
    public async Task<GeoLocationResult> LookUpApiIpAsync([FromQuery][Required] string ipAddress)
    {
        return await _appService.LookUpApiIpAsync(ipAddress);
    }

    /// <summary>
    /// Returns geolocation data based on given IP address using FreeIp service
    /// </summary>
    /// <param name="ipAddress"></param>
    /// <returns></returns>
    [Time]
    [HttpGet]
    public async Task<GeoLocationResult> LookUpFreeIpAsync([FromQuery][Required] string ipAddress)
    {
        return await _appService.LookUpFreeIpAsync(ipAddress);
    }

    /// <summary>
    /// Returns geolocation data based on given IP address using Ip2Location service
    /// </summary>
    /// <param name="ipAddress"></param>
    /// <returns></returns>
    [Time]
    [HttpGet]
    public async Task<GeoLocationResult> LookUpIp2LocationAsync([FromQuery][Required] string ipAddress)
    {
        return await _appService.LookUpIp2LocationAsync(ipAddress);
    }

    /// <summary>
    /// Returns geolocation data based on given IP address using IpGeoLocation service
    /// </summary>
    /// <param name="ipAddress"></param>
    /// <returns></returns>
    [Time]
    [HttpGet]
    public async Task<GeoLocationResult> LookUpIpGeoAsync([FromQuery][Required] string ipAddress)
    {
        return await _appService.LookUpIpGeoAsync(ipAddress);
    }

    /// <summary>
    /// Returns geolocation data based on user IP address
    /// </summary>
    /// <returns></returns>
    [Time]
    [HttpGet]
    public async Task<GeoLocationResult> LookUpFromContextIpAddress()
    {
        var clientIp = HttpContext.GetClientIpAddress();
        return await _appService.LookUpAsync(clientIp);
    }

    /// <summary>
    /// Returns geolocation data for a random IP address
    /// </summary>
    /// <returns></returns>
    [Time]
    [HttpGet]
    public async Task<GeoLocationResult> LookUpFromRandomIpAddress()
    {
        return await _appService.LookUpFromRandomIpAddressAsync();
    }

    /// <summary>
    /// Returns geolocation data for Google IP address
    /// </summary>
    /// <returns></returns>
    [Time]
    [HttpGet]
    public async Task<GeoLocationResult> LookUpFromGoogleIpAddress()
    {
        return await _appService.LookUpAsync("8.8.8.8");
    }

    /// <summary>
    /// Gets all geo location data from database
    /// </summary>
    /// <returns></returns>
    [Time]
    [HttpGet]
    public async Task<IEnumerable<GeoLocation>> GetAllGeoLocationsFromDatabase()
    {
        return await _appService.GetAllGeoLocationsFromDatabaseAsync();
    }
}