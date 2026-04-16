// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using Microsoft.ServiceFabric.Services.Remoting.V2;

namespace OpenTelemetry.Instrumentation.ServiceFabricRemoting;

/// <summary>
/// Options for ServiceFabric Remoting instrumentation.
/// </summary>
public class ServiceFabricRemotingInstrumentationOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceFabricRemotingInstrumentationOptions"/> class.
    /// </summary>
    public ServiceFabricRemotingInstrumentationOptions()
    {
    }

    /// <summary>
    /// Gets or sets a Filter function that determines whether or not to collect telemetry about requests on a per request basis.
    /// The Filter gets the <see cref="IServiceRemotingRequestMessage"/>, and should return a boolean.
    /// If Filter returns true, the request is collected.
    /// If Filter returns false or throw exception, the request is filtered out.
    /// </summary>
    public Func<IServiceRemotingRequestMessage, bool>? Filter { get; set; }

    /// <summary>
    /// Gets or sets an action to enrich the <see cref="Activity"/> created by the client instrumentation, from the request.
    /// </summary>
    public Action<Activity, IServiceRemotingRequestMessage>? EnrichAtClientFromRequest { get; set; }

    /// <summary>
    /// Gets or sets an action to enrich the <see cref="Activity"/> created by the client instrumentation, from the response.
    /// </summary>
    public Action<Activity, IServiceRemotingResponseMessage?, Exception?>? EnrichAtClientFromResponse { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the exception will be recorded at the client as <see cref="ActivityEvent"/> or not.
    /// </summary>
    /// <remarks>
    /// https://github.com/open-telemetry/opentelemetry-specification/blob/main/specification/trace/exceptions.md.
    /// </remarks>
    public bool AddExceptionAtClient { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the exception will be recorded at the server as <see cref="ActivityEvent"/> or not.
    /// </summary>
    /// <remarks>
    /// https://github.com/open-telemetry/opentelemetry-specification/blob/main/specification/trace/exceptions.md.
    /// </remarks>
    public bool AddExceptionAtServer { get; set; }
}
