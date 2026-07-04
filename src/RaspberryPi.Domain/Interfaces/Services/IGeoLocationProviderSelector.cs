namespace RaspberryPi.Domain.Interfaces.Services;

public interface IGeoLocationProviderSelector
{
    IGeoLocationProvider? GetNextAvailableProvider();
}