﻿namespace RaspberryPi.Domain.Models
{
    public class SqlLiteKeyValue
    {
        public int Id { get; init; }
        public string Value { get; init; } = default!;
        public DateTime DateModified { get; init; } = DateTime.UtcNow;
    }
}
