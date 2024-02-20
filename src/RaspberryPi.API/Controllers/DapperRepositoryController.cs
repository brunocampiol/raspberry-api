using Microsoft.AspNetCore.Mvc;
using RaspberryPi.Domain.Interfaces.Repositories;
using RaspberryPi.Domain.Models.Entity;
using RaspberryPi.Infrastructure.Data.Dapper;

namespace RaspberryPi.API.Controllers
{
    /// <summary>
    /// Dapper related methods
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class DapperRepositoryController : ControllerBase
    {
        private readonly IDapperRepository _repository;

        public DapperRepositoryController(IDapperRepository sqlLiteKeyValueRepository)
        {
            _repository = sqlLiteKeyValueRepository ?? throw new ArgumentNullException(nameof(sqlLiteKeyValueRepository));
        }

        [HttpGet]
        public AspNetUser? Get(Guid id)
        {
            return _repository.Get(id);
        }

        [HttpGet("list")]
        public IEnumerable<AspNetUser> List()
        {
            return _repository.List();
        }

        [HttpPost]
        public bool Post([FromBody] AspNetUser keyValue)
        {
            return _repository.Create(keyValue);
        }

        [HttpPost("truncate")]
        public void Truncate()
        {
            _repository.Truncate();
        }
    }
}