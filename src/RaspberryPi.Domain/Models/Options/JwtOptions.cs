﻿namespace RaspberryPi.Domain.Models.Options
{
    public sealed class JwtOptions
    {
        public const string SectionName = "Jwt";
        public string Issuer { get; init; } = default!;
        public string Audience { get; init; } = default!;
        public string Key { get; init; } = default!;
        public int ExpirationInSeconds { get; init; }
    }
}