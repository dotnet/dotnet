namespace Microsoft.ApplicationInsights.DependencyCollector.Implementation.EventHandlers
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights.Common;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.W3C;
    using Microsoft.ApplicationInsights.W3C.Internal;

    /// <summary>
    /// Base implementation of diagnostic event handler.
    /// </summary>
    internal abstract class DiagnosticsEventHandlerBase : IDiagnosticEventHandler
    {
        protected const string StatusPropertyName = "Status";

        protected readonly TelemetryClient TelemetryClient;

        // Every fetcher is unique for event payload and particular property into this payload
        // when we first receive an event and require particular property, we cache fetcher for it
        // There are just a few (~10) events we can receive and each will a have a few payload fetchers
        // I.e. this dictionary is quite small, does not grow up after service warm-up and does not require clean up
        private readonly ConcurrentDictionary<Property, PropertyFetcher> propertyFetchers = new ConcurrentDictionary<Property, PropertyFetcher>();

        protected DiagnosticsEventHandlerBase(TelemetryConfiguration configuration)
        {
            this.TelemetryClient = new TelemetryClient(configuration);
        }

        protected DiagnosticsEventHandlerBase(TelemetryClient client)
        {
            this.TelemetryClient = client;
        }

        public virtual bool IsEventEnabled(string evnt, object arg1, object arg2)
        {
            return !evnt.EndsWith(TelemetryDiagnosticSourceListener.ActivityStartNameSuffix, StringComparison.Ordinal);
        }

        public abstract void OnEvent(KeyValuePair<string, object> evnt, DiagnosticListener ignored);

        protected void SetCommonProperties(string eventName, object eventPayload, Activity activity, OperationTelemetry telemetry)
        {
            if (string.IsNullOrEmpty(telemetry.Name))
            {
                telemetry.Name = this.GetOperationName(eventName, eventPayload, activity);
            }

            telemetry.Duration = activity.Duration;
            telemetry.Timestamp = activity.StartTimeUtc;

            if (activity.IdFormat == ActivityIdFormat.W3C)
            {
                var traceId = activity.TraceId.ToHexString();
                telemetry.Context.Operation.Id = traceId;

                if (string.IsNullOrEmpty(telemetry.Context.Operation.ParentId))
                {
                    if (activity.ParentSpanId != default)
                    {
                        telemetry.Context.Operation.ParentId = activity.ParentSpanId.ToHexString();
                    }
                    else if (!string.IsNullOrEmpty(activity.ParentId))
                    {
                        // W3C activity with non-W3C parent must keep parentId
                        telemetry.Context.Operation.ParentId = activity.ParentId;
                    }
                }

                telemetry.Id = activity.SpanId.ToHexString();
            }
            else
            {
                telemetry.Id = activity.Id;
                telemetry.Context.Operation.Id = activity.RootId;
                telemetry.Context.Operation.ParentId = activity.ParentId;
            }

            this.PopulateTags(activity, telemetry);

            foreach (var item in activity.Baggage)
            {
                if (!telemetry.Properties.ContainsKey(item.Key))
                {
                    telemetry.Properties[item.Key] = item.Value;
                }
            }

            if (!telemetry.Success.HasValue || telemetry.Success.Value)
            {
                telemetry.Success = this.IsOperationSuccessful(eventName, eventPayload, activity);
            }
        }

        protected virtual void PopulateTags(Activity activity, OperationTelemetry telemetry)
        {
            foreach (var item in activity.Tags)
            {
                if (!telemetry.Properties.ContainsKey(item.Key))
                {
                    telemetry.Properties[item.Key] = item.Value;
                }
            }
        }

        protected virtual string GetOperationName(string eventName, object eventPayload, Activity activity)
        {
            // activity name looks like 'Microsoft.Azure.<...>.<Name>'
            // as namespace is too verbose, we'll just take the last node from the activity name as telemetry name
            string activityName = activity.OperationName;
            int lastDotIndex = activityName.LastIndexOf('.');
            if (lastDotIndex >= 0)
            {
                return activityName.Substring(lastDotIndex + 1);
            }

            return activityName;
        }

        protected virtual bool IsOperationSuccessful(string eventName, object eventPayload, Activity activity)
        {
            return this.FetchPayloadProperty<TaskStatus>(eventName, StatusPropertyName, eventPayload) == TaskStatus.RanToCompletion;
        }

        protected T FetchPayloadProperty<T>(string eventName, string propertyName, object payload)
        {
            Property property = new Property(eventName, propertyName);
            var fetcher = this.propertyFetchers.GetOrAdd(property, prop => new PropertyFetcher(prop.PropertyName));
            return (T)fetcher.Fetch(payload);
        }

        private struct Property : IEquatable<Property>
        {
            public readonly string PropertyName;
            private readonly string eventName;

            public Property(string eventName, string propertyName)
            {
                this.eventName = eventName;
                this.PropertyName = propertyName;
            }

            public bool Equals(Property other)
            {
                return this.eventName == other.eventName && this.PropertyName == other.PropertyName;
            }

            public override bool Equals(object obj)
            {
                if (obj == null)
                {
                    return false;
                }

                return obj is Property property && this.Equals(property);
            }

            public override int GetHashCode()
            {
                return this.eventName.GetHashCode() ^ this.PropertyName.GetHashCode();
            }
        }
    }
}
