// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using OpenTelemetry.Internal;

namespace OpenTelemetry.Logs;

internal sealed class ActivityEventAttachingLogProcessor : BaseProcessor<LogRecord>
{
    private static readonly Action<LogRecordScope, State> ProcessScope = (LogRecordScope scope, State state) =>
    {
        try
        {
            state.Processor.options.ScopeConverter?.Invoke(state.Tags, state.Depth++, scope);
        }
        catch (Exception ex)
        {
            OpenTelemetryExtensionsEventSource.Log.LogProcessorException($"Processing scope of type [{scope.GetType().FullName}]", ex);
        }
    };

    private readonly LogToActivityEventConversionOptions options;

    public ActivityEventAttachingLogProcessor(LogToActivityEventConversionOptions options)
    {
        Guard.ThrowIfNull(options);

        this.options = options;
    }

    public override void OnEnd(LogRecord data)
    {
        var activity = Activity.Current;

        if (activity?.IsAllDataRequested == true)
        {
            try
            {
                if (this.options.Filter?.Invoke(data) == false)
                {
                    return;
                }
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                OpenTelemetryExtensionsEventSource.Log.LogRecordFilterException(data.CategoryName, ex);
                return;
            }

            var tags = new ActivityTagsCollection
            {
                { nameof(data.CategoryName), data.CategoryName },
                { nameof(data.LogLevel), data.LogLevel },
            };

            if (data.EventId != 0)
            {
                tags[nameof(data.EventId)] = data.EventId;
            }

            data.ForEachScope(ProcessScope, new State(tags, this));

            if (data.Attributes != null)
            {
                try
                {
                    this.options.StateConverter?.Invoke(tags, data.Attributes);
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    OpenTelemetryExtensionsEventSource.Log.LogProcessorException($"Processing attributes for LogRecord with CategoryName [{data.CategoryName}]", ex);
                }
            }

            if (!string.IsNullOrEmpty(data.FormattedMessage))
            {
                tags[nameof(data.FormattedMessage)] = data.FormattedMessage;
            }

            var activityEvent = new ActivityEvent("log", data.Timestamp, tags);
            activity.AddEvent(activityEvent);

            if (data.Exception != null)
            {
                activity.AddException(data.Exception);
            }
        }
    }

    private sealed class State
    {
        public State(ActivityTagsCollection tags, ActivityEventAttachingLogProcessor processor)
        {
            this.Tags = tags;
            this.Processor = processor;
        }

        public ActivityTagsCollection Tags { get; }

        public ActivityEventAttachingLogProcessor Processor { get; }

        public int Depth { get; set; }
    }
}
