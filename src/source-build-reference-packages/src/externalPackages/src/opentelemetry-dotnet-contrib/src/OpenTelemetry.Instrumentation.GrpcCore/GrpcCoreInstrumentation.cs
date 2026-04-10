// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using System.Reflection;
using OpenTelemetry.Internal;

namespace OpenTelemetry.Instrumentation.GrpcCore;

/// <summary>
/// Instrumentation class Grpc.Core.
/// </summary>
internal static class GrpcCoreInstrumentation
{
    /// <summary>
    /// The assembly.
    /// </summary>
    internal static readonly Assembly Assembly = typeof(GrpcCoreInstrumentation).Assembly;

    /// <summary>
    /// The assembly name.
    /// </summary>
    internal static readonly AssemblyName AssemblyName = Assembly.GetName();

    /// <summary>
    /// The activity source name.
    /// </summary>
    internal static readonly string ActivitySourceName = AssemblyName.Name!;

    /// <summary>
    /// The activity source.
    /// </summary>
    internal static readonly ActivitySource ActivitySource = new(ActivitySourceName, Assembly.GetPackageVersion());
}
