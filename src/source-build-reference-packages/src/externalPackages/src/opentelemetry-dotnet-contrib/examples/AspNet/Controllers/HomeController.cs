// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Web.Mvc;

namespace Examples.AspNet.Controllers;

public class HomeController : Controller
{
    // For testing traditional routing. Ex: https://localhost:XXXX/
    [HttpGet]
    public ActionResult Index()
    {
        return this.View();
    }

    [HttpGet]
    [Route("about_attr_route/{customerId}")] // For testing attribute routing. Ex: https://localhost:XXXX/about_attr_route
    public ActionResult About(int? customerId)
    {
        this.ViewBag.Message = $"Your application description page for customer {customerId}.";

        return this.View();
    }
}
