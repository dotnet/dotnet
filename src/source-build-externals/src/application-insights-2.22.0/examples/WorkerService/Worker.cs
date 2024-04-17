using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private TelemetryClient tc;
        private static HttpClient httpClient = new HttpClient();

        public Worker(ILogger<Worker> logger, TelemetryClient tc)
        {
            _logger = logger;
            this.tc = tc;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // By default only Warning of above is captured.
                // However the following Info level will be captured by ApplicationInsights,
                // as appsettings.json configured Information level for the category 'WorkerServiceSampleWithApplicationInsights.Worker'
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                using (tc.StartOperation<RequestTelemetry>("workeroperation"))
                {
                    var res = httpClient.GetAsync("https://bing.com").Result.StatusCode;
                    _logger.LogInformation("bing http call completed with status:" + res);
                }

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
