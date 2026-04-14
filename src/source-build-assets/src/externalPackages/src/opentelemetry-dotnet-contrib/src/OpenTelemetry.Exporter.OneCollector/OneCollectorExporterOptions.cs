// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.ComponentModel.DataAnnotations;

namespace OpenTelemetry.Exporter.OneCollector;

/// <summary>
/// Contains options for the <see cref="OneCollectorExporter{T}"/> class.
/// </summary>
public abstract class OneCollectorExporterOptions
{
    internal OneCollectorExporterOptions()
    {
    }

    /// <summary>
    /// Gets or sets the OneCollector connection string.
    /// </summary>
    /// <remarks>
    /// Note: Connection string is required.
    /// </remarks>
    [Required]
    public string? ConnectionString { get; set; }

    /// <summary>
    /// Gets the OneCollector transport options.
    /// </summary>
    public OneCollectorExporterTransportOptions TransportOptions { get; } = new();

    /// <summary>
    /// Gets the OneCollector instrumentation key.
    /// </summary>
    internal string? InstrumentationKey { get; private set; }

    /// <summary>
    /// Gets the OneCollector tenant token.
    /// </summary>
    internal string? TenantToken { get; private set; }

    internal virtual void Validate()
    {
        if (string.IsNullOrWhiteSpace(this.ConnectionString))
        {
            throw new OneCollectorExporterValidationException($"{nameof(this.ConnectionString)} was not specified on {this.GetType().Name} options.");
        }

        this.InstrumentationKey = new ConnectionStringParser(this.ConnectionString!).InstrumentationKey;
        if (string.IsNullOrWhiteSpace(this.InstrumentationKey))
        {
            throw new OneCollectorExporterValidationException("Instrumentation key was not specified on connection string.");
        }

#if NET || NETSTANDARD2_1_OR_GREATER
        var positionOfFirstDash = this.InstrumentationKey.IndexOf('-', StringComparison.OrdinalIgnoreCase);
#else
        var positionOfFirstDash = this.InstrumentationKey!.IndexOf('-');
#endif
        if (positionOfFirstDash < 0)
        {
            throw new OneCollectorExporterValidationException($"Instrumentation key specified as part of {nameof(this.ConnectionString)} on {this.GetType().Name} options is invalid.");
        }

        this.TenantToken = this.InstrumentationKey.Substring(0, positionOfFirstDash);

        this.TransportOptions.Validate();
    }
}
