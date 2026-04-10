// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpenTelemetry.Internal;

namespace OpenTelemetry.Exporter.OneCollector;

internal sealed class ConnectionStringParser
{
    public ConnectionStringParser(string connectionString)
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

#if NET || NETSTANDARD2_1_OR_GREATER
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
                throw new ArgumentException("Connection string cannot contain empty keys or values.", nameof(connectionString));
            }

            this.ParsedKeyValues[key] = value;
        }

        if (this.ParsedKeyValues.Count == 0)
        {
            throw new ArgumentException("Connection string is invalid.", nameof(connectionString));
        }
    }

    public string? InstrumentationKey
    {
        get
        {
            this.ParsedKeyValues.TryGetValue(nameof(this.InstrumentationKey), out var instrumentationKey);

            return instrumentationKey;
        }
    }

    internal Dictionary<string, string> ParsedKeyValues { get; } = new(StringComparer.Ordinal);
}
