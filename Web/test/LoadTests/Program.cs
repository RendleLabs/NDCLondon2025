using NBomber.Contracts;
using NBomber.CSharp;
using NBomber.Http.CSharp;

var httpClient = new HttpClient();

var minimalScenario = CreateWeatherForecastScenario(5001, httpClient, "minimal_api");
var controllerScenario = CreateWeatherForecastScenario(6001, httpClient, "controller");

NBomberRunner.RegisterScenarios(minimalScenario) //, controllerScenario)
    .Run();

static ScenarioProps CreateWeatherForecastScenario(int port, HttpClient httpClient, string name)
{
    var scenarioProps = Scenario.Create(name, async context =>
        {
            var request = Http.CreateRequest("GET", $"https://localhost:{port}/weatherforecast")
                .WithHeader("Accept", "application/json");

            var response = await Http.Send(httpClient, request);

            return response.IsError
                ? Response.Fail()
                : Response.Ok(statusCode: response.StatusCode, sizeBytes: response.SizeBytes);
        })
        .WithLoadSimulations(
            Simulation.Inject(rate: 1000,
                interval: TimeSpan.FromSeconds(1),
                during: TimeSpan.FromSeconds(60))
        );
    return scenarioProps;
}