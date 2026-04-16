// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.Exporter.Geneva;

/// <summary>
/// Helpers to convert .NET time related types to the timestamp used in OTLP.
/// </summary>
internal static class TimestampHelpers
{
    private const long NanosecondsPerTick = 100;
    private const long UnixEpochTicks = 621355968000000000; // = DateTimeOffset.FromUnixTimeMilliseconds(0).Ticks

    internal static long ToUnixTimeNanoseconds(this DateTimeOffset dto)
    {
        return (dto.Ticks - UnixEpochTicks) * NanosecondsPerTick;
    }
}
