using _1.RoadToSenior.Api.Filter;
using _1.RoadToSenior.Api.Models.Auth;
using _1.RoadToSenior.Api.Models.Common;
using _1.RoadToSenior.Api.Models.WeatherForecast;
using Microsoft.AspNetCore.Mvc;

namespace _1.RoadToSenior.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger) 
        {
            _logger = logger;
        }

        /// <summary>
        /// test api with role admin
        /// </summary>
        /// <returns></returns>
        [Authorize(Role.Admin)]
        [HttpGet("GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        /// <summary>
        /// test api with policy adminedit
        /// </summary>
        /// <returns></returns>
        [Policy(Policy.AdminEdit)]
        [HttpGet("GetWeatherForecast2")]
        public IEnumerable<WeatherForecast> Get2()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        /// <summary>
        /// test AllowAnonymous api
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("GetWeatherForecast3")]
        public IEnumerable<WeatherForecast> Get3()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
