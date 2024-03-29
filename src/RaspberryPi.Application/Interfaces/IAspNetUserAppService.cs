﻿using FluentValidation.Results;
using RaspberryPi.Application.Models.ViewModels;
using RaspberryPi.Domain.Models.Entity;

namespace RaspberryPi.Application.Interfaces
{
    public interface IAspNetUserAppService
    {
        AspNetUser? Get(Guid id);
        IEnumerable<AspNetUser> List();
        ValidationResult Register(RegisterAspNetUserViewModel viewModel);
    }
}