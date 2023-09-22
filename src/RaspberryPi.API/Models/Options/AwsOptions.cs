﻿namespace RaspberryPi.API.Models.Options
{
    public sealed class AwsOptions
    {
        public const string Aws = "AWS";

        public string AccessKeyId { get; init; } = default!;
        public string SecretAccessKey { get; init; } = default!;
    }
}