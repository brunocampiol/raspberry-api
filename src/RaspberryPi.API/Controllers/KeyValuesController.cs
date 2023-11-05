using Microsoft.AspNetCore.Mvc;
using RaspberryPi.Domain.Interfaces.Repositories;
using RaspberryPi.Domain.Models;
using RaspberryPi.Infrastructure.Data;

namespace RaspberryPi.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KeyValuesController : ControllerBase
    {
        private readonly ISqlLiteKeyValueRepository _repository;
        private readonly DatabaseInitializer _databaseInitializer;
        public KeyValuesController(ISqlLiteKeyValueRepository sqlLiteKeyValueRepository, DatabaseInitializer databaseInitializer)
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
        public async Task<bool> Post([FromBody] SqlLiteKeyValue keyValue)
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