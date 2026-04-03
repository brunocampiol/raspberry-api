using RaspberryPi.API.Mapping;
using RaspberryPi.API.Models.ViewModels;
using RaspberryPi.Application.Mapping;
using RaspberryPi.Application.Models.Dtos;
using RaspberryPi.Infrastructure.Models.Facts;

namespace RaspberryPi.Unit.Tests.API;

public class MappingTests
{

    [Fact]
    public void Should_Map_FactInfraResponse_To_FactViewModel()
    {
        // Arrange
        var source = new FactInfraResponse { Text = "Test Fact" };

        // Act
        var result = source.MapToFactViewModel();

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
        var result = source.MapToFactInfraResponse();

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
        var result = source.MapToWeatherViewModel();

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
        var result = source.MapToEmailDto();

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
        var result = source.MapToEmail();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(source.To, result.To);
        Assert.Equal(source.Subject, result.Subject);
        Assert.Equal(source.Body, result.Body);
        Assert.Equal(source.IsBodyHtml, result.IsBodyHtml);
    }

    [Fact]
    public void Should_Map_IEnumerable_FactInfraResponse_To_FactViewModel()
    {
        var source = new List<FactInfraResponse>
        {
            new FactInfraResponse { Text = "Fact 1" },
            new FactInfraResponse { Text = "Fact 2" }
        };

        var result = source.Select(x => x.MapToFactViewModel());

        Assert.NotNull(result);
        var resultList = result.ToList();
        Assert.Equal(source.Count, resultList.Count);
        for (int i = 0; i < source.Count; i++)
        {
            Assert.Equal(source[i].Text, resultList[i].Fact);
        }
    }

    [Fact]
    public void Should_Map_List_FactViewModel_To_List_FactInfraResponse()
    {
        var source = new List<FactViewModel>
        {
            new FactViewModel { Fact = "Fact A" },
            new FactViewModel { Fact = "Fact B" }
        };

        var result = source.Select(x => x.MapToFactInfraResponse()).ToList();

        Assert.NotNull(result);
        Assert.Equal(source.Count, result.Count);
        for (int i = 0; i < source.Count; i++)
        {
            Assert.Equal(source[i].Fact, result[i].Text);
        }
    }

    [Fact]
    public void Should_Map_IEnumerable_WeatherDto_To_WeatherViewModel()
    {
        var source = new List<WeatherDto>
        {
            new WeatherDto { EnglishName = "City1", CountryCode = "C1", WeatherText = "Sunny", Temperature = "20" },
            new WeatherDto { EnglishName = "City2", CountryCode = "C2", WeatherText = "Rainy", Temperature = "10" }
        };

        var result = source.Select(x => x.MapToWeatherViewModel());

        Assert.NotNull(result);
        var resultList = result.ToList();
        Assert.Equal(source.Count, resultList.Count);
        for (int i = 0; i < source.Count; i++)
        {
            Assert.Equal(source[i].EnglishName, resultList[i].EnglishName);
            Assert.Equal(source[i].CountryCode, resultList[i].CountryCode);
            Assert.Equal(source[i].WeatherText, resultList[i].WeatherText);
            Assert.Equal(source[i].Temperature, resultList[i].Temperature);
        }
    }

    [Fact]
    public void Should_Map_List_EmailViewModel_To_List_EmailDto()
    {
        var source = new List<EmailViewModel>
        {
            new EmailViewModel { To = "a@b.com", Subject = "S1", Body = "B1", IsBodyHtml = true },
            new EmailViewModel { To = "c@d.com", Subject = "S2", Body = "B2", IsBodyHtml = false }
        };

        var result = source.Select(x => x.MapToEmailDto()).ToList();

        Assert.NotNull(result);
        Assert.Equal(source.Count, result.Count);
        for (int i = 0; i < source.Count; i++)
        {
            Assert.Equal(source[i].To, result[i].To);
            Assert.Equal(source[i].Subject, result[i].Subject);
            Assert.Equal(source[i].Body, result[i].Body);
            Assert.Equal(source[i].IsBodyHtml, result[i].IsBodyHtml);
        }
    }
}
