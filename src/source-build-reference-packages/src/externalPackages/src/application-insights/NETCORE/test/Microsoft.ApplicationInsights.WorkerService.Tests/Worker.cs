using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.ApplicationInsights.WorkerService.Tests
{
    internal class Worker : IHostedService
    {
        private readonly ILogger _logger;
        private static HttpClient httpClient = new HttpClient();
        private Timer _timer;
        private TelemetryClient tc;

        public Worker(ILogger<Worker> logger, TelemetryClient tc)
        {
            _logger = logger;
            this.tc = tc;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("information level log - starting");
            _logger.LogWarning("warning level log - starting");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(1));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            using (tc.StartOperation<RequestTelemetry>("myoperation"))
            {
                _logger.LogInformation("information level log - running");
                _logger.LogWarning("warning level log - calling bing");
               var res =  httpClient.GetAsync("http://bing.com").Result.StatusCode;
                _logger.LogWarning("warning level log - calling bing completed with status:" + res);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}