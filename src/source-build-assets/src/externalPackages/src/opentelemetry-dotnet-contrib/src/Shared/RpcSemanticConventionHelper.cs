// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;

namespace OpenTelemetry.Internal;

/// <summary>
/// Helper class for RPC Semantic Conventions.
/// </summary>
/// <remarks>
/// Due to a breaking change in the semantic conventions, affected instrumentation libraries
/// must inspect an environment variable to determine which attributes to emit.
/// This is expected to be removed when the instrumentation libraries reach Stable.
/// <see href="https://github.com/open-telemetry/semantic-conventions/blob/v1.23.0/docs/rpc/rpc-spans.md"/>.
/// <see href="https://github.com/open-telemetry/semantic-conventions/blob/v1.41.0/docs/rpc/rpc-spans.md"/>.
/// </remarks>
internal static class RpcSemanticConventionHelper
{
    internal const string SemanticConventionOptInKeyName = "OTEL_SEMCONV_STABILITY_OPT_IN";
    internal static readonly char[] Separator = [',', ' '];

    [Flags]
    internal enum RpcSemanticConvention
    {
        /// <summary>
        /// Instructs an instrumentation library to emit the old experimental RPC attributes.
        /// </summary>
        Old = 0x1,

        /// <summary>
        /// Instructs an instrumentation library to emit the new, v1.23.0 RPC attributes.
        /// </summary>
        New = 0x2,

        /// <summary>
        /// Instructs an instrumentation library to emit both the old and new attributes.
        /// </summary>
        Dupe = Old | New,
    }

    public static RpcSemanticConvention GetSemanticConventionOptIn(IConfiguration configuration)
    {
        if (TryGetConfiguredValues(configuration, out var values))
        {
            if (values.Contains("rpc/dup"))
            {
                return RpcSemanticConvention.Dupe;
            }
            else if (values.Contains("rpc"))
            {
                return RpcSemanticConvention.New;
            }
        }

        return RpcSemanticConvention.Old;
    }

    private static bool TryGetConfiguredValues(IConfiguration configuration, [NotNullWhen(true)] out HashSet<string>? values)
    {
        try
        {
            var stringValue = configuration[SemanticConventionOptInKeyName];

            if (string.IsNullOrWhiteSpace(stringValue))
            {
                values = null;
                return false;
            }

#pragma warning disable IDE0370 // Suppression is unnecessary
            var stringValues = stringValue!.Split(separator: Separator, options: StringSplitOptions.RemoveEmptyEntries);
#pragma warning restore IDE0370 // Suppression is unnecessary
            values = new HashSet<string>(stringValues, StringComparer.OrdinalIgnoreCase);
            return true;
        }
        catch
        {
            values = null;
            return false;
        }
    }
}
