using Microsoft.AspNetCore.Mvc;
using RaspberryPi.API.Models.Data;
using RaspberryPi.API.Repositories;

namespace RaspberryPi.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _repository;

        public CommentController(ICommentRepository repository)
        {
                _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet]
        public async Task<Comment?> Get(Guid id)
        {
            return await _repository.GetAsync(id);
        }

        [HttpGet("list")]
        public async Task<IEnumerable<Comment>> List()
        {
            return await _repository.ListAsync();
        }

        [HttpPost]
        public async Task<bool> Post([FromBody] Comment comment)
        {
            return await _repository.CreateAsync(comment);
        }
    }
}
