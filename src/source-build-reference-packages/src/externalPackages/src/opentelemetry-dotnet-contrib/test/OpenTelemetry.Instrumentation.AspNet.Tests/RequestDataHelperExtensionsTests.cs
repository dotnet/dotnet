// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Collections.Specialized;
using System.Web;
using OpenTelemetry.Instrumentation.AspNet.Implementation;
using Xunit;

namespace OpenTelemetry.Instrumentation.AspNet.Tests;

public class RequestDataHelperExtensionsTests
{
    [InlineData("1.1", "HTTP/1.1")]
    [InlineData("2", "2")]
    [InlineData("3", "3")]
    [InlineData("NotKnownVersion", "NotKnownVersion")]
    [Theory]
    public void GetHttpProtocolVersionTest(string expectedHttpProtocolVersion, string httpProtocolServerVariable)
    {
        var httpProtocolVersion = RequestDataHelperExtensions.GetHttpProtocolVersion(new HttpRequestMock(httpProtocolServerVariable));

        Assert.Equal(expectedHttpProtocolVersion, httpProtocolVersion);
    }

    private class HttpRequestMock : HttpRequestBase
    {
        public HttpRequestMock(string httpProtocolServerVariable)
        {
            this.ServerVariables = new NameValueCollection { { "SERVER_PROTOCOL", httpProtocolServerVariable } };
        }

        public override NameValueCollection ServerVariables { get; }
    }
}
