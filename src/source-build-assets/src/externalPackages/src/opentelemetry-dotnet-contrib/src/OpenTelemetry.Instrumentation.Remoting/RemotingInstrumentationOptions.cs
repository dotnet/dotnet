// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using System.Runtime.Remoting.Messaging;
using OpenTelemetry.Context.Propagation;

namespace OpenTelemetry.Instrumentation.Remoting;

/// <summary>
/// Options for .NET Remoting instrumentation.
/// </summary>
public class RemotingInstrumentationOptions
{
    /// <summary>
    /// Gets or sets an action to enrich an <see cref="Activity"/> from a remoting method message.
    /// </summary>
    /// <remarks>
    /// This callback is invoked after the activity is created and before the remoting call is processed.
    /// </remarks>
    public Action<Activity, IMethodMessage>? EnrichWithMethodMessage { get; set; }

    /// <summary>
    /// Gets or sets an action to enrich an <see cref="Activity"/> from a remoting method return message.
    /// </summary>
    /// <remarks>
    /// This callback is invoked before the activity is stopped when a remoting method return message is available.
    /// </remarks>
    public Action<Activity, IMethodReturnMessage>? EnrichWithMethodReturnMessage { get; set; }

    /// <summary>
    /// Gets or sets a <see cref="TextMapPropagator"/> for context propagation. Default value: <see cref="CompositeTextMapPropagator"/> with <see cref="TraceContextPropagator"/> and <see cref="BaggagePropagator"/>.
    /// </summary>
    public TextMapPropagator Propagator { get; set; } = new CompositeTextMapPropagator([
        new TraceContextPropagator(),
        new BaggagePropagator()
    ]);

    /// <summary>
    /// Gets or sets a filter function that determines whether or not to collect telemetry about a Remoting message.
    /// </summary>
    /// <remarks>
    /// The filter function receives an <see cref="IMessage"/> and should return a boolean.
    /// <list type="bullet">
    /// <item>If the filter returns <see langword="true"/>, the message is instrumented.</item>
    /// <item>If the filter returns <see langword="false"/> or throws an exception, no instrumentation is performed.</item>
    /// </list>
    /// </remarks>
    public Func<IMessage, bool>? Filter { get; set; }
}
