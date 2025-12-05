namespace RaspberryPi.API.Models.ViewModels;

public class SettingsResponseViewModel
{
    public required string EnvironmentName { get; init; }
    public required string UserName { get; init; }
    public required string OSDescription { get; init; }
    public string? ASPNetCore_Environment { get; init; }
    public string? FrameworkName { get; init; }
    public bool IsDevelopment { get; init; }
    public bool IsProduction { get; init; }
}