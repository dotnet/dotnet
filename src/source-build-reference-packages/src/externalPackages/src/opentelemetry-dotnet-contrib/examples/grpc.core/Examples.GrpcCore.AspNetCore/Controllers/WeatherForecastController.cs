// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Microsoft.AspNetCore.Mvc;

namespace Examples.GrpcCore.AspNetCore.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    private readonly Echo.EchoClient echoClient;

    public WeatherForecastController(Echo.EchoClient echoClient)
    {
        this.echoClient = echoClient;
    }

    [HttpGet]
    public async Task<IEnumerable<WeatherForecast>> Get()
    {
        var echoCall = this.echoClient.EchoAsync(new EchoRequest { Message = "Hello" });
        var echoResponse = await echoCall.ResponseAsync.ConfigureAwait(false);

        var rng = new Random();
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast(
                date: DateTime.Now.AddDays(index),
                temperatureC: rng.Next(-20, 55),
                summary: Summaries[rng.Next(Summaries.Length)]))
            .ToArray();
    }
}
