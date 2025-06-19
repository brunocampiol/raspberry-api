using System.Text.Json.Serialization;

namespace RaspberryPi.Infrastructure.Models.GeoLocation;

public record LookUpInfraDto
{
    [JsonPropertyName("ip")]
    public string Ip { get; set; }

    [JsonPropertyName("continentCode")]
    public string ContinentCode { get; set; }

    [JsonPropertyName("continentName")]
    public string ContinentName { get; set; }

    [JsonPropertyName("countryCode")]
    public string CountryCode { get; set; }

    [JsonPropertyName("countryName")]
    public string CountryName { get; set; }

    [JsonPropertyName("countryNameNative")]
    public string CountryNameNative { get; set; }

    [JsonPropertyName("officialCountryName")]
    public string OfficialCountryName { get; set; }

    [JsonPropertyName("regionCode")]
    public string RegionCode { get; set; }

    [JsonPropertyName("regionName")]
    public string RegionName { get; set; }

    [JsonPropertyName("city")]
    public string City { get; set; }

    [JsonPropertyName("postalCode")]
    public string PostalCode { get; set; }

    [JsonPropertyName("latitude")]
    public double Latitude { get; set; }

    [JsonPropertyName("longitude")]
    public double Longitude { get; set; }

    [JsonPropertyName("capital")]
    public string Capital { get; set; }

    [JsonPropertyName("phoneCode")]
    public string PhoneCode { get; set; }

    [JsonPropertyName("countryFlagEmoj")]
    public string CountryFlagEmoj { get; set; }

    [JsonPropertyName("countryFlagEmojUnicode")]
    public string CountryFlagEmojUnicode { get; set; }

    [JsonPropertyName("isEu")]
    public bool IsEu { get; set; }

    [JsonPropertyName("borders")]
    public string[] Borders { get; set; }

    [JsonPropertyName("topLevelDomains")]
    public string[] TopLevelDomains { get; set; }

    [JsonPropertyName("languages")]
    public Languages Languages { get; set; }

    [JsonPropertyName("currency")]
    public Currency Currency { get; set; }

    [JsonPropertyName("timeZone")]
    public TimeZone TimeZone { get; set; }

    [JsonPropertyName("userAgent")]
    public UserAgent UserAgent { get; set; }

    [JsonPropertyName("connection")]
    public Connection Connection { get; set; }

    [JsonPropertyName("security")]
    public Security Security { get; set; }
}

public class Languages
{
    [JsonPropertyName("en")]
    public Language English { get; set; }
}

public class Language
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("native")]
    public string Native { get; set; }
}

public class Currency
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("symbol")]
    public string Symbol { get; set; }

    [JsonPropertyName("number")]
    public string Number { get; set; }

    [JsonPropertyName("rates")]
    public Rates Rates { get; set; }
}

public class Rates
{
    [JsonPropertyName("EURUSD")]
    public double EurUsd { get; set; }
}

public class TimeZone
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("currentTime")]
    public string CurrentTime { get; set; }

    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("timeZoneName")]
    public string TimeZoneName { get; set; }

    [JsonPropertyName("utcOffset")]
    public int UtcOffset { get; set; }
}

public class UserAgent
{
    [JsonPropertyName("isMobile")]
    public bool IsMobile { get; set; }

    [JsonPropertyName("isiPod")]
    public bool IsiPod { get; set; }

    [JsonPropertyName("isTablet")]
    public bool IsTablet { get; set; }

    [JsonPropertyName("isDesktop")]
    public bool IsDesktop { get; set; }

    [JsonPropertyName("isSmartTV")]
    public bool IsSmartTV { get; set; }

    [JsonPropertyName("isRaspberry")]
    public bool IsRaspberry { get; set; }

    [JsonPropertyName("isBot")]
    public bool IsBot { get; set; }

    [JsonPropertyName("browser")]
    public string Browser { get; set; }

    [JsonPropertyName("browserVersion")]
    public string BrowserVersion { get; set; }

    [JsonPropertyName("operatingSystem")]
    public string OperatingSystem { get; set; }

    [JsonPropertyName("platform")]
    public string Platform { get; set; }

    [JsonPropertyName("source")]
    public string Source { get; set; }
}

public class Connection
{
    [JsonPropertyName("asn")]
    public int Asn { get; set; }

    [JsonPropertyName("isp")]
    public string Isp { get; set; }
}

public class Security
{
    [JsonPropertyName("isProxy")]
    public bool IsProxy { get; set; }

    [JsonPropertyName("isBogon")]
    public bool IsBogon { get; set; }

    [JsonPropertyName("isTorExitNode")]
    public bool IsTorExitNode { get; set; }

    [JsonPropertyName("isCloud")]
    public bool IsCloud { get; set; }

    [JsonPropertyName("isHosting")]
    public bool IsHosting { get; set; }

    [JsonPropertyName("isSpamhaus")]
    public bool IsSpamhaus { get; set; }

    [JsonPropertyName("suggestion")]
    public string Suggestion { get; set; }

    [JsonPropertyName("network")]
    public string Network { get; set; }
}