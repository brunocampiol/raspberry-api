namespace RaspberryPi.Infrastructure.Models.Weather
{
    public class WeatherCurrentConditionsInfraDto
    {
        public string LocalObservationDateTime { get; init; }

        public int EpochTime { get; init; }

        public string WeatherText { get; init; }

        public int WeatherIcon { get; init; }

        public bool HasPrecipitation { get; init; }

        public string PrecipitationType { get; init; }

        public bool IsDayTime { get; init; }

        public Temperature Temperature { get; init; }

        public string MobileLink { get; init; }

        public string Link { get; init; }
    }
}