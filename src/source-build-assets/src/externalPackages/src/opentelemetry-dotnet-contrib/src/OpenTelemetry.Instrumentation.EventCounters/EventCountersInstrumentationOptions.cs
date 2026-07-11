// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.Instrumentation.EventCounters;

/// <summary>
/// EventCounters Instrumentation Options.
/// </summary>
public class EventCountersInstrumentationOptions
{
    private HashSet<string> eventSourceNames = [];

    /// <summary>
    /// Gets or sets the subscription interval in seconds for reading values
    /// from the configured EventCounters.
    /// </summary>
    public int RefreshIntervalSecs { get; set; } = 1;

    /// <summary>
    /// Listens to EventCounters from the given EventSource name.
    /// </summary>
    /// <param name="names">The EventSource names to listen to.</param>
    /// <exception cref="NotSupportedException">Thrown if <paramref name="names"/> contains the <c>System.Runtime"</c> EventSource.</exception>"
    public void AddEventSources(params string[] names)
    {
        if (names.Contains("System.Runtime"))
        {
            throw new NotSupportedException("Use the `OpenTelemetry.Instrumentation.Runtime` or `OpenTelemetry.Instrumentation.Process` instrumentations.");
        }

        this.eventSourceNames = [.. this.eventSourceNames.Union(names)];
    }

    internal void ClearEventSources() => this.eventSourceNames = [];

    /// <summary>
    /// Returns whether or not an EventSource should be enabled on the EventListener.
    /// </summary>
    /// <param name="eventSourceName">The EventSource name.</param>
    /// <returns><c>true</c> when an EventSource with the name <paramref name="eventSourceName"/> should be enabled.</returns>
    internal bool ShouldListenToSource(string eventSourceName)
        => this.eventSourceNames.Contains(eventSourceName);
}
