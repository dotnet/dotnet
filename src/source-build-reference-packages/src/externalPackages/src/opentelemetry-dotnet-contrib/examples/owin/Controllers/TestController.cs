// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Web.Http;

namespace Examples.Owin.Controllers;

public class TestController : ApiController
{
    // GET api/test/{id}
    public string Get(string? id = null)
    {
        return $"id:{id}";
    }
}
