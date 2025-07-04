﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaspberryPi.API.Models.ViewModels;
using RaspberryPi.Application.Interfaces;
using RaspberryPi.Application.Models.Dtos;
using RaspberryPi.Domain.Models.Entity;

namespace RaspberryPi.API.Controllers
{
    /// <summary>
    /// Email related methods
    /// </summary>
    [ApiController]
    [Route("[controller]/[action]")]
    public class EmailController : ControllerBase
    {
        private readonly IEmailAppService _service;
        private readonly IMapper _mapper;

        public EmailController(IEmailAppService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        /// <summary>
        /// Sends an email
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "root")]
        public async Task<EmailOutbox> Send(EmailViewModel viewModel)
        {
            var dto = _mapper.Map<EmailDto>(viewModel);
            return await _service.SendEmailAsync(dto);
        }

        /// <summary>
        /// Gets all sent emails from database
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "root")]
        public async Task<IEnumerable<EmailOutbox>> GetAll()
        {
            return await _service.GetAllAsync();
        }

        /// <summary>
        /// Gets the last sent email from database
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<EmailOutbox?> GetLastSentEmail()
        {
            return await _service.GetLastSentEmailAsync();
        }

        /// <summary>
        /// Deletes all emails from database
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Authorize(Roles = "root")]
        public async Task DeleteAll()
        {
            await _service.DeleteAllAsync();
        }
    }
}