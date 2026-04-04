// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace Examples.GrpcCore.AspNetCore;

public class WeatherForecast
{
    public WeatherForecast(DateTime date, int temperatureC, string summary)
    {
        this.Date = date;
        this.TemperatureC = temperatureC;
        this.Summary = summary;
    }

    public DateTime Date { get; }

    public int TemperatureC { get; }

    public string Summary { get; }
}
