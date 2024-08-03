using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RaspberryPi.API.Models.ViewModels;
using RaspberryPi.Application.Interfaces;
using RaspberryPi.Domain.Models.Entity;

namespace RaspberryPi.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class FactController : ControllerBase
    {
        private readonly IFactAppService _service;
        private readonly IMapper _mapper;

        public FactController(IFactAppService service,  IMapper mapper)
        {
            _service = service;   
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<Fact>> GetAllDatabaseFacts()
        {
            var result = await _service.GetAllDatabaseFactsAsync();
            return result;
        }

        [HttpGet]
        public async Task<Fact?> GetFirstOrDefaultFactFromDatabase()
        {
            var result = await _service.GetFirstOrDefaultFactFromDatabaseAsync();
            return result;
        }

        [HttpGet]
        public async Task<long> CountAllDatabaseFacts()
        {
            var result = await _service.CountAllDatabaseFacts();
            return result;
        }

        [HttpGet]
        // TODO: update this to a better name
        public async Task<FactViewModel> SaveFactAndComputeHash()
        {
            var result = await _service.SaveFactAndComputeHashAsync();
            var viewModel = _mapper.Map<FactViewModel>(result);
            return viewModel;
        }
    }
}