using System.Text.Json.Serialization;

namespace RaspberryPi.Infrastructure.Models.Weather;

public class WeatherInfraResponse
{
    [JsonPropertyName("coord")]
    public Coordinates? Coordinates { get; set; }

    [JsonPropertyName("weather")]
    public WeatherCondition[]? Weather { get; set; }

    [JsonPropertyName("base")]
    public string? Base { get; set; }

    [JsonPropertyName("main")]
    public MainWeatherData? Main { get; set; }

    [JsonPropertyName("visibility")]
    public int Visibility { get; set; }

    [JsonPropertyName("wind")]
    public WindData? Wind { get; set; }

    [JsonPropertyName("clouds")]
    public CloudData? Clouds { get; set; }

    [JsonPropertyName("dt")]
    public long DateTimeUnix { get; set; }

    [JsonPropertyName("sys")]
    public SystemData? System { get; set; }

    [JsonPropertyName("timezone")]
    public int TimezoneOffset { get; set; }

    [JsonPropertyName("id")]
    public int CityId { get; set; }

    [JsonPropertyName("name")]
    public string? CityName { get; set; }

    [JsonPropertyName("cod")]
    public int ResponseCode { get; set; }
}

public class Coordinates
{
    [JsonPropertyName("lon")]
    public double Longitude { get; set; }

    [JsonPropertyName("lat")]
    public double Latitude { get; set; }
}

public class WeatherCondition
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("main")]
    public string? Main { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("icon")]
    public string? Icon { get; set; }
}

public class MainWeatherData
{
    [JsonPropertyName("temp")]
    public double Temperature { get; set; }

    [JsonPropertyName("feels_like")]
    public double FeelsLike { get; set; }

    [JsonPropertyName("temp_min")]
    public double TemperatureMin { get; set; }

    [JsonPropertyName("temp_max")]
    public double TemperatureMax { get; set; }

    [JsonPropertyName("pressure")]
    public int Pressure { get; set; }

    [JsonPropertyName("humidity")]
    public int Humidity { get; set; }

    [JsonPropertyName("sea_level")]
    public int SeaLevel { get; set; }

    [JsonPropertyName("grnd_level")]
    public int GroundLevel { get; set; }
}

public class WindData
{
    [JsonPropertyName("speed")]
    public double Speed { get; set; }

    [JsonPropertyName("deg")]
    public int Direction { get; set; }

    [JsonPropertyName("gust")]
    public double Gust { get; set; }
}

public class CloudData
{
    [JsonPropertyName("all")]
    public int Cloudiness { get; set; }
}

public class SystemData
{
    [JsonPropertyName("country")]
    public string? CountryCode { get; set; }

    [JsonPropertyName("sunrise")]
    public long SunriseUnix { get; set; }

    [JsonPropertyName("sunset")]
    public long SunsetUnix { get; set; }
}