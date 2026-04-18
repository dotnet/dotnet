// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using OpenTelemetry.Exporter.Geneva.MsgPack;
using OpenTelemetry.Exporter.Geneva.Transports;
using OpenTelemetry.Internal;

namespace OpenTelemetry.Exporter.Geneva;

internal enum TransportProtocol
{
    Etw,
    Tcp,
    Udp,
    Unix,
    EtwTld,
    Unspecified,
}

internal sealed class ConnectionStringBuilder
{
    private readonly Dictionary<string, string> parts = new(StringComparer.Ordinal);

    public ConnectionStringBuilder([NotNull] string? connectionString)
    {
        Guard.ThrowIfNullOrWhitespace(connectionString);

        const char Semicolon = ';';
        const char EqualSign = '=';
        foreach (var token in connectionString.Split(Semicolon))
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                continue;
            }

#if NET
            var index = token.IndexOf(EqualSign, StringComparison.Ordinal);
#else
            var index = token.IndexOf(EqualSign);
#endif
            if (index == -1 || index != token.LastIndexOf(EqualSign))
            {
                continue;
            }

            var pair = token.Trim().Split(EqualSign);

            var key = pair[0].Trim();
            var value = pair[1].Trim();
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("Connection string cannot contain empty keys or values.");
            }

            this.parts[key] = value;
        }

        if (this.parts.Count == 0)
        {
            throw new ArgumentNullException(nameof(connectionString), $"{nameof(connectionString)} is invalid.");
        }
    }

    public string EtwSession
    {
        get => this.ThrowIfNotExists<string>(nameof(this.EtwSession));
        set => this.parts[nameof(this.EtwSession)] = value;
    }

    public bool PrivatePreviewEnableTraceLoggingDynamic => this.parts.TryGetValue(nameof(this.PrivatePreviewEnableTraceLoggingDynamic), out var value)
                && bool.TrueString.Equals(value, StringComparison.OrdinalIgnoreCase);

    public bool PrivatePreviewEnableOtlpProtobufEncoding => this.parts.TryGetValue(nameof(this.PrivatePreviewEnableOtlpProtobufEncoding), out var value)
                && bool.TrueString.Equals(value, StringComparison.OrdinalIgnoreCase);

    public bool PrivatePreviewEnableUserEvents => this.parts.TryGetValue(nameof(this.PrivatePreviewEnableUserEvents), out var value)
                && bool.TrueString.Equals(value, StringComparison.OrdinalIgnoreCase);

    public bool PrivatePreviewEnableAFDCorrelationIdEnrichment => this.parts.TryGetValue(nameof(this.PrivatePreviewEnableAFDCorrelationIdEnrichment), out var value)
                && bool.TrueString.Equals(value, StringComparison.OrdinalIgnoreCase);

    public int PrivatePreviewLogMessagePackStringSizeLimit
    {
        get
        {
            if (!this.parts.TryGetValue(nameof(this.PrivatePreviewLogMessagePackStringSizeLimit), out var value))
            {
                return MessagePackSerializer.DEFAULT_STRING_SIZE_LIMIT_CHAR_COUNT;
            }

            if (!int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var sizeLimit))
            {
                throw new ArgumentException(
                    $"{nameof(this.PrivatePreviewLogMessagePackStringSizeLimit)} is malformed.",
                    nameof(this.PrivatePreviewLogMessagePackStringSizeLimit));
            }

            if (sizeLimit <= 0 || sizeLimit > MsgPackLogExporter.BUFFER_SIZE)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(this.PrivatePreviewLogMessagePackStringSizeLimit),
                    $"{nameof(this.PrivatePreviewLogMessagePackStringSizeLimit)} should be greater than zero and less than or equal to {MsgPackLogExporter.BUFFER_SIZE} characters.");
            }

            return sizeLimit;
        }
    }

    public string Endpoint
    {
        get => this.ThrowIfNotExists<string>(nameof(this.Endpoint));
        set => this.parts[nameof(this.Endpoint)] = value;
    }

    public TransportProtocol Protocol
    {
        get
        {
            try
            {
                // Checking Etw first, since it's preferred for Windows and enables fail fast on Linux
                if (this.parts.ContainsKey(nameof(this.EtwSession)))
                {
                    return this.PrivatePreviewEnableTraceLoggingDynamic ? TransportProtocol.EtwTld : TransportProtocol.Etw;
                }

                if (!this.parts.ContainsKey(nameof(this.Endpoint)))
                {
                    return TransportProtocol.Unspecified;
                }

                var endpoint = new Uri(this.Endpoint);
                return Enum.TryParse(endpoint.Scheme, true, out TransportProtocol protocol)
                    ? protocol
                    : throw new ArgumentException("Endpoint scheme is invalid.");
            }
            catch (UriFormatException ex)
            {
                throw new ArgumentException($"{nameof(this.Endpoint)} value is malformed.", ex);
            }
        }
    }

    public int TimeoutMilliseconds
    {
        get
        {
            if (!this.parts.TryGetValue(nameof(this.TimeoutMilliseconds), out var value))
            {
                return UnixDomainSocketDataTransport.DefaultTimeoutMilliseconds;
            }

            try
            {
                var timeout = int.Parse(value, CultureInfo.InvariantCulture);
                return timeout <= 0
                    ? throw new ArgumentException(
                        $"{nameof(this.TimeoutMilliseconds)} should be greater than zero.",
                        nameof(this.TimeoutMilliseconds))
                    : timeout;
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(
                    $"{nameof(this.TimeoutMilliseconds)} is malformed.",
                    nameof(this.TimeoutMilliseconds),
                    ex);
            }
        }
        set => this.parts[nameof(this.TimeoutMilliseconds)] = value.ToString(CultureInfo.InvariantCulture);
    }

    public string Host
    {
        get
        {
            try
            {
                var endpoint = new Uri(this.Endpoint);
                return endpoint.Host;
            }
            catch (UriFormatException ex)
            {
                throw new ArgumentException($"{nameof(this.Endpoint)} value is malformed.", ex);
            }
        }
    }

    public int Port
    {
        get
        {
            try
            {
                var endpoint = new Uri(this.Endpoint);
                return endpoint.IsDefaultPort
                    ? throw new ArgumentException($"Port should be explicitly set in {nameof(this.Endpoint)} value.")
                    : endpoint.Port;
            }
            catch (UriFormatException ex)
            {
                throw new ArgumentException($"{nameof(this.Endpoint)} value is malformed.", ex);
            }
        }
    }

    public string Account
    {
        get => this.ThrowIfNotExists<string>(nameof(this.Account));
        set => this.parts[nameof(this.Account)] = value;
    }

    public string Namespace
    {
        get => this.ThrowIfNotExists<string>(nameof(this.Namespace));
        set => this.parts[nameof(this.Namespace)] = value;
    }

    public bool DisableMetricNameValidation
    {
        get => this.parts.TryGetValue(nameof(this.DisableMetricNameValidation), out var value) && string.Equals(bool.TrueString, value, StringComparison.OrdinalIgnoreCase);
        set => this.parts[nameof(this.DisableMetricNameValidation)] = value ? bool.TrueString : bool.FalseString;
    }

    public string ParseUnixDomainSocketPath()
    {
        try
        {
            var endpoint = new Uri(this.Endpoint);
            return ReplaceFirstChar(endpoint.AbsolutePath, '@', '\0');
        }
        catch (UriFormatException ex)
        {
            throw new ArgumentException($"{nameof(this.Endpoint)} value is malformed.", ex);
        }
    }

    public bool TryGetMetricsAccountAndNamespace(
        [NotNullWhen(true)] out string? metricsAccount,
        [NotNullWhen(true)] out string? metricsNamespace)
    {
        var hasAccount = this.parts.TryGetValue(nameof(this.Account), out metricsAccount);
        var hasNamespace = this.parts.TryGetValue(nameof(this.Namespace), out metricsNamespace);

        return hasAccount && hasNamespace;
    }

    /// <summary>
    /// Replace first charater of string if it matches with <paramref name="oldChar"/> with <paramref name="newChar"/>.
    /// </summary>
    /// <param name="str">String to be updated.</param>
    /// <param name="oldChar">Old character to be replaced.</param>
    /// <param name="newChar">New character to be replaced with.</param>
    /// <returns>Updated string.</returns>
    internal static string ReplaceFirstChar(string str, char oldChar, char newChar)
    {
        return str.Length > 0 && str[0] == oldChar ? $"{newChar}{str.Substring(1)}" : str;
    }

    private T ThrowIfNotExists<T>(string name)
    {
        return !this.parts.TryGetValue(name, out var value)
            ? throw new ArgumentException($"'{name}' value is missing in connection string.")
            : (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
    }
}
