using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using RaspberryPi.Application.Interfaces;
using RaspberryPi.Application.Models.ViewModels;
using RaspberryPi.Domain.Models.Entity;

namespace RaspberryPi.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AspNetUserController : ControllerBase
    {
        private readonly IAspNetUserAppService _appService;

        public AspNetUserController(IAspNetUserAppService appService)
        {
           _appService = appService;
        }

        [HttpGet]
        public AspNetUser? Get(Guid id)
        {
            return _appService.Get(id);
        }

        [HttpGet("list")]
        public IEnumerable<AspNetUser> List()
        {
            return _appService.List();
        }

        [HttpPost]
        public ValidationResult Post([FromBody] RegisterAspNetUserViewModel viewModel)
        {
            return _appService.Register(viewModel);
        }
    }
}