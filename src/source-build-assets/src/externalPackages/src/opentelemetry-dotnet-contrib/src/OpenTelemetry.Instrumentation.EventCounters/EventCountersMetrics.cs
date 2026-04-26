// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Collections.Concurrent;
using System.Diagnostics.Metrics;
using System.Diagnostics.Tracing;
using System.Globalization;
using OpenTelemetry.Internal;

namespace OpenTelemetry.Instrumentation.EventCounters;

/// <summary>
/// .NET EventCounters Instrumentation.
/// </summary>
internal sealed class EventCountersMetrics : EventListener
{
    private const string Prefix = "ec";
    private const int MaxInstrumentNameLength = 63;

    private readonly EventCountersInstrumentationOptions options;
    private readonly List<EventSource> preInitEventSources = [];
    private readonly List<EventSource> enabledEventSources = [];
    private readonly ConcurrentDictionary<(string, string), Instrument> instruments = new();
    private readonly ConcurrentDictionary<(string, string), double> values = new();
    private bool isDisposed;

    static EventCountersMetrics()
    {
        // Ensure EventCountersInstrumentationEventSource got initialized when the class was accessed for the first time
        // to prevent potential deadlock:
        // https://github.com/open-telemetry/opentelemetry-dotnet-contrib/issues/1024.
        _ = EventCountersInstrumentationEventSource.Log;

        var assembly = typeof(EventCountersMetrics).Assembly;
        MeterInstance = new Meter(assembly.GetName().Name, assembly.GetPackageVersion());
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EventCountersMetrics"/> class.
    /// </summary>
    /// <param name="options">The options to define the metrics.</param>
    public EventCountersMetrics(EventCountersInstrumentationOptions options)
    {
        lock (this.preInitEventSources)
        {
            this.options = options;

            foreach (var eventSource in this.preInitEventSources)
            {
                if (this.options.ShouldListenToSource(eventSource.Name))
                {
                    this.EnableEvents(eventSource);
                    this.enabledEventSources.Add(eventSource);
                }
            }

            this.preInitEventSources.Clear();
        }
    }

    internal static Meter MeterInstance { get; }

    /// <inheritdoc />
    public override void Dispose()
    {
        if (!this.isDisposed)
        {
            lock (this.preInitEventSources)
            {
                if (!this.isDisposed)
                {
                    foreach (var eventSource in this.enabledEventSources)
                    {
                        this.DisableEvents(eventSource);
                    }

                    this.isDisposed = true;
                }
            }

            // DO NOT clear the ConcurrentDictionary instances as some other thread executing the OnEventWritten callback might be using them
            this.enabledEventSources.Clear();
            this.options.EventSourceNames.Clear();
        }

        base.Dispose();
    }

    /// <inheritdoc />
    protected override void OnEventSourceCreated(EventSource eventSource)
    {
        lock (this.preInitEventSources)
        {
            if (this.isDisposed)
            {
                return;
            }

            if (this.options == null)
            {
                this.preInitEventSources.Add(eventSource);
            }
            else if (this.options.ShouldListenToSource(eventSource.Name))
            {
                this.EnableEvents(eventSource);
                this.enabledEventSources.Add(eventSource);
            }
        }
    }

    /// <inheritdoc />
    protected override void OnEventWritten(EventWrittenEventArgs eventData)
    {
        if (this.options == null)
        {
            return;
        }

        var eventSourceName = eventData.EventSource.Name;

        if (eventData.EventName != "EventCounters")
        {
            EventCountersInstrumentationEventSource.Log.IgnoreNonEventCountersName(eventSourceName);
            return;
        }

        if (eventData.Payload == null || eventData.Payload.Count == 0 || eventData.Payload[0] is not IDictionary<string, object> payload)
        {
            EventCountersInstrumentationEventSource.Log.IgnoreEventWrittenEventArgsPayloadNotParsable(eventSourceName);
            return;
        }

        var hasName = payload.TryGetValue("Name", out var nameObj);

        if (!hasName)
        {
            EventCountersInstrumentationEventSource.Log.IgnoreEventWrittenEventArgsWithoutName(eventSourceName);
            return;
        }

        var name = nameObj.ToString();

        var hasMean = payload.TryGetValue("Mean", out var mean);
        var hasIncrement = payload.TryGetValue("Increment", out var increment);

        if (!(hasIncrement ^ hasMean))
        {
            EventCountersInstrumentationEventSource.Log.IgnoreMeanIncrementConflict(eventSourceName);
            return;
        }

        var value = Convert.ToDouble(hasMean ? mean : increment, CultureInfo.InvariantCulture);
        this.UpdateInstrumentWithEvent(hasMean, eventSourceName, name, value);
    }

    private static Dictionary<string, string> GetEnableEventsArguments(EventCountersInstrumentationOptions options) =>
        new() { { "EventCounterIntervalSec", options.RefreshIntervalSecs.ToString(CultureInfo.InvariantCulture) } };

    /// <summary>
    /// If the resulting instrument name is too long, it trims the event source name
    /// to fit in as many characters as possible keeping the event name intact.
    /// E.g. instrument for `Microsoft-AspNetCore-Server-Kestrel`, `tls-handshakes-per-second`
    /// would be too long (64 chars), so it's shortened to `ec.Microsoft-AspNetCore-Server-Kestre.tls-handshakes-per-second`.
    ///
    /// If there is no room for event source name, returns `ec.{event name}` and
    /// if it's still too long, it will be validated and ignored later in the pipeline.
    /// </summary>
    private static string GetInstrumentName(string sourceName, string eventName)
    {
        var totalLength = Prefix.Length + 1 + sourceName.Length + 1 + eventName.Length;
        if (totalLength <= MaxInstrumentNameLength)
        {
            return string.Concat(Prefix, ".", sourceName, ".", eventName);
        }

        var maxEventSourceLength = MaxInstrumentNameLength - Prefix.Length - 2 - eventName.Length;
        if (maxEventSourceLength < 1)
        {
            // event name is too long, there is not enough space for sourceName.
            // let ec.<eventName> flow to metrics SDK and it will suppress it if needed.
            return string.Concat(Prefix, ".", eventName);
        }

        while (maxEventSourceLength > 0 && (sourceName[maxEventSourceLength - 1] == '.' || sourceName[maxEventSourceLength - 1] == '-'))
        {
            maxEventSourceLength--;
        }

        return string.Concat(Prefix, ".", sourceName.Substring(0, maxEventSourceLength), ".", eventName);
    }

    private void EnableEvents(EventSource eventSource)
    {
        this.EnableEvents(eventSource, EventLevel.Critical, EventKeywords.None, GetEnableEventsArguments(this.options));
    }

    private void UpdateInstrumentWithEvent(bool isGauge, string eventSourceName, string name, double value)
    {
        try
        {
            ValueTuple<string, string> metricKey = new(eventSourceName, name);
            _ = this.values.AddOrUpdate(metricKey, value, isGauge ? (_, _) => value : (_, existing) => existing + value);

            if (!this.instruments.ContainsKey(metricKey))
            {
                var instrumentName = GetInstrumentName(eventSourceName, name);
                Instrument instrument = isGauge
                    ? MeterInstance.CreateObservableGauge(instrumentName, () => this.values[metricKey])
                    : MeterInstance.CreateObservableCounter(instrumentName, () => this.values[metricKey]);
                _ = this.instruments.TryAdd(metricKey, instrument);
            }
        }
        catch (Exception ex)
        {
            EventCountersInstrumentationEventSource.Log.ErrorWhileWritingEvent(eventSourceName, ex.Message);
        }
    }
}
