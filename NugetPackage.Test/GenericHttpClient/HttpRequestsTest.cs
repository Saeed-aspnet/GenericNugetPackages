using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using NugetPackage.GenericHttpClient.Interfaces;
using NugetPackage.WebApi;

namespace NugetPackage.Test.GenericHttpClient;
public sealed class HttpRequestsTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly IHttpClientService _httpClientService;
    private const string _baseUrl = "http://localhost:5057";
    private readonly WebApplicationFactory<Program> _factory;
    public HttpRequestsTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _httpClientService = _factory.Services.GetRequiredService<IHttpClientService>();
    }

    [Fact]
    public async Task GetWeatherForcast_Should()
    {
        // Arrange


        // Act

        var response = await _httpClientService.GetAsync<List<WeatherForecast>>(string.Concat(_baseUrl, "/weatherforecast/"), CancellationToken.None);

        // Assert

        Assert.NotNull(response);

    }
}