namespace WebApp.AspNetCore.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.ApplicationInsights;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    using WebApp.AspNetCore.Models;

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TelemetryClient _telemetryClient;
        private readonly TelemetryConfiguration _telemetryConfiguration;

        public HomeController(ILogger<HomeController> logger, TelemetryClient telemetryClient, TelemetryConfiguration telemetryConfiguration)
        {
            this._logger = logger;
            this._telemetryClient = telemetryClient;

            // In a real app, you wouldn't need the TelemetryConfiguration here.
            // This is included in this sample because it allows you to debug and verify that the configuration at runtime matches the expected configuration.
            this._telemetryConfiguration = telemetryConfiguration;
        }

        public IActionResult Index()
        {
            this._telemetryClient.TrackEvent(eventName: "Hello World!");

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
