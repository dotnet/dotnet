using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using FunctionalTests.MVC.Tests.Models;

namespace FunctionalTests.MVC.Tests.Controllers
{
    public class HomeController : Controller
    {
        private TelemetryClient telemetryClient;

        public HomeController(TelemetryClient telemetryClient)
        {
            this.telemetryClient = telemetryClient;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About(int index)
        {
            ViewBag.Message = "Your application description page # " + index;

            return View();
        }

        public IActionResult Contact()
        {
            this.telemetryClient.TrackEvent("GetContact");
            this.telemetryClient.TrackMetric("ContactFile", 1);
            this.telemetryClient.TrackTrace("Fetched contact details.", SeverityLevel.Information);
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Exception()
        {
            throw new InvalidOperationException("Do not call the method called Exception");
        }

        public IActionResult Dependency()
        {
            this.telemetryClient.TrackDependency(
                dependencyTypeName: "test",
                dependencyName: "MyDependency", 
                data: "MyCommand", 
                startTime: DateTimeOffset.Now, 
                duration: TimeSpan.FromMilliseconds(1), 
                success: true);
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
