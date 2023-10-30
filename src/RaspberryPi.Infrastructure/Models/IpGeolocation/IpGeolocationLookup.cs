using System.Text.Json.Serialization;

namespace RaspberryPi.Infrastructure.Models.IpGeolocation
{
    public class IpGeoLocationLookup
    {
        [JsonPropertyName("ip")]
        public string Ip { get; set; }

        [JsonPropertyName("hostname")]
        public string Hostname { get; set; }

        [JsonPropertyName("continent_code")]
        public string ContinentCode { get; set; }

        [JsonPropertyName("continent_name")]
        public string ContinentName { get; set; }

        [JsonPropertyName("country_code2")]
        public string CountryCode2 { get; set; }

        [JsonPropertyName("country_code3")]
        public string CountryCode3 { get; set; }

        [JsonPropertyName("country_name")]
        public string CountryName { get; set; }

        [JsonPropertyName("country_capital")]
        public string CountryCapital { get; set; }

        [JsonPropertyName("state_prov")]
        public string StateProv { get; set; }

        [JsonPropertyName("district")]
        public string District { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("zipcode")]
        public string Zipcode { get; set; }

        [JsonPropertyName("latitude")]
        public string Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public string Longitude { get; set; }

        [JsonPropertyName("is_eu")]
        public bool IsEu { get; set; }

        [JsonPropertyName("calling_code")]
        public string CallingCode { get; set; }

        [JsonPropertyName("country_tld")]
        public string CountryTld { get; set; }

        [JsonPropertyName("languages")]
        public string Languages { get; set; }

        [JsonPropertyName("country_flag")]
        public string CountryFlag { get; set; }

        [JsonPropertyName("geoname_id")]
        public string GeonameId { get; set; }

        [JsonPropertyName("isp")]
        public string Isp { get; set; }

        [JsonPropertyName("connection_type")]
        public string ConnectionType { get; set; }

        [JsonPropertyName("organization")]
        public string Organization { get; set; }

        [JsonPropertyName("asn")]
        public string Asn { get; set; }

        [JsonPropertyName("currency")]
        public Currency Currency { get; set; }

        [JsonPropertyName("time_zone")]
        public TimeZone TimeZone { get; set; }
    }

    public class Currency
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }
    }

    public class TimeZone
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("offset")]
        public int Offset { get; set; }

        [JsonPropertyName("current_time")]
        public string CurrentTime { get; set; }

        [JsonPropertyName("current_time_unix")]
        public double CurrentTimeUnix { get; set; }

        [JsonPropertyName("is_dst")]
        public bool IsDst { get; set; }

        [JsonPropertyName("dst_savings")]
        public int DstSavings { get; set; }
    }
}