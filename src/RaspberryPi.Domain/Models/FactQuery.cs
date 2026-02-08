namespace RaspberryPi.Domain.Models;

public sealed record FactQuery(
    string? TextContains,
    DateTime? CreatedFromUtc,
    DateTime? CreatedToUtc,
    int Page = 1,
    int PageSize = 20);