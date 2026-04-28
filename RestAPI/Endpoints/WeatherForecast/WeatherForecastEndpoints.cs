namespace Presentation.API.Endpoints.WeatherForecast;

public static class WeatherForecastEndpoints
{
    public static void AddWeatherForecastEndpoints(this WebApplication app)
    {
        app.MapGet("api/weatherforecast", async (IHttpClientFactory httpClientFactory) =>
        {
            var client = httpClientFactory.CreateClient("webapplication2-api");
            var result = await client.GetAsync("api/weatherforecast");
            result.EnsureSuccessStatusCode();
            var content = await result.Content.ReadAsStringAsync();
            return Results.Content(content, "application/json");
        }).WithTags("WeatherForecast");
    }
}
