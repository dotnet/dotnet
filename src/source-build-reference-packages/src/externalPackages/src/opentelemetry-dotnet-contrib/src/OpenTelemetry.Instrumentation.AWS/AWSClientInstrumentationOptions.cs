// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpenTelemetry.AWS;

namespace OpenTelemetry.Instrumentation.AWS;

/// <summary>
/// Options for AWS client instrumentation.
/// </summary>
public class AWSClientInstrumentationOptions
{
    /// <summary>
    /// Gets or sets a value indicating whether downstream instrumentation is suppressed.
    /// </summary>
    public bool SuppressDownstreamInstrumentation { get; set; }

    /// <inheritdoc cref="AWS.SemanticConventionVersion"/>
    public SemanticConventionVersion SemanticConventionVersion { get; set; } = AWSSemanticConventions.DefaultSemanticConventionVersion;
}
