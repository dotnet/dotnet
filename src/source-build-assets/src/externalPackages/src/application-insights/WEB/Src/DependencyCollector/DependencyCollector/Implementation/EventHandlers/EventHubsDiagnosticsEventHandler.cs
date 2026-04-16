namespace Microsoft.ApplicationInsights.DependencyCollector.Implementation.EventHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;

    /// <summary>
    /// Implements EventHubs DiagnosticSource events handling.
    /// </summary>
    internal class EventHubsDiagnosticsEventHandler : DiagnosticsEventHandlerBase
    {
        public const string DiagnosticSourceName = "Microsoft.Azure.EventHubs";
        private const string EntityPropertyName = "Entity";
        private const string EndpointPropertyName = "Endpoint";

        internal EventHubsDiagnosticsEventHandler(TelemetryConfiguration configuration) : base(configuration)
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
                case "Microsoft.Azure.EventHubs.Send.Start":
                case "Microsoft.Azure.EventHubs.Receive.Start":
                case "Send.Start":
                case "Receive.Start":
                    break;
                case "Microsoft.Azure.EventHubs.Send.Stop":
                case "Microsoft.Azure.EventHubs.Receive.Stop":
                case "Send.Stop":
                case "Receive.Stop":
                    this.OnDependency(evnt.Key, evnt.Value, currentActivity);
                    break;
            }
        }

        private void OnDependency(string name, object payload, Activity activity)
        {
            DependencyTelemetry telemetry = new DependencyTelemetry { Type = RemoteDependencyConstants.AzureEventHubs };

            this.SetCommonProperties(name, payload, activity, telemetry);

            // Endpoint is URL of particular EventHub, e.g. sb://eventhubname.servicebus.windows.net/
            string endpoint = this.FetchPayloadProperty<Uri>(name, EndpointPropertyName, payload)?.ToString();

            // Queue/Topic name, e.g. myqueue/mytopic
            string queueName = this.FetchPayloadProperty<string>(name, EntityPropertyName, payload);

            // Target uniquely identifies the resource, we use both: queueName and endpoint 
            // with schema used for SQL-dependencies
            if (endpoint == null || queueName == null)
            {
                return;
            }

            // Target uniquely identifies the resource, we use both: queueName and endpoint 
            // with schema used for SQL-dependencies
            string separator = "/";
            if (endpoint.EndsWith(separator, StringComparison.Ordinal))
            {
                separator = string.Empty;
            }

            telemetry.Target = string.Concat(endpoint, separator, queueName);
            this.TelemetryClient.TrackDependency(telemetry);
        }
    }
}