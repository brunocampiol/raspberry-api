namespace RaspberryPi.Infrastructure.Models.AccuWeather
{
    public class AccuWeatherCurrentConditionsResponse
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

    public class Temperature
    {
        public Metric Metric { get; init; }

        public Imperial Imperial { get; init; }
    }

    public class Metric
    {
        public double Value { get; init; }
        public string Unit { get; init; }
        public int UnitType { get; init; }
    }

    public class Imperial
    {
        public double Value { get; init; }
        public string Unit { get; init; }
        public int UnitType { get; init; }
    }
}