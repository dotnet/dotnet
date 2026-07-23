// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using OpenTelemetry.Trace;

namespace OpenTelemetry.Instrumentation.GrpcCore;

/// <summary>
/// Instrumentation class Grpc.Core.
/// </summary>
internal static class GrpcCoreInstrumentation
{
    /// <summary>
    /// Gets the version of the RPC Semantic Conventions used by the instrumentation.
    /// </summary>
    internal static readonly Version SemanticConventionsVersion = new(1, 42, 0);

    /// <summary>
    /// Gets the activity source for the instrumentation.
    /// </summary>
    internal static readonly ActivitySource ActivitySource = ActivitySourceFactory.Create(typeof(GrpcCoreInstrumentation), SemanticConventionsVersion);
}
