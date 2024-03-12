namespace Microsoft.ApplicationInsights.DependencyCollector.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using Microsoft.ApplicationInsights.Common;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.DependencyCollector.Implementation.EventHandlers;
    using Microsoft.ApplicationInsights.DependencyCollector.Implementation.Operation;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;

    internal class AzureSdkDiagnosticsEventHandler : DiagnosticsEventHandlerBase
    {
#if NET452
        private static readonly DateTimeOffset EpochStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
#endif

        private readonly ObjectInstanceBasedOperationHolder<OperationTelemetry> operationHolder = new ObjectInstanceBasedOperationHolder<OperationTelemetry>();

        // fetchers must not be reused between sources
        // fetcher is created per AzureSdkDiagnosticsEventHandler and AzureSdkDiagnosticsEventHandler is created per DiagnosticSource
        private readonly PropertyFetcher linksPropertyFetcher = new PropertyFetcher("Links");

        public AzureSdkDiagnosticsEventHandler(TelemetryClient client) : base(client)
        {
        }

        public override bool IsEventEnabled(string evnt, object arg1, object arg2)
        {
            return true;
        }

        public override void OnEvent(KeyValuePair<string, object> evnt, DiagnosticListener diagnosticListener)
        {
            try
            {
                if (SdkInternalOperationsMonitor.IsEntered())
                {
                    // Because we support AAD, we must to check if an internal operation is being caught here (type = "InProc | Microsoft.AAD").
                    return;
                }

                var currentActivity = Activity.Current;
                if (evnt.Key.EndsWith(".Start", StringComparison.Ordinal))
                {
                    OperationTelemetry telemetry = null;

                    foreach (var tag in currentActivity.Tags)
                    {
                        if (tag.Key == "kind" && (tag.Value == "server" || tag.Value == "consumer"))
                        {
                            telemetry = new RequestTelemetry();
                            break;
                        }
                    }

                    string type = GetType(currentActivity);

                    if (telemetry == null)
                    {
                        telemetry = new DependencyTelemetry { Type = type };
                    }

                    if (IsMessagingDependency(type))
                    {
                        SetMessagingProperties(currentActivity, telemetry);
                    }

                    if (this.linksPropertyFetcher.Fetch(evnt.Value) is IEnumerable<Activity> activityLinks)
                    {
                        PopulateLinks(activityLinks, telemetry);

                        if (telemetry is RequestTelemetry request &&
                            TryGetAverageTimeInQueueForBatch(activityLinks, currentActivity.StartTimeUtc, out long enqueuedTime))
                        {
                            request.Metrics["timeSinceEnqueued"] = enqueuedTime;
                        }
                    }

                    this.operationHolder.Store(currentActivity, Tuple.Create(telemetry, /* isCustomCreated: */ false));
                }
                else if (evnt.Key.EndsWith(".Stop", StringComparison.Ordinal))
                {
                    var telemetry = this.operationHolder.Get(currentActivity).Item1;

                    this.SetCommonProperties(evnt.Key, evnt.Value, currentActivity, telemetry);

                    if (telemetry is DependencyTelemetry dependency && dependency.Type == RemoteDependencyConstants.HTTP)
                    {
                        SetHttpProperties(currentActivity, dependency);
                        if (evnt.Value != null)
                        {
                            dependency.SetOperationDetail(evnt.Value.GetType().FullName, evnt.Value);
                        }
                    }

                    this.TelemetryClient.Track(telemetry);
                }
                else if (evnt.Key.EndsWith(".Exception", StringComparison.Ordinal))
                {
                    Exception ex = evnt.Value as Exception;

                    var telemetry = this.operationHolder.Get(currentActivity);
                    telemetry.Item1.Success = false;
                    if (ex != null)
                    {
                        telemetry.Item1.Properties[RemoteDependencyConstants.DependencyErrorPropertyKey] = ex.ToInvariantString();
                    }
                }
            }
            catch (Exception ex)
            {
                DependencyCollectorEventSource.Log.TelemetryDiagnosticSourceCallbackException(evnt.Key, ex.ToInvariantString());
            }
        }

        protected override void PopulateTags(Activity activity, OperationTelemetry telemetry)
        {
        }

        protected override string GetOperationName(string eventName, object eventPayload, Activity activity)
        {
            // activity name looks like 'Azure.<...>.<Class>.<Name>'
            // as namespace is too verbose, we'll just take the last two nodes from the activity name as telemetry name
            string activityName = activity.OperationName;
            int methodDotIndex = activityName.LastIndexOf('.');
            if (methodDotIndex <= 0)
            {
                return activityName;
            }

            int classDotIndex = activityName.LastIndexOf('.', methodDotIndex - 1);

            if (classDotIndex == -1)
            {
                return activityName;
            }

            return activityName.Substring(classDotIndex + 1, activityName.Length - classDotIndex - 1);
        }

        protected override bool IsOperationSuccessful(string eventName, object eventPayload, Activity activity)
        {
            return true;
        }

        private static bool TryGetAverageTimeInQueueForBatch(IEnumerable<Activity> links, DateTimeOffset requestStartTime, out long avgTimeInQueue)
        {
            avgTimeInQueue = 0;
            int linksCount = 0;
            foreach (var link in links)
            {
                if (!TryGetEnqueuedTime(link, out var msgEnqueuedTime))
                {
                    // instrumentation does not consistently report enqueued time, ignoring whole span
                    return false;
                }

                long startEpochTime = 0;
#if NET452
                startEpochTime = (long)(requestStartTime - EpochStart).TotalMilliseconds;
#else
                startEpochTime = requestStartTime.ToUnixTimeMilliseconds();
#endif
                avgTimeInQueue += Math.Max(startEpochTime - msgEnqueuedTime, 0);
                linksCount++;
            }

            if (linksCount == 0)
            {
                return false;
            }

            avgTimeInQueue /= linksCount;
            return true;
        }

        private static bool TryGetEnqueuedTime(Activity link, out long enqueuedTime)
        {
            enqueuedTime = 0;
            foreach (var attribute in link.Tags)
            {
                if (attribute.Key == "enqueuedTime")
                {
                    if (attribute.Value is string strValue)
                    {
                        return long.TryParse(strValue, out enqueuedTime);
                    }
                }
            }

            return false;
        }

        private static string GetType(Activity currentActivity)
        {
            string kind = RemoteDependencyConstants.InProc;
            string component = null;
            foreach (var tag in currentActivity.Tags)
            {
                if (tag.Key.StartsWith("http.", StringComparison.Ordinal))
                {
                    return RemoteDependencyConstants.HTTP;
                }

                switch (tag.Key)
                {
                    case "kind":
                        switch (tag.Value)
                        {
                            case "internal":
                                break;
                            case "producer":
                                kind = RemoteDependencyConstants.QueueMessage;
                                break;
                            default:
                                kind = null;
                                break;
                        }

                        break;

                    case "component":
                        // old tag populated for back-compat, if az.namespace is set - ignore it.
                        if (component == null)
                        {
                            component = tag.Value;
                        }

                        break;
                    case "az.namespace":
                        component = tag.Value;
                        break;
                }
            }

            if (component == "eventhubs" || component == "Microsoft.EventHub")
            {
                component = RemoteDependencyConstants.AzureEventHubs;
            } 
            else if (component == "Microsoft.ServiceBus")
            {
                component = RemoteDependencyConstants.AzureServiceBus;
            }

            if (component != null)
            {
                return kind == null
                    ? component
                    : string.Concat(kind, " | ", component);
            }

            return kind ?? string.Empty;
        }

        private static void SetHttpProperties(Activity activity, DependencyTelemetry dependency)
        {
            string method = null;
            string url = null;
            string status = null;
            bool failed = false;
            bool hasExplicitStatus = false;

            foreach (var tag in activity.Tags)
            {
                if (tag.Key == "http.url")
                {
                    url = tag.Value;
                }
                else if (tag.Key == "http.method")
                {
                    method = tag.Value;
                }
                else if (tag.Key == "requestId")
                {
                    dependency.Properties["ClientRequestId"] = tag.Value;
                }
                else if (tag.Key == "serviceRequestId")
                {
                    dependency.Properties["ServerRequestId"] = tag.Value;
                }
                else if (tag.Key == "http.status_code")
                {
                    status = tag.Value;
                }
                else if (tag.Key == "otel.status_code")
                {
                    hasExplicitStatus = true;
                    failed = string.Equals(tag.Value, "ERROR", StringComparison.OrdinalIgnoreCase);
                }
            }

            // TODO: could be optimized to avoid full URI parsing and allocation
            if (url == null || !Uri.TryCreate(url, UriKind.Absolute, out var parsedUrl))
            {
                DependencyCollectorEventSource.Log.FailedToParseUrl(url);
                return;
            }

            dependency.Name = string.Concat(method, " ", parsedUrl.AbsolutePath);
            dependency.Data = url;
            dependency.Target = DependencyTargetNameHelper.GetDependencyTargetName(parsedUrl);
            dependency.ResultCode = status;

            if (!hasExplicitStatus)
            {
                if (int.TryParse(status, out var statusCode))
                {
                    dependency.Success = (statusCode > 0) && (statusCode < 400);
                }
            }
            else if (failed)
            {
                dependency.Success = false;
            }
        }

        private static bool IsMessagingDependency(string dependencyType)
        {
            return dependencyType != null && (dependencyType.EndsWith(RemoteDependencyConstants.AzureEventHubs, StringComparison.Ordinal) ||
                         dependencyType.EndsWith(RemoteDependencyConstants.AzureServiceBus, StringComparison.Ordinal));
        }

        private static void SetMessagingProperties(Activity activity, OperationTelemetry telemetry)
        {
            string endpoint = null;
            string entityName = null;

            foreach (var tag in activity.Tags)
            {
                if (tag.Key == "peer.address")
                {
                    endpoint = tag.Value;
                }
                else if (tag.Key == "message_bus.destination")
                {
                    entityName = tag.Value;
                }
            }

            if (endpoint == null || entityName == null)
            {
                return;
            }

            // Target uniquely identifies the resource, we use both: entityName and endpoint
            // with schema used for SQL-dependencies
            string separator = "/";
            if (endpoint.EndsWith(separator, StringComparison.Ordinal))
            {
                separator = string.Empty;
            }

            string brokerInfo = string.Concat(endpoint, separator, entityName);

            if (telemetry is DependencyTelemetry dependency)
            {
                dependency.Target = brokerInfo;
            }
            else if (telemetry is RequestTelemetry request)
            {
                request.Source = brokerInfo;
            }
        }

        private static void PopulateLinks(IEnumerable<Activity> links, OperationTelemetry telemetry)
        {
            if (links.Any())
            {
                var linksJson = new StringBuilder();
                linksJson.Append('[');
                foreach (var link in links)
                {
                    var linkTraceId = link.TraceId.ToHexString();

                    // avoiding json serializers for now because of extra dependency.
                    // serialization is trivial and looks like `links` property with json blob
                    // [{"operation_Id":"5eca8b153632494ba00f619d6877b134","id":"d4c1279b6e7b7c47"},
                    //  {"operation_Id":"ff28988d0776b44f9ca93352da126047","id":"bf4fa4855d161141"}]
                    linksJson
                        .Append('{')
                        .Append("\"operation_Id\":")
                        .Append('\"')
                        .Append(linkTraceId)
                        .Append('\"')
                        .Append(',');
                    linksJson
                        .Append("\"id\":")
                        .Append('\"')
                        .Append(link.ParentSpanId.ToHexString())
                        .Append('\"');

                    // we explicitly ignore sampling flag, tracestate and attributes at this point.
                    linksJson.Append("},");
                }

                if (linksJson.Length > 0)
                {
                    // trim trailing comma - json does not support it
                    linksJson.Remove(linksJson.Length - 1, 1);
                }

                linksJson.Append(']');
                telemetry.Properties["_MS.links"] = linksJson.ToString();
            }
        }
    }
}
