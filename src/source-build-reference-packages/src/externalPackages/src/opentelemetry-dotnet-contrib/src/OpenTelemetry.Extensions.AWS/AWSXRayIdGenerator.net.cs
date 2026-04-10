// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if NET
using System.Buffers.Binary;
using System.Diagnostics;
using System.Security.Cryptography;
using OpenTelemetry.Trace;

namespace OpenTelemetry.Extensions.AWS;

/// <summary>
/// Generate AWS X-Ray compatible trace IDs.
/// See https://docs.aws.amazon.com/xray/latest/devguide/xray-api-sendingdata.html#xray-api-traceids.
/// </summary>
public static class AWSXRayIdGenerator
{
    /// <summary>
    /// Sets <see cref="Activity.TraceIdGenerator"/> to <see cref="GenerateAWSXRayCompatibleTraceId"/>.
    /// </summary>
    public static void ReplaceTraceId()
    {
        Activity.TraceIdGenerator = GenerateAWSXRayCompatibleTraceId;
    }

    /// <summary>
    /// Sets <see cref="Activity.TraceIdGenerator"/> to <see cref="GenerateAWSXRayCompatibleTraceId"/>.
    /// </summary>
    /// <param name="sampler">Unused. (See deprecation message.)</param>
    [Obsolete($"When targeting .NET 6.0 or later, the X-Ray ID generator does not need to update the sampling decision. Use ${nameof(ReplaceTraceId)}() instead.")]
    public static void ReplaceTraceId(Sampler sampler)
    {
        ReplaceTraceId();
    }

    /// <summary>
    /// Generates an AWS X-Ray compatible trace ID.
    /// </summary>
    /// <returns>
    /// An <see cref="ActivityTraceId"/> whose first 4 bytes are the big-endian unix timestamp (in seconds) and whose
    /// remaining 12 bytes are randomly generated.
    /// </returns>
    internal static ActivityTraceId GenerateAWSXRayCompatibleTraceId()
    {
        Span<byte> buffer = stackalloc byte[16];

        // intentionally truncating to 4 bytes because AWS X-Ray requires 8 hex characters
        var seconds = (uint)DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        _ = BinaryPrimitives.TryWriteUInt32BigEndian(buffer, seconds);

        // fill the rest of the buffer with random bytes
        RandomNumberGenerator.Fill(buffer.Slice(4, 12));

        return ActivityTraceId.CreateFromBytes(buffer);
    }
}
#endif
