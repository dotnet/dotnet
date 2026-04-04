// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Fabric;
using System.Globalization;
using System.Runtime.Serialization;
using Microsoft.ServiceFabric.Services.Remoting.V2;

namespace OpenTelemetry.Instrumentation.ServiceFabricRemoting.Tests;

internal class ServiceRemotingRequestMessageHeaderMock : IServiceRemotingRequestMessageHeader
{
    [DataMember(Name = "Headers", IsRequired = true, Order = 2)]
    private readonly Dictionary<string, byte[]> headers = new Dictionary<string, byte[]>();

    public ServiceRemotingRequestMessageHeaderMock()
    {
        this.InvocationId = null;
    }

    /// <summary>
    /// Gets or sets the methodId of the remote method.
    /// </summary>
    [DataMember(Name = "MethodId", IsRequired = true, Order = 0)]
    public int MethodId { get; set; }

    /// <summary>
    /// Gets or sets the interface id of the remote interface.
    /// </summary>
    [DataMember(Name = "InterfaceId", IsRequired = true, Order = 1)]
    public int InterfaceId { get; set; }

    /// <summary>
    /// Gets or sets identifier for the remote method invocation.
    /// </summary>
    [DataMember(Name = "InvocationId", IsRequired = false, Order = 3, EmitDefaultValue = false)]
    public string? InvocationId { get; set; }

    /// <summary>
    /// Gets or sets the method name of the remote method.
    /// </summary>
    [DataMember(Name = "MethodName", IsRequired = false, Order = 4)]
    public string? MethodName { get; set; }

    /// <summary>
    /// Gets or sets the request id.
    /// </summary>
    [DataMember(Name = "RequestId", IsRequired = false, Order = 5)]
    public Guid RequestId { get; set; }

    public void AddHeader(string headerName, byte[] headerValue)
    {
        if (this.headers.ContainsKey(headerName))
        {
            throw new FabricElementAlreadyExistsException(string.Format((IFormatProvider)(object)CultureInfo.CurrentCulture, "ErrorHeaderAlreadyExists"));
        }

        this.headers[headerName] = headerValue;
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
