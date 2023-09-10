using Microsoft.AspNetCore.Mvc;
using RaspberryPi.API.Contracts.Data;
using RaspberryPi.API.Database;
using RaspberryPi.API.Repositories;

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
            _databaseInitializer = databaseInitializer ?? throw new ArgumentNullException(nameof(_databaseInitializer));
        }

        [HttpGet]
        public async Task<SqlLiteKeyValue?> Get(int id)
        {
            return await _repository.GetAsync(id);
        }

        [HttpGet("list")]
        public async Task<IEnumerable<SqlLiteKeyValue>> List()
        {
            return await _repository.ListAsync();
        }

        [HttpPost]
        public async Task<bool> Post([FromBody] SqlLiteKeyValue keyValue)
        {
            return await _repository.CreateAsync(keyValue);
        }

        [HttpPost("initialize")]
        public async Task Initialize()
        {
            await _databaseInitializer.InitializeAsync();
        }
    }
}