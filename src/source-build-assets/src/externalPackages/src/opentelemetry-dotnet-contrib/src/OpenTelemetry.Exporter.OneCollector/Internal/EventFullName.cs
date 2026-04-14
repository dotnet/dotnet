// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if NET || NETSTANDARD2_1_OR_GREATER
using System.Diagnostics.CodeAnalysis;
#endif

namespace OpenTelemetry.Exporter.OneCollector;

internal sealed class EventFullName
{
    private EventFullName(
        string eventNamespace,
        string eventName)
    {
        this.EventNamespace = eventNamespace;
        this.EventName = eventName;
    }

    public string EventNamespace { get; }

    public string EventName { get; }

    public static EventFullName Create(string eventName)
    {
        return new(eventNamespace: string.Empty, eventName: eventName);
    }

    public static bool TryParseFromMapping(
        string? eventFullNameMapping,
#if NET || NETSTANDARD2_1_OR_GREATER
        [NotNullWhen(true)]
#endif
        out EventFullName? eventFullName)
    {
        if (string.IsNullOrWhiteSpace(eventFullNameMapping))
        {
            eventFullName = null;
            return false;
        }

        var parts = eventFullNameMapping!.Split('.');
        eventFullName = parts.Length > 1
            ? new(
                string.Join(".", parts, 0, parts.Length - 1),
                parts[parts.Length - 1])
            : new(string.Empty, parts[0]);

        return true;
    }

    internal void Validate(string key)
    {
        if (this.EventNamespace?.Length != 0
            && (this.EventNamespace == null
                || !EventNameManager.IsEventNamespaceValid(this.EventNamespace!)))
        {
            throw new OneCollectorExporterValidationException($"The event full name mapping namespace value provided for key '{key}' was null or invalid.");
        }

        if (this.EventName != "*"
            && (string.IsNullOrWhiteSpace(this.EventName)
                || !EventNameManager.IsEventNameValid(this.EventName!)))
        {
            throw new OneCollectorExporterValidationException($"The event full name mapping name value provided for key '{key}' was null or invalid.");
        }

        if (this.EventName != "*")
        {
            var length = this.EventNamespace.Length;
            if (length > 0)
            {
                length++;
            }

            length += this.EventName.Length;

            if (length is < EventNameManager.MinimumEventFullNameLength
                or > EventNameManager.MaximumEventFullNameLength)
            {
                throw new OneCollectorExporterValidationException($"The event full name mapping value provided for key '{key}' is shorter or longer than what is allowed.");
            }
        }
        else if (this.EventNamespace.Length != 0)
        {
            throw new OneCollectorExporterValidationException($"The event full name mapping value provided for key '{key}' has an invalid wild card pattern.");
        }
    }
}
