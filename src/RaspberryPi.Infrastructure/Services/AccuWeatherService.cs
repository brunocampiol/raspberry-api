using Microsoft.Extensions.Options;
using RaspberryPi.Domain.Core;
using RaspberryPi.Infrastructure.Interfaces;
using RaspberryPi.Infrastructure.Models.Options;

namespace RaspberryPi.Infrastructure.Services
{
    public class AccuWeatherService : IAccuWeatherService
    {
        private readonly AccuWeatherOptions _settings;
        private readonly IHttpClientFactory _httpClientFactory;

        public AccuWeatherService(IOptions<AccuWeatherOptions> settings,
                                  IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> LocationIpAddressSearchAsync(string ipAddress)
        {
            // http://dataservice.accuweather.com/locations/v1/cities/ipaddress
            const string endpoint = "locations/v1/cities/ipaddress";
            var httpClient = _httpClientFactory.CreateClient();
            var url = $"{_settings.BaseUrl}{endpoint}?apiKey={_settings.ApiKey}&q={ipAddress}";

            var httpResponse = await httpClient.GetAsync(url);
            var httpContent = await httpResponse.Content.ReadAsStringAsync();

            if (!httpResponse.IsSuccessStatusCode)
            {
                var errorMessage = $"Failed to get LocationIpAddressSearchAsync. " +
                                   $"The HTTP response '{httpResponse.StatusCode}' " +
                                   $"is not in 2XX range for '{url}'. Received " +
                                   $"content is '{httpContent}'";
                throw new AppException(errorMessage);
            }

            return httpContent;
        }

        //        {
        //        {
        //  "Version": 1,
        //  "Key": "36318",
        //  "Type": "City",
        //  "Rank": 65,
        //  "LocalizedName": "Pari",
        //  "EnglishName": "Pari",
        //  "PrimaryPostalCode": "",
        //  "Region": {
        //    "ID": "SAM",
        //    "LocalizedName": "South America",
        //    "EnglishName": "South America"
        //  },
        //  "Country": {
        //    "ID": "BR",
        //    "LocalizedName": "Brazil",
        //    "EnglishName": "Brazil"
        //  },
        //  "AdministrativeArea": {
        //    "ID": "SP",
        //    "LocalizedName": "São Paulo",
        //    "EnglishName": "São Paulo",
        //    "Level": 1,
        //    "LocalizedType": "State",
        //    "EnglishType": "State",
        //    "CountryID": "BR"
        //  },
        //  "TimeZone": {
        //    "Code": "BRT",
        //    "Name": "America/Sao_Paulo",
        //    "GmtOffset": -3,
        //    "IsDaylightSaving": false,
        //    "NextOffsetChange": null
        //  },
        //  "GeoPosition": {
        //    "Latitude": -23.533,
        //    "Longitude": -46.624,
        //    "Elevation": {
        //      "Metric": {
        //        "Value": 636,
        //        "Unit": "m",
        //        "UnitType": 5
        //      },
        //      "Imperial": {
        //        "Value": 2086,
        //        "Unit": "ft",
        //        "UnitType": 0
        //      }
        //    }
        //  },
        //  "IsAlias": false,
        //  "ParentCity": {
        //    "Key": "45881",
        //    "LocalizedName": "São Paulo",
        //    "EnglishName": "São Paulo"
        //  },
        //  "SupplementalAdminAreas": [
        //    {
        //      "Level": 2,
        //      "LocalizedName": "São Paulo",
        //      "EnglishName": "São Paulo"
        //    },
        //    {
        //      "Level": 3,
        //      "LocalizedName": "Pari",
        //      "EnglishName": "Pari"
        //    }
        //  ],
        //  "DataSets": [
        //    "AirQualityCurrentConditions",
        //    "AirQualityForecasts",
        //    "Alerts",
        //    "FutureRadar",
        //    "MinuteCast",
        //    "Radar"
        //  ]
        //}

        public async Task<string> CurrentConditions(int key)
        {
            // http://dataservice.accuweather.com/currentconditions/v1/{key}
            const string endpoint = "currentconditions/v1/";
            var httpClient = _httpClientFactory.CreateClient();
            var url = $"{_settings.BaseUrl}{endpoint}{key}?apiKey={_settings.ApiKey}";

            var httpResponse = await httpClient.GetAsync(url);
            var httpContent = await httpResponse.Content.ReadAsStringAsync();

            if (!httpResponse.IsSuccessStatusCode)
            {
                var errorMessage = $"Failed to get LocationIpAddressSearchAsync. " +
                                   $"The HTTP response '{httpResponse.StatusCode}' " +
                                   $"is not in 2XX range for '{url}'. Received " +
                                   $"content is '{httpContent}'";
                throw new AppException(errorMessage);
            }

            return httpContent;
        }

        //        [
        //  {
        //    "LocalObservationDateTime": "2023-10-18T16:20:00-03:00",
        //    "EpochTime": 1697656800,
        //    "WeatherText": "Cloudy",
        //    "WeatherIcon": 7,
        //    "HasPrecipitation": false,
        //    "PrecipitationType": null,
        //    "IsDayTime": true,
        //    "Temperature": {
        //      "Metric": {
        //        "Value": 21.7,
        //        "Unit": "C",
        //        "UnitType": 17
        //      },
        //      "Imperial": {
        //        "Value": 71,
        //        "Unit": "F",
        //        "UnitType": 18
        //      }
        //    },
        //    "MobileLink": "http://www.accuweather.com/en/br/pari/36318/current-weather/36318?lang=en-us",
        //    "Link": "http://www.accuweather.com/en/br/pari/36318/current-weather/36318?lang=en-us"
        //  }
        //]
    }
}
