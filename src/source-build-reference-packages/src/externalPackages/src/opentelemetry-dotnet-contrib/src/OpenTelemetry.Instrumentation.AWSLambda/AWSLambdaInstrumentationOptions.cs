// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpenTelemetry.AWS;

namespace OpenTelemetry.Instrumentation.AWSLambda;

/// <summary>
/// AWS lambda instrumentation options.
/// </summary>
public class AWSLambdaInstrumentationOptions
{
    /// <summary>
    /// Gets or sets a value indicating whether AWS X-Ray context extraction should be disabled.
    /// </summary>
    public bool DisableAwsXRayContextExtraction { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the parent Activity should be set when a potentially batched event is received where multiple parents are potentially available (e.g. SQS).
    /// If set to true, the parent is set using the last received record (e.g. last message). Otherwise the parent is not set. In both cases, links will be created for such events.
    /// </summary>
    /// <remarks>
    /// Currently, the only event type to which this applies is SQS.
    /// </remarks>
    public bool SetParentFromBatch { get; set; }

    /// <inheritdoc cref="AWSLambda.SemanticConventionVersion"/>
    public SemanticConventionVersion SemanticConventionVersion { get; set; } = AWSSemanticConventions.DefaultSemanticConventionVersion;
}
