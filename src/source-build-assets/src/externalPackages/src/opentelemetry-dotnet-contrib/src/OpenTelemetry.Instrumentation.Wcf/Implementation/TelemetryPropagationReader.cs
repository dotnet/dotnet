// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.ServiceModel.Channels;
using OpenTelemetry.Internal;

namespace OpenTelemetry.Instrumentation.Wcf.Implementation;

/// <summary>
/// Pre-defined PropagationReader callbacks.
/// </summary>
internal static class TelemetryPropagationReader
{
    private static readonly Func<Message, string, IEnumerable<string>?> DefaultReader = Compose(HttpRequestHeaders, SoapMessageHeaders);

    /// <summary>
    /// Reads the values from the SOAP message headers. If the message is not a SOAP message it returns null.
    /// </summary>
    /// <param name="request">The incoming <see cref="Message"/> to read trace context from.</param>
    /// <param name="name">The header name being requested.</param>
    /// <returns>An enumerable of all the values for the requested header, or null if the requested header was not present on the request.</returns>
    public static IEnumerable<string>? SoapMessageHeaders(Message request, string name)
    {
        Guard.ThrowIfNull(request);
        Guard.ThrowIfNull(name);

        if (request.Version == MessageVersion.None)
        {
            return null;
        }

        var header = TelemetryMessageHeader.FindHeader(name, request.Headers);
        return header == null ? null : new[] { header.Value };
    }

    /// <summary>
    /// Reads the values from the incoming HTTP request headers. If the message was not made via an HTTP request it returns null.
    /// </summary>
    /// <param name="request">The incoming <see cref="Message"/> to read trace context from.</param>
    /// <param name="name">The header name being requested.</param>
    /// <returns>An enumerable of all the values for the requested header, or null if the requested header was not present on the request.</returns>
    public static IEnumerable<string>? HttpRequestHeaders(Message request, string name)
    {
        Guard.ThrowIfNull(request);
        Guard.ThrowIfNull(name);

        if (!HttpRequestMessagePropertyWrapper.IsHttpFunctionalityEnabled || !request.Properties.TryGetValue(HttpRequestMessagePropertyWrapper.Name, out var prop))
        {
            return null;
        }

        var value = HttpRequestMessagePropertyWrapper.GetHeaders(prop)[name];
        return value == null ? null : new[] { value };
    }

    /// <summary>
    /// Reads the values from the incoming HTTP request headers and falls back to SOAP message headers if not found on the HTTP request.
    /// </summary>
    /// <param name="request">The incoming <see cref="Message"/> to read trace context from.</param>
    /// <param name="name">The header name being requested.</param>
    /// <returns>An enumerable of all the values for the requested header, or null if the requested header was not present on the request.</returns>
    public static IEnumerable<string>? Default(Message request, string name)
    {
        return DefaultReader(request, name);
    }

    /// <summary>
    /// Compose multiple PropagationReader callbacks into a single callback. The callbacks
    /// are called sequentially and the first one to return a non-null value wins.
    /// </summary>
    /// <param name="callbacks">The callbacks to compose into a single callback.</param>
    /// <returns>The composed callback.</returns>
    public static Func<Message, string, IEnumerable<string>?> Compose(params Func<Message, string, IEnumerable<string>?>[] callbacks)
    {
        Guard.ThrowIfNull(callbacks);
        Array.ForEach(callbacks, cb => Guard.ThrowIfNull(cb));

        return (Message request, string name) =>
        {
            foreach (var reader in callbacks)
            {
                var values = reader(request, name);
                if (values != null)
                {
                    return values;
                }
            }

            return null;
        };
    }
}
