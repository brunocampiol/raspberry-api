namespace RaspberryPi.Infrastructure.Models.Weather
{
    public class Temperature
    {
        public Metric Metric { get; init; }

        public Imperial Imperial { get; init; }
    }
}