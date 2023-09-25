﻿using RaspberryPi.Domain.Core;

namespace RaspberryPi.Domain.Models
{
    public abstract class BaseEntity : IAggregateRoot
    {
        public Guid Id { get; init; } = Guid.NewGuid();
    }
}