// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if !NET
using System.Runtime.Serialization;
#endif

namespace OpenTelemetry.Exporter.OneCollector;

/// <summary>
/// Represents errors that occur validating OneCollectorExporter configuration.
/// </summary>
[Serializable]
public sealed class OneCollectorExporterValidationException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OneCollectorExporterValidationException"/> class.
    /// </summary>
    public OneCollectorExporterValidationException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="OneCollectorExporterValidationException"/> class.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public OneCollectorExporterValidationException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="OneCollectorExporterValidationException"/> class.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the
    /// exception.</param>
    /// <param name="innerException">The exception that is the cause of the
    /// current exception, or a <see langword="null"/> reference if no inner
    /// exception is specified.</param>
    public OneCollectorExporterValidationException(string message, Exception? innerException)
        : base(message, innerException)
    {
    }

#if !NET
    private OneCollectorExporterValidationException(SerializationInfo serializationInfo, StreamingContext streamingContext)
        : base(serializationInfo, streamingContext)
    {
    }
#endif
}
