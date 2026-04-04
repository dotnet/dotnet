// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.Instrumentation.Http.Tests;

public class HttpOutTestCase
{
    public HttpOutTestCase(string name, string method, string url, Dictionary<string, string>? headers, int responseCode, string spanName, bool responseExpected, bool? recordException, string spanStatus, Dictionary<string, string> spanAttributes)
    {
        this.Name = name;
        this.Method = method;
        this.Url = url;
        this.Headers = headers;
        this.ResponseCode = responseCode;
        this.SpanName = spanName;
        this.ResponseExpected = responseExpected;
        this.RecordException = recordException;
        this.SpanStatus = spanStatus;
        this.SpanAttributes = spanAttributes;
    }

    public string Name { get; set; }

    public string Method { get; set; }

#pragma warning disable CA1056
    public string Url { get; set; }
#pragma warning restore CA1056

#pragma warning disable CA2227
    public Dictionary<string, string>? Headers { get; set; }
#pragma warning restore CA2227

    public int ResponseCode { get; set; }

    public string SpanName { get; set; }

    public bool ResponseExpected { get; set; }

    public bool? RecordException { get; set; }

    public string SpanStatus { get; set; }

#pragma warning disable CA2227
    public Dictionary<string, string> SpanAttributes { get; set; }
#pragma warning restore CA2227
}
