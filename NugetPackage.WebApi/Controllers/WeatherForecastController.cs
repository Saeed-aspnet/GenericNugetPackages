using Microsoft.AspNetCore.Mvc;
using NugetPackage.GenericHttpClient.Interfaces;

namespace NugetPackage.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IHttpClientService _httpClientService;
        public WeatherForecastController(ILogger<WeatherForecastController> logger, IHttpClientService httpClientService)
        {
            _logger = logger;
            _httpClientService = httpClientService;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("GetData")]
        public async Task<IActionResult> GetData(CancellationToken cancellationToken)
        {
            var data = await _httpClientService.GetAsync<Root>("https://httpbin.org/get", cancellationToken);
            return Ok(data);
        }
    }
}
