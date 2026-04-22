// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpenTelemetry.AWS;

namespace OpenTelemetry.Resources.AWS;

/// <summary>
/// AWS Resource builder options.
/// </summary>
public class AWSResourceBuilderOptions
{
    /// <inheritdoc cref="AWS.SemanticConventionVersion"/>
    public SemanticConventionVersion SemanticConventionVersion { get; set; } = AWSSemanticConventions.DefaultSemanticConventionVersion;
}
