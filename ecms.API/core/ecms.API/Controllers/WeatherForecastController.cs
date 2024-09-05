using Asp.Versioning;
using ecms.API.Controllers.Base;
using ecms.API.Extensions;
using ecms.API.Infrastructure;
using ecms.Application.Abstractions.Auth;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace ecms.API.Controllers;

[ApiVersion(EcmsApiVersion.Version1)]
public class WeatherForecastController : BaseController
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly ICurrentUserService currentUser;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, ICurrentUserService currentUser)
    {
        _logger = logger;
        this.currentUser = currentUser;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IActionResult Get()
    {
        var user = currentUser.UserId;
        var forecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();

        return Result.Success(forecasts).Match(
            onSuccess: forecasts => Ok(forecasts),
            onFailure: CustomResults.Problem
        );
    }
}