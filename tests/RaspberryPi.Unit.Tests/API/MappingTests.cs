using AutoMapper;
using RaspberryPi.API.AutoMapper;
using RaspberryPi.API.Models.ViewModels;
using RaspberryPi.Application.Models.Dtos;
using RaspberryPi.Infrastructure.Models.Emails;
using RaspberryPi.Infrastructure.Models.Facts;

namespace RaspberryPi.Unit.Tests.API;

public class MappingTests
{
    private readonly IMapper _mapper;

    public MappingTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();
    }

    [Fact]
    public void Should_Map_FactInfraResponse_To_FactViewModel()
    {
        // Arrange
        var source = new FactInfraResponse { Text = "Test Fact" };

        // Act
        var result = _mapper.Map<FactViewModel>(source);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(source.Text, result.Fact);
    }

    [Fact]
    public void Should_Map_FactViewModel_To_FactInfraResponse()
    {
        // Arrange
        var source = new FactViewModel { Fact = "Reverse Fact" };

        // Act
        var result = _mapper.Map<FactInfraResponse>(source);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(source.Fact, result.Text);
    }

    [Fact]
    public void Should_Map_WeatherDto_To_WeatherViewModel()
    {
        // Arrange
        var source = new WeatherDto
        {
            EnglishName = "London",
            CountryCode = "GB",
            WeatherText = "Cloudy",
            Temperature = "15"
        };

        // Act
        var result = _mapper.Map<WeatherViewModel>(source);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(source.EnglishName, result.EnglishName);
        Assert.Equal(source.CountryCode, result.CountryCode);
        Assert.Equal(source.WeatherText, result.WeatherText);
        Assert.Equal(source.Temperature, result.Temperature);
    }

    [Fact]
    public void Should_Map_EmailViewModel_To_EmailDto()
    {
        // Arrange
        var source = new EmailViewModel
        {
            To = "test@example.com",
            Subject = "Subject",
            Body = "Body",
            IsBodyHtml = true
        };

        // Act
        var result = _mapper.Map<EmailDto>(source);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(source.To, result.To);
        Assert.Equal(source.Subject, result.Subject);
        Assert.Equal(source.Body, result.Body);
        Assert.Equal(source.IsBodyHtml, result.IsBodyHtml);
    }

    [Fact]
    public void Should_Map_EmailDto_To_Email()
    {
        // Arrange
        var source = new EmailDto
        {
            To = "test2@example.com",
            Subject = "Subject2",
            Body = "Body2",
            IsBodyHtml = false
        };

        // Act
        var result = _mapper.Map<Email>(source);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(source.To, result.To);
        Assert.Equal(source.Subject, result.Subject);
        Assert.Equal(source.Body, result.Body);
        Assert.Equal(source.IsBodyHtml, result.IsBodyHtml);
    }
}