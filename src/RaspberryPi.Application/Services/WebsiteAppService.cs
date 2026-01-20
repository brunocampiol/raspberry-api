using RaspberryPi.Application.Interfaces;
using RaspberryPi.Application.Models.Dtos;
using RaspberryPi.Domain.Models.Entity;
using RaspberryPi.Infrastructure.Models.Facts;

namespace RaspberryPi.Application.Services;

public sealed class WebsiteAppService : IWebsiteAppService
{
    private readonly IWeatherAppService _weatherAppService;
    private readonly IFactAppService _factAppService;

    public WebsiteAppService(IWeatherAppService weatherAppService,
                                    IFactAppService factAppService)
    {
        _weatherAppService = weatherAppService ?? throw new ArgumentNullException(nameof(weatherAppService));
        _factAppService = factAppService ?? throw new ArgumentNullException(nameof(factAppService));
    }

    public async Task<FactInfraResponse> FetchAndStoreUniqueFactAsync()
    {
        return await _factAppService.FetchAndStoreUniqueFactAsync();
    }

    /// <summary>
    /// Retrieves weather information for a specified IP address.
    /// </summary>
    /// <param name="ipAddress"></param>
    /// <returns></returns>
    /// /// <para>
    /// This method performs the following steps:
    /// 1. Validates the IP address input
    /// 2. Looks up geolocation information for the IP address (with caching)
    /// 3. Validates that a country code and location name can be determined
    /// 4. Retrieves or creates a <see cref="GeoLocation"/> entity from the repository
    /// 5. Sends a notification email for newly discovered locations
    /// 6. Retrieves weather conditions for the location (with caching)
    /// 7. Returns formatted weather information
    /// </para>
    public async Task<WeatherDto> GetWeatherAsync(string ipAddress)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(ipAddress);
        return await _weatherAppService.GetWeatherWorkflowAsync(ipAddress);
    }
}