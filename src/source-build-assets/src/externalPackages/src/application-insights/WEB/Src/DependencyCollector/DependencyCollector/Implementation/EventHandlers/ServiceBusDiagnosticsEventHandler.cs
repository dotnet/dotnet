namespace Microsoft.ApplicationInsights.DependencyCollector.Implementation.EventHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.W3C.Internal;

    /// <summary>
    /// Implements ServiceBus DiagnosticSource events handling.
    /// </summary>
    internal class ServiceBusDiagnosticsEventHandler : DiagnosticsEventHandlerBase
    {
        public const string DiagnosticSourceName = "Microsoft.Azure.ServiceBus";
        private const string EntityPropertyName = "Entity";
        private const string EndpointPropertyName = "Endpoint";

        internal ServiceBusDiagnosticsEventHandler(TelemetryConfiguration configuration) : base(configuration)
        {
        }

        public override bool IsEventEnabled(string evnt, object arg1, object arg2)
        {
            return true;
        }

        public override void OnEvent(KeyValuePair<string, object> evnt, DiagnosticListener ignored)
        {
            Activity currentActivity = Activity.Current;

            switch (evnt.Key)
            {
                case "Microsoft.Azure.ServiceBus.ProcessSession.Start":
                case "Microsoft.Azure.ServiceBus.Process.Start":
                    // if Activity is W3C, but there is a parent id which is not W3C.
                    if (currentActivity.IdFormat == ActivityIdFormat.W3C && !string.IsNullOrEmpty(currentActivity.ParentId) && currentActivity.ParentSpanId == default)
                    {
                        // if hierarchical parent has compatible rootId, reuse it and keep legacy parentId
                        if (W3CUtilities.TryGetTraceId(currentActivity.ParentId, out var traceId))
                        {
#pragma warning disable CA2000 // Dispose objects before losing scope
                            // Since we don't know when it will finish, we will not dispose
                            var backCompatActivity = new Activity(currentActivity.OperationName);
#pragma warning restore CA2000 // Dispose objects before losing scope
                            backCompatActivity.SetParentId(ActivityTraceId.CreateFromString(traceId), default, currentActivity.ActivityTraceFlags);
                            backCompatActivity.Start();
                            backCompatActivity.AddTag("__legacyParentId", currentActivity.ParentId);
                            foreach (var tag in currentActivity.Tags)
                            {
                                backCompatActivity.AddTag(tag.Key, tag.Value);
                            }

                            foreach (var baggage in currentActivity.Baggage)
                            {
                                backCompatActivity.AddBaggage(baggage.Key, baggage.Value);
                            }
                        }
                        else
                        {
                            currentActivity.AddTag(W3C.W3CConstants.LegacyRootPropertyIdKey, W3CUtilities.GetRootId(currentActivity.ParentId));
                        }
                    }

                    break;
                case "Microsoft.Azure.ServiceBus.ProcessSession.Stop":
                case "Microsoft.Azure.ServiceBus.Process.Stop":
                    bool isBackCompatActivity = false;
                    if (currentActivity.Duration == TimeSpan.Zero)
                    {
                        isBackCompatActivity = true;
                        currentActivity.SetEndTime(DateTime.UtcNow);
                    }

                    this.OnRequest(evnt.Key, evnt.Value, currentActivity);

                    if (isBackCompatActivity)
                    {
                        currentActivity.Stop();
                    }

                    break;
                case "Microsoft.Azure.ServiceBus.Exception":
                    break;
                default:
                    if (evnt.Key.EndsWith(TelemetryDiagnosticSourceListener.ActivityStopNameSuffix, StringComparison.Ordinal))
                    {
                        this.OnDependency(evnt.Key, evnt.Value, currentActivity);
                    }

                    break;
            }
        }

        private void OnDependency(string name, object payload, Activity activity)
        {
            DependencyTelemetry telemetry = new DependencyTelemetry { Type = RemoteDependencyConstants.AzureServiceBus };

            this.SetCommonProperties(name, payload, activity, telemetry);
            
            // Endpoint is URL of particular ServiceBus, e.g. sb://myservicebus.servicebus.windows.net/
            string endpoint = this.FetchPayloadProperty<Uri>(name, EndpointPropertyName, payload)?.ToString();

            // Queue/Topic name, e.g. myqueue/mytopic
            string queueName = this.FetchPayloadProperty<string>(name, EntityPropertyName, payload);

            // Target uniquely identifies the resource, we use both: queueName and endpoint 
            // with schema used for SQL-dependencies
            telemetry.Target = string.Join(" | ", endpoint, queueName);
            this.TelemetryClient.TrackDependency(telemetry);
        }

        private void OnRequest(string name, object payload, Activity activity)
        {
            RequestTelemetry telemetry = new RequestTelemetry();

            foreach (var tag in activity.Tags)
            {
                if (tag.Key == "__legacyParentId")
                {
                    telemetry.Context.Operation.ParentId = tag.Value;
                    break;
                }
            }

            this.SetCommonProperties(name, payload, activity, telemetry);

            string endpoint = this.FetchPayloadProperty<Uri>(name, EndpointPropertyName, payload)?.ToString();

            // Queue/Topic name, e.g. myqueue/mytopic
            string queueName = this.FetchPayloadProperty<string>(name, EntityPropertyName, payload);

            // We want to make Source field extendable at the beginning as we may add
            // multi ikey support and also build some special UI for requests coming from the queues
            // it follows Request.Source schema invented for multi ikey
            telemetry.Source = string.Format(CultureInfo.InvariantCulture, "type:{0} | name:{1} | endpoint:{2}", RemoteDependencyConstants.AzureServiceBus, queueName, endpoint);

            this.TelemetryClient.TrackRequest(telemetry);
        }
    }
}
