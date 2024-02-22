using Microsoft.AspNetCore.Mvc;
using RaspberryPi.Application.Interfaces;
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

        /// <summary>
        /// Plays a music based on the melody and duration
        /// </summary>
        /// <param name="music"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult PlayMusic([FromBody][Required] Music music)
        {
            _appService.PlayMusic(music);
            return NoContent();
        }

        /// <summary>
        /// Plays Nokia ring tone (1994)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult PlayNokiaRingtone()
        {
            _appService.PlayNokiaRingTone();
            return NoContent();
        }
    }
}