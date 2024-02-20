using Microsoft.AspNetCore.Mvc;
using RaspberryPi.Application.Interfaces;
using RaspberryPi.Domain.Interfaces.Services;
using RaspberryPi.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace RaspberryPi.API.Controllers
{
    /// <summary>
    /// Buzzer music related methods
    /// </summary>
    [ApiController]
    [Route("[controller]/[action]")]
    public class MusicController : ControllerBase
    {
        private readonly IMusicAppService _appService;

        public MusicController(IMusicAppService appService)
        {
            _appService = appService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult PlayMusic([FromBody][Required] Music music)
        {
            _appService.PlayMusic(music);
            return NoContent();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult PlayImperialMarch()
        {
            _appService.PlayImperialMarch();
            return NoContent();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult PlaySuperMarioWorld()
        {
            _appService.PlaySuperMarioWorld();
            return NoContent();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult PlayPinkPanther()
        {
            _appService.PlayPinkPanther();
            return NoContent();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult PlayNokiaRingtone()
        {
            _appService.PlayNokiaRingtone();
            return NoContent();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult PlayPiratesOfTheCaribbean()
        {
            _appService.PlayPiratesOfTheCaribbean();
            return NoContent();
        }
    }
}