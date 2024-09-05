using FluentAssertions;
using FunctionalTests.Abstractions;

namespace FunctionalTests.WeatherForecast;

public class WeatherForecastTests : BaseFunctionalTest
{
    public WeatherForecastTests(FunctionalTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Get_ShouldReturnRandomForecasts()
    {
        //Arrange

        //Act
        var response = await AuthorizedHttpClient.GetFromJsonAsync<IEnumerable<ecms.API.WeatherForecast>>("api/v1/WeatherForecast");

        //Assert
        response.Should().BeOfType<List<ecms.API.WeatherForecast>>();
    }
}