using System.ComponentModel.DataAnnotations;

namespace RaspberryPi.API.Models.ViewModels;

public record EmailViewModel
{
    [EmailAddress]
    public required string To { get; init; }
    public required string Subject { get; init; }
    public required string Body { get; init; }
}