using RaspberryPi.API.Mapping;
using RaspberryPi.Application.Models.Dtos;

namespace RaspberryPi.Unit.Tests.API;

public class WeatherMappingExtensionsTests
{
    [Fact]
    public void MapToWeatherViewModel_MapsAllProperties()
    {
        // Arrange
        var dto = new WeatherDto
        {
            EnglishName = "London",
            CountryCode = "GB",
            WeatherText = "Cloudy",
            Temperature = "12°C"
        };

        // Act
        var vm = dto.MapToWeatherViewModel();

        // Assert
        Assert.NotNull(vm);
        Assert.Equal(dto.EnglishName, vm.LocationName);
        Assert.Equal(dto.CountryCode, vm.CountryCode);
        Assert.Equal(dto.WeatherText, vm.WeatherText);
        Assert.Equal(dto.Temperature, vm.Temperature);
    }

    [Fact]
    public void MapToWeatherViewModel_NotAvailable_MapsNA()
    {
        // Arrange
        var dto = WeatherDto.NotAvailable();

        // Act
        var vm = dto.MapToWeatherViewModel();

        // Assert
        Assert.NotNull(vm);
        Assert.Equal("N/A", vm.LocationName);
        Assert.Equal("N/A", vm.CountryCode);
        Assert.Equal("N/A", vm.WeatherText);
        Assert.Equal("N/A", vm.Temperature);
    }
}
