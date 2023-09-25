using Microsoft.AspNetCore.Mvc;
using RaspberryPi.Domain.Interfaces;
using RaspberryPi.Domain.Models;

namespace RaspberryPi.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AnonymousCommentController : ControllerBase
    {
        private readonly IAnonymousCommentRepository _repository;

        public AnonymousCommentController(IAnonymousCommentRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public AnonymousComment? Get(Guid id)
        {
            return _repository.GetById(id);
        }

        [HttpGet("list")]
        public IEnumerable<AnonymousComment> List()
        {
            return _repository.GetList();
        }

        [HttpPost]
        public void Post([FromBody] AnonymousComment viewModel)
        {
            _repository.Add(viewModel);
            _repository.UnitOfWork.Commit();
        }
    }
}