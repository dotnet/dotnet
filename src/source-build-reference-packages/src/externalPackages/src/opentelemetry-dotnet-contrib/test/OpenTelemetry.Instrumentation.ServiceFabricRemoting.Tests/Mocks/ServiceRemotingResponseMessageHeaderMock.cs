// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Fabric;
using System.Globalization;
using Microsoft.ServiceFabric.Services.Remoting.V2;

namespace OpenTelemetry.Instrumentation.ServiceFabricRemoting.Tests;

internal class ServiceRemotingResponseMessageHeaderMock : IServiceRemotingResponseMessageHeader
{
    private Dictionary<string, byte[]> headers;

    public ServiceRemotingResponseMessageHeaderMock()
    {
        this.headers = new Dictionary<string, byte[]>();
    }

    public void AddHeader(string headerName, byte[] headerValue)
    {
        if (this.headers.ContainsKey(headerName))
        {
            throw new FabricElementAlreadyExistsException(string.Format((IFormatProvider)(object)CultureInfo.CurrentCulture, "ErrorHeaderAlreadyExists"));
        }

        this.headers[headerName] = headerValue;
    }

    public bool CheckIfItsEmpty()
    {
        if (this.headers == null || this.headers.Count == 0)
        {
            return true;
        }

        return false;
    }

    public bool TryGetHeaderValue(string headerName, out byte[]? headerValue)
    {
        headerValue = null;
        if (this.headers == null)
        {
            return false;
        }

        return this.headers.TryGetValue(headerName, out headerValue);
    }
}
