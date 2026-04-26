// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;

namespace OpenTelemetry.Instrumentation.Owin;

/// <summary>
/// Describes the possible events fired when enriching an <see cref="Activity"/>.
/// </summary>
public enum OwinEnrichEventType
{
    /// <summary>
    /// Begin request.
    /// </summary>
    BeginRequest,

    /// <summary>
    /// End request.
    /// </summary>
    EndRequest,
}
