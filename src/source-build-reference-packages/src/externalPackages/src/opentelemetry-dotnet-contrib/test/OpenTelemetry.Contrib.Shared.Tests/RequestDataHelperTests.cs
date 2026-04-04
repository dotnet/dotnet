// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Xunit;

namespace OpenTelemetry.Internal.Tests;

public class RequestDataHelperTests
{
    public static IEnumerable<object[]> MappingVersionProtocolToVersionData =>
        [
            [new Version(1, 0), "1.0"],
            [new Version(1, 1), "1.1"],
            [new Version(2, 0), "2"],
            [new Version(3, 0), "3"],
            [new Version(7, 6, 5), "7.6.5"],
        ];

    [Theory]
    [InlineData("GET", "GET")]
    [InlineData("POST", "POST")]
    [InlineData("PUT", "PUT")]
    [InlineData("DELETE", "DELETE")]
    [InlineData("HEAD", "HEAD")]
    [InlineData("OPTIONS", "OPTIONS")]
    [InlineData("TRACE", "TRACE")]
    [InlineData("PATCH", "PATCH")]
    [InlineData("CONNECT", "CONNECT")]
    [InlineData("get", "GET")]
    [InlineData("invalid", "_OTHER")]
    public void MethodMappingWorksForKnownMethods(string method, string expected)
    {
        var requestHelper = new RequestDataHelper(configureByHttpKnownMethodsEnvironmentalVariable: true);
        var actual = requestHelper.GetNormalizedHttpMethod(method);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("GET", "GET")]
    [InlineData("POST", "POST")]
    [InlineData("PUT", "_OTHER")]
    [InlineData("DELETE", "_OTHER")]
    [InlineData("HEAD", "_OTHER")]
    [InlineData("OPTIONS", "_OTHER")]
    [InlineData("TRACE", "_OTHER")]
    [InlineData("PATCH", "_OTHER")]
    [InlineData("CONNECT", "_OTHER")]
    [InlineData("get", "GET")]
    [InlineData("post", "POST")]
    [InlineData("invalid", "_OTHER")]
    public void MethodMappingWorksForEnvironmentVariables(string method, string expected)
    {
        using (EnvironmentVariableScope.Create("OTEL_INSTRUMENTATION_HTTP_KNOWN_METHODS", "GET,POST"))
        {
            var requestHelper = new RequestDataHelper(configureByHttpKnownMethodsEnvironmentalVariable: true);
            var actual = requestHelper.GetNormalizedHttpMethod(method);
            Assert.Equal(expected, actual);
        }
    }

    [Fact]
    public void MethodMappingWorksIfEnvironmentalVariableConfigurationIsDisabled()
    {
        using (EnvironmentVariableScope.Create("OTEL_INSTRUMENTATION_HTTP_KNOWN_METHODS", "GET,POST"))
        {
            var requestHelper = new RequestDataHelper(configureByHttpKnownMethodsEnvironmentalVariable: false);
            var actual = requestHelper.GetNormalizedHttpMethod("CONNECT");
            Assert.Equal("CONNECT", actual);
        }
    }

    [Theory]
    [InlineData("HTTP/1.1", "1.1")]
    [InlineData("HTTP/2", "2")]
    [InlineData("HTTP/3", "3")]
    [InlineData("Unknown", "Unknown")]
    public void MappingProtocolToVersion(string protocolVersion, string expected)
    {
        var actual = RequestDataHelper.GetHttpProtocolVersion(protocolVersion);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(MappingVersionProtocolToVersionData))]
    public void MappingVersionProtocolToVersion(Version protocolVersion, string expected)
    {
        var actual = RequestDataHelper.GetHttpProtocolVersion(protocolVersion);
        Assert.Equal(expected, actual);
    }
}
