namespace RaspberryPi.Infrastructure.Models.AccuWeather
{
    public class AccuWeatherCurrentConditionsResponse
    {
        public string LocalObservationDateTime { get; set; }

        public int EpochTime { get; set; }

        public string WeatherText { get; set; }

        public int WeatherIcon { get; set; }

        public bool HasPrecipitation { get; set; }

        public string PrecipitationType { get; set; }

        public bool IsDayTime { get; set; }

        public Temperature Temperature { get; set; }

        public string MobileLink { get; set; }

        public string Link { get; set; }
    }

    public class Temperature
    {
        public Metric Metric { get; set; }

        public Imperial Imperial { get; set; }
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