using KyecloackNet6.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KyecloackNet6.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
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

        [HttpGet(Name = "GetWeatherForecast")]
        public IActionResult Get()
        {
            var currentUser = HttpContext.User;
            Securite securite = new Securite();
            // Récupérer les rôles de la revendication "realm_access"
            var realmAccessRoles = currentUser.FindFirst("realm_access")?.Value;

            // Si la revendication est présente, analyser-la pour extraire les rôles
            var roles = realmAccessRoles != null ? securite.GetRolesFromRealmAccess(realmAccessRoles) : Enumerable.Empty<string>();

            // Vérifier si le rôle "Admin" est présent dans les rôles
            var isAdmin = roles.Contains("Admin");


            var rng = new Random();
            var forecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            }).ToArray();
            if (isAdmin)
            {
                return Ok(forecasts);
            }
            else
            {
                return Unauthorized();
            }

        }
    }
}