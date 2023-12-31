﻿namespace RaspberryPi.Application.Models.Options
{
    public sealed class JwtAppOptions
    {
        public const string SectionName = "Jwt";
        public string Issuer { get; init; } = default!;
        public string Audience { get; init; } = default!;
        public string Key { get; init; } = default!;
        public int ExpirationInSeconds { get; init; }
    }
}