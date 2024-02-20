using Microsoft.AspNetCore.Mvc;
using RaspberryPi.Domain.Interfaces.Repositories;
using RaspberryPi.Domain.Models;
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
        private readonly DatabaseInitializer _databaseInitializer;
        public DapperRepositoryController(IDapperRepository sqlLiteKeyValueRepository, DatabaseInitializer databaseInitializer)
        {
            _repository = sqlLiteKeyValueRepository ?? throw new ArgumentNullException(nameof(sqlLiteKeyValueRepository));
            _databaseInitializer = databaseInitializer ?? throw new ArgumentNullException(nameof(databaseInitializer));
        }

        [HttpGet]
        public SqlLiteKeyValue? Get(int id)
        {
            return _repository.Get(id);
        }

        [HttpGet("list")]
        public IEnumerable<SqlLiteKeyValue> List()
        {
            return _repository.List();
        }

        [HttpPost]
        public bool Post([FromBody] SqlLiteKeyValue keyValue)
        {
            return _repository.Create(keyValue);
        }

        [HttpPost("initialize")]
        public void Initialize()
        {
            _databaseInitializer.Initialize();
        }

        [HttpPost("truncate")]
        public void Truncate()
        {
            _repository.Truncate();
        }
    }
}