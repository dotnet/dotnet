namespace Microsoft.ApplicationInsights.DependencyCollector.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Microsoft.ApplicationInsights.Common;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;

    internal class TelemetryDiagnosticSourceListener : DiagnosticSourceListenerBase<HashSet<string>>, IDiagnosticEventHandler
    {
        internal const string ActivityStartNameSuffix = ".Start";
        internal const string ActivityStopNameSuffix = ".Stop";

        private readonly HashSet<string> includedDiagnosticSources 
            = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        private readonly Dictionary<string, HashSet<string>> includedDiagnosticSourceActivities 
            = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);

        private readonly Dictionary<string, IDiagnosticEventHandler> customEventHandlers = new Dictionary<string, IDiagnosticEventHandler>(StringComparer.OrdinalIgnoreCase);

        public TelemetryDiagnosticSourceListener(TelemetryConfiguration configuration, ICollection<string> includeDiagnosticSourceActivities) 
            : base(configuration)
        {
            this.Client.Context.GetInternalContext().SdkVersion = SdkVersionUtils.GetSdkVersion("rdd" + RddSource.DiagnosticSourceListener + ":");
            this.PrepareInclusionLists(includeDiagnosticSourceActivities);
        }

        public bool IsEventEnabled(string evnt, object input1, object input2)
        {
            return !evnt.EndsWith(ActivityStartNameSuffix, StringComparison.Ordinal);
        }

        public void OnEvent(KeyValuePair<string, object> evnt, DiagnosticListener diagnosticListener)
        {
            if (!evnt.Key.EndsWith(ActivityStopNameSuffix, StringComparison.Ordinal))
            {
                return;
            }

            Activity currentActivity = Activity.Current;

            // extensibility point - can chain more telemetry extraction methods here
            var telemetry = ExtractDependencyTelemetry(diagnosticListener, currentActivity);
            if (telemetry == null)
            {
                return;
            }

            // properly fill dependency telemetry operation context
            if (currentActivity.IdFormat == ActivityIdFormat.W3C)
            {
                telemetry.Context.Operation.Id = currentActivity.TraceId.ToHexString();
                if (currentActivity.ParentSpanId != default)
                {
                    telemetry.Context.Operation.ParentId = currentActivity.ParentSpanId.ToHexString();
                }

                telemetry.Id = currentActivity.SpanId.ToHexString();
            }
            else
            {
                telemetry.Id = currentActivity.Id;
                telemetry.Context.Operation.Id = currentActivity.RootId;
                telemetry.Context.Operation.ParentId = currentActivity.ParentId;
            }

            telemetry.Timestamp = currentActivity.StartTimeUtc;

            telemetry.Properties["DiagnosticSource"] = diagnosticListener.Name;
            telemetry.Properties["Activity"] = currentActivity.OperationName;

            this.Client.TrackDependency(telemetry);
        }

        internal static DependencyTelemetry ExtractDependencyTelemetry(DiagnosticListener diagnosticListener, Activity currentActivity)
        {
            DependencyTelemetry telemetry = new DependencyTelemetry
            {
                Id = currentActivity.Id,
                Duration = currentActivity.Duration,
                Name = currentActivity.OperationName,
            };

            Uri requestUri = null;
            string component = null;
            string queryStatement = null;
            string httpUrl = null;
            string peerAddress = null;
            string peerService = null;

            foreach (KeyValuePair<string, string> tag in currentActivity.Tags)
            {
                // interpret Tags as defined by OpenTracing conventions
                // https://github.com/opentracing/specification/blob/master/semantic_conventions.md
                switch (tag.Key)
                {
                    case "component":
                        {
                            component = tag.Value;
                            break;
                        }

                    case "db.statement":
                        {
                            queryStatement = tag.Value;
                            break;
                        }

                    case "error":
                        {
                            if (bool.TryParse(tag.Value, out var failed))
                            {
                                telemetry.Success = !failed;
                                continue; // skip Properties
                            }

                            break;
                        }

                    case "http.status_code":
                        {
                            telemetry.ResultCode = tag.Value;
                            continue; // skip Properties
                        }

                    case "http.method":
                        {
                            continue; // skip Properties
                        }

                    case "http.url":
                        {
                            httpUrl = tag.Value;
                            if (Uri.TryCreate(tag.Value, UriKind.RelativeOrAbsolute, out requestUri))
                            {
                                continue; // skip Properties
                            }

                            break;
                        }

                    case "peer.address":
                        {
                            peerAddress = tag.Value;
                            break;
                        }

                    case "peer.hostname":
                        {
                            telemetry.Target = tag.Value;
                            continue; // skip Properties
                        }

                    case "peer.service":
                        {
                            peerService = tag.Value;
                            break;
                        }
                }

                // if more than one tag with the same name is specified, the first one wins
                // TODO verify if still needed once https://github.com/Microsoft/ApplicationInsights-dotnet/issues/562 is resolved 
                if (!telemetry.Properties.ContainsKey(tag.Key))
                {
                    telemetry.Properties.Add(tag);
                }
            }

            if (string.IsNullOrEmpty(telemetry.Type))
            {
                telemetry.Type = peerService ?? component ?? diagnosticListener.Name;
            }

            if (string.IsNullOrEmpty(telemetry.Target))
            {
                // 'peer.address' can be not user-friendly, thus use only if nothing else specified
                telemetry.Target = requestUri?.Host ?? peerAddress;
            }

            if (string.IsNullOrEmpty(telemetry.Name))
            {
                telemetry.Name = currentActivity.OperationName;
            }

            if (string.IsNullOrEmpty(telemetry.Data))
            {
                telemetry.Data = queryStatement ?? requestUri?.OriginalString ?? httpUrl;
            }

            return telemetry;
        }

        internal void RegisterHandler(string diagnosticSourceName, IDiagnosticEventHandler eventHandler)
        {
            this.customEventHandlers[diagnosticSourceName] = eventHandler;
        }

        internal override bool IsSourceEnabled(DiagnosticListener value)
        {
            return this.includedDiagnosticSources.Contains(value.Name);
        }

        internal override bool IsActivityEnabled(string activityName, HashSet<string> includedActivities)
        {
            // if no list of included activities then all are included
            return includedActivities == null || includedActivities.Contains(activityName);
        }

        protected override HashSet<string> GetListenerContext(DiagnosticListener diagnosticListener)
        {
            if (!this.includedDiagnosticSourceActivities.TryGetValue(diagnosticListener.Name, out var includedActivities))
            {
                return null;
            }

            return includedActivities;
        }

        protected override IDiagnosticEventHandler GetEventHandler(string diagnosticListenerName)
        {
            if (this.customEventHandlers.TryGetValue(diagnosticListenerName, out var eventHandler))
            {
                return eventHandler;
            }

            return this;
        }

        private void PrepareInclusionLists(ICollection<string> includeDiagnosticSourceActivities)
        {
            if (includeDiagnosticSourceActivities == null)
            {
                return;
            }

            foreach (string inclusion in includeDiagnosticSourceActivities)
            {
                if (string.IsNullOrWhiteSpace(inclusion))
                {
                    continue;
                }

                // each individual inclusion can specify
                // 1) the name of Diagnostic Source 
                //    - in that case the whole source is included
                //    - e.g. "System.Net.Http"
                // 2) the names of Diagnostic Source and Activity separated by ':' 
                //   - in that case only the activity is enabled from given source
                //   - e.g. ""
                string[] tokens = inclusion.Split(':');

                // the Diagnostic Source is included (even if only certain activities are enabled)
                this.includedDiagnosticSources.Add(tokens[0]);

                if (tokens.Length > 1)
                {
                    // only certain Activity from the Diagnostic Source is included
                    if (!this.includedDiagnosticSourceActivities.TryGetValue(tokens[0], out var includedActivities))
                    {
                        includedActivities = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                        this.includedDiagnosticSourceActivities[tokens[0]] = includedActivities;
                    }

                    // include activity and activity Stop events
                    includedActivities.Add(tokens[1]);
                    includedActivities.Add(tokens[1] + ActivityStopNameSuffix);
                }
            }
        }
    }
}