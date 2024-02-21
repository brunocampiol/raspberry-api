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
    public class DapperController : ControllerBase
    {
        private readonly IDapperRepository _repository;

        public DapperController(IDapperRepository sqlLiteKeyValueRepository)
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

        [HttpDelete("Delete")]
        public bool Delete(Guid id)
        {
            return _repository.Delete(id);
        }

        [HttpDelete("DeleteAll")]
        public int DeleteAll()
        {
            return _repository.DeleteAll();
        }
    }
}