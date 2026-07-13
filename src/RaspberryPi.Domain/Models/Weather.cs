namespace RaspberryPi.Domain.Models;

public class Weather
{
    public required double Latitude { get; init; }
    public required double Longitude { get; init; }
    public required string Description { get; set; }
    public double TemperatureCelcius { get; set; }
    public double TemperatureFahrenheit => TemperatureCelcius * 9 / 5 + 32;
}