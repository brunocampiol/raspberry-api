using Microsoft.AspNetCore.Mvc;
using RaspberryPi.Domain.Interfaces.Services;
using RaspberryPi.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace RaspberryPi.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class MusicController : ControllerBase
    {
        private readonly IMusicService _musicService;

        public MusicController(IMusicService musicService)
        {
            _musicService = musicService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult PlayMusic([FromBody][Required] Music music)
        {
            _musicService.PlayMusic(music);
            return NoContent();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult PlayImperialMarch()
        {
            _musicService.PlayImperialMarch();
            return NoContent();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult PlaySuperMarioWorld()
        {
            _musicService.PlaySuperMarioWorld();
            return NoContent();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult PlayPinkPanther()
        {
            _musicService.PlayPinkPanther();
            return NoContent();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult PlayNokiaRingtone()
        {
            _musicService.PlayNokiaRingtone();
            return NoContent();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult PlayPiratesOfTheCaribbean()
        {
            _musicService.PlayPiratesOfTheCaribbean();
            return NoContent();
        }
    }
}