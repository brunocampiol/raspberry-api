namespace RaspberryPi.Infrastructure.Models.Weather
{
    public class Metric
    {
        public double Value { get; init; }
        public string Unit { get; init; }
        public int UnitType { get; init; }
    }
}