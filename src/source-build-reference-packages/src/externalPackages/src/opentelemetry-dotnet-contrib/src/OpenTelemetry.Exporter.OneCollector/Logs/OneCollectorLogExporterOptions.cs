// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;

namespace OpenTelemetry.Exporter.OneCollector;

/// <summary>
/// Contains options used to build a <see cref="OneCollectorExporter{T}"/>
/// instance for exporting <see cref="LogRecord"/> telemetry data.
/// </summary>
public sealed class OneCollectorLogExporterOptions : OneCollectorExporterOptions
{
    private IReadOnlyDictionary<string, string>? eventFullNameMappings;

    /// <summary>
    /// Gets or sets the default event namespace. Default value:
    /// <c>OpenTelemetry.Logs</c>.
    /// </summary>
    /// <remarks>
    /// Notes:
    /// <list type="bullet">
    /// <item>The default event namespace is used if a <see
    /// cref="LogRecord.CategoryName"/> is not supplied or if <see
    /// cref="EventFullNameMappings"/> are defined and an event full name does
    /// not match a defined rule.</item>
    /// <item>Can be set to empty string <c>""</c> but cannot be <see
    /// langword="null"/>.</item>
    /// <item>
    /// When combined with <see cref="DefaultEventName"/> the final default full
    /// event name value must be at least four characters long and cannot be
    /// more than 100 characters long.
    /// </item>
    /// </list>
    /// </remarks>
    public string DefaultEventNamespace { get; set; } = "OpenTelemetry.Logs";

    /// <summary>
    /// Gets or sets the default event name. Default value: <c>Log</c>.
    /// </summary>
    /// <remarks>
    /// Notes:
    /// <list type="bullet">
    /// <item>The default event name is used when an <see
    /// cref="LogRecord.EventId"/> has a null, whitespace, or invalid <see
    /// cref="EventId.Name"/> or if <see cref="EventFullNameMappings"/> are
    /// defined and an event full name does not match a defined rule.</item>
    /// <item>
    /// Cannot be set to empty string <c>""</c> or <see langword="null"/>.
    /// </item>
    /// <item>
    /// When combined with <see cref="DefaultEventNamespace"/> the final default
    /// full event name value must be at least four characters long and cannot
    /// be more than 100 characters long.
    /// </item>
    /// </list>
    /// </remarks>
    public string DefaultEventName { get; set; } = "Log";

    /// <summary>
    /// Gets or sets the event full name mappings.
    /// </summary>
    /// <remarks>
    /// <para>Event full name mappings may be used to change the final event
    /// namespace and/or event name for a given <see cref="LogRecord"/>. By
    /// default (when <see cref="EventFullNameMappings"/> is not used) event
    /// full name is derived from the <see cref="LogRecord.CategoryName"/>
    /// (event namespace) and <see cref="EventId.Name"/> (event name)
    /// values.</para>
    /// Notes:
    /// <list type="bullet">
    /// <item>Key:
    /// <list type="bullet">
    /// <item>Key may be <c>*</c> to set the default mapping rule. Only a single
    /// default mapping rule may be defined. Example: <c>*</c>.</item>
    /// <item>Key may be a fully qualified event name (exact match rule).
    /// Example: <c>MyCompany.Library.EventName</c>.</item>
    /// <item>Key may be a prefix (<c>StartsWith</c> rule). Example:
    /// <c>MyCompany.Library</c>.</item>
    /// </list>
    /// </item>
    /// <item>Value:
    /// <list type="bullet">
    /// <item>Value may be an event name without a namespace. Example:
    /// <c>MyNewEventName</c>.</item>
    /// <item>Value may be a a fully qualified event name. Example:
    /// <c>MyCompany.Library.MyNewEventName</c>.</item>
    /// <item>Value may be <c>*</c> to pass-through the originally resolved full
    /// event name value. Example: <c>*</c>.</item>
    /// </list>
    /// </item>
    /// </list>
    /// </remarks>
    public IReadOnlyDictionary<string, string>? EventFullNameMappings
    {
        get => this.eventFullNameMappings;
        set
        {
            if (value == null)
            {
                this.eventFullNameMappings = null;
                return;
            }

            var copy = new Dictionary<string, string>(value.Count);

            foreach (var entry in value)
            {
                copy[entry.Key] = entry.Value;
            }

            this.eventFullNameMappings = copy;
        }
    }

    /// <summary>
    /// Gets the OneCollector log serialization options.
    /// </summary>
    public OneCollectorLogExporterSerializationOptions SerializationOptions { get; } = new();

    internal IReadOnlyDictionary<string, EventFullName>? ParsedEventFullNameMappings { get; private set; }

    internal override void Validate()
    {
        if (this.DefaultEventNamespace?.Length != 0
            && (this.DefaultEventNamespace == null
                || !EventNameManager.IsEventNamespaceValid(this.DefaultEventNamespace!)))
        {
            throw new OneCollectorExporterValidationException($"{nameof(this.DefaultEventNamespace)} was not specified on {nameof(OneCollectorLogExporterOptions)} options or was invalid.");
        }

        if (string.IsNullOrWhiteSpace(this.DefaultEventName)
            || !EventNameManager.IsEventNameValid(this.DefaultEventName))
        {
            throw new OneCollectorExporterValidationException($"{nameof(this.DefaultEventName)} was not specified on {nameof(OneCollectorLogExporterOptions)} options or was invalid.");
        }

        var defaultEventFullNameLength = this.DefaultEventName.Length
            + this.DefaultEventNamespace.Length
            + (this.DefaultEventNamespace.Length > 0 ? 1 : 0);

        if (defaultEventFullNameLength is < EventNameManager.MinimumEventFullNameLength
            or > EventNameManager.MaximumEventFullNameLength)
        {
            throw new OneCollectorExporterValidationException($"{nameof(this.DefaultEventNamespace)} & {nameof(this.DefaultEventName)} specified on {nameof(OneCollectorLogExporterOptions)} options cannot be less than 4 characters or greater than 100 characters in length when combined.");
        }

        var eventFullNameMappings = this.eventFullNameMappings;
        if (eventFullNameMappings != null)
        {
            var parsedEventFullNameMappings = new Dictionary<string, EventFullName>(
                capacity: eventFullNameMappings.Count,
                comparer: StringComparer.OrdinalIgnoreCase);

            foreach (var entry in eventFullNameMappings)
            {
                if (string.IsNullOrWhiteSpace(entry.Key))
                {
                    throw new OneCollectorExporterValidationException("An event full name mapping key was null, empty, or consisted only of white-space characters.");
                }

                if (!EventFullName.TryParseFromMapping(entry.Value, out var eventFullName))
                {
                    throw new OneCollectorExporterValidationException($"The event full name mapping value provided for key '{entry.Key}' was null.");
                }

                eventFullName!.Validate(entry.Key);

                parsedEventFullNameMappings.Add(entry.Key, eventFullName);
            }

            this.ParsedEventFullNameMappings = parsedEventFullNameMappings;
        }

        this.SerializationOptions.Validate();

        base.Validate();
    }
}
