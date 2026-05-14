// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics.Metrics;
using OpenTelemetry.Internal;

namespace OpenTelemetry.Instrumentation.Cassandra;

internal static class CassandraMeter
{
    static CassandraMeter()
    {
        var assembly = typeof(CassandraMeter).Assembly;
        Instance = new Meter(assembly.GetName().Name, assembly.GetPackageVersion());
    }

    public static Meter Instance { get; }
}
