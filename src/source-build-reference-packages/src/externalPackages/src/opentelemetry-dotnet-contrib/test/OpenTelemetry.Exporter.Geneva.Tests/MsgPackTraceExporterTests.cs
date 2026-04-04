// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpenTelemetry.Exporter.Geneva.MsgPack;
using Xunit;

namespace OpenTelemetry.Exporter.Geneva.Tests;

public class MsgPackTraceExporterTests
{
    [Fact]
    public void CacheIfPartOfHttpUrl_KeyPresent_IndexInRange_SetsValueAndReturnsTrue()
    {
        var entry = new KeyValuePair<string, object?>("url.scheme", "https");
        var arr = new object?[MsgPackTraceExporter.CS40_PART_B_HTTPURL_MAPPING.Count];
        var result = MsgPackTraceExporter.CacheIfPartOfHttpUrl(entry, arr);
        Assert.True(result);
        Assert.Equal("https", arr[0]);
    }

    [Fact]
    public void CacheIfPartOfHttpUrl_KeyPresent_IndexOutOfRange_ReturnsFalse()
    {
        var entry = new KeyValuePair<string, object?>("url.scheme", "https");
        var arr = Array.Empty<object?>(); // zero-length array
        var result = MsgPackTraceExporter.CacheIfPartOfHttpUrl(entry, arr);
        Assert.False(result);
    }

    [Fact]
    public void CacheIfPartOfHttpUrl_KeyNotPresent_ReturnsFalse()
    {
        var entry = new KeyValuePair<string, object?>("not.a.key", "value");
        var arr = new object?[MsgPackTraceExporter.CS40_PART_B_HTTPURL_MAPPING.Count];
        var result = MsgPackTraceExporter.CacheIfPartOfHttpUrl(entry, arr);
        Assert.False(result);
    }

    [Fact]
    public void CacheIfPartOfHttpUrl_NullValue_SetsNull()
    {
        var entry = new KeyValuePair<string, object?>("url.scheme", null);
        var arr = new object?[MsgPackTraceExporter.CS40_PART_B_HTTPURL_MAPPING.Count];
        var result = MsgPackTraceExporter.CacheIfPartOfHttpUrl(entry, arr);
        Assert.True(result);
        Assert.Null(arr[0]);
    }

    [Theory]
    [InlineData("", "", "", "", null)]
    [InlineData("http", "host", "", "", "http://host")]
    [InlineData("http", "host", "8080", "/x", "http://host:8080/x")]
    [InlineData("https", "server", "443", "/api", "https://server:443/api")]
    [InlineData("http", "host", "", "/x?y=1", "http://host/x?y=1")]
    public void GetHttpUrl_ReturnsExpectedUrl(string scheme, string hostOrAddress, string port, string pathAndQuery, string? expected)
    {
        var arr = new object?[MsgPackTraceExporter.CS40_PART_B_MAPPING_DICTIONARY.Count];
        arr[0] = scheme;
        arr[1] = hostOrAddress;
        arr[2] = string.IsNullOrEmpty(port) ? null : port;
        if (!string.IsNullOrEmpty(pathAndQuery) && pathAndQuery.Contains('?'))
        {
            var split = pathAndQuery.Split(['?'], 2);
            arr[3] = split[0];
            arr[4] = split.Length > 1 ? split[1] : null;
        }
        else
        {
            arr[3] = string.IsNullOrEmpty(pathAndQuery) ? null : pathAndQuery;
        }

        var url = MsgPackTraceExporter.GetHttpUrl(arr);
        Assert.Equal(expected, url);
    }

    [Fact]
    public void GetHttpUrl_UnknownMethod_ReturnsNull()
    {
        var arr = new object?[MsgPackTraceExporter.CS40_PART_B_HTTPURL_MAPPING.Count];
        var url = MsgPackTraceExporter.GetHttpUrl(arr);
        Assert.Null(url);
    }
}
