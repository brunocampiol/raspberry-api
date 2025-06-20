﻿namespace RaspberryPi.API.Models.ViewModels;

public record WeatherViewModel
{
    public required string EnglishName { get; init; }
    public required string CountryCode { get; init; }
    public required string WeatherText { get; init; }
    public required string Temperature { get; init; }
}