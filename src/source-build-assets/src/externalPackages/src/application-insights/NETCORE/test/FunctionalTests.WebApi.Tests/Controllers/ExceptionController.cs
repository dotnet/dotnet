using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace FunctionalTests.WebApi.Tests.Controllers
{
    [Route("api/[controller]")]
    public class ExceptionController : Controller
    {
        // GET: api/exception
        [HttpGet]
        public IEnumerable<string> Get()
        {
            throw new InvalidOperationException();
        }
    }
}
