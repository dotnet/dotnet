namespace Microsoft.ApplicationInsights.Web
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Web;

    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Common;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Web.Implementation;

    /// <summary>
    /// A telemetry initializer that will set the correlation context for all telemetry items in web application.
    /// </summary>
    public class OperationCorrelationTelemetryInitializer : WebTelemetryInitializerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OperationCorrelationTelemetryInitializer"/> class.
        /// </summary>
        public OperationCorrelationTelemetryInitializer()
        {
            ActivityHelpers.ParentOperationIdHeaderName = null;
            ActivityHelpers.RootOperationIdHeaderName = null;
        }

        /// <summary>
        /// Gets or sets the name of the header to get parent operation Id from.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Part of Public API and too late to change.")]
        public string ParentOperationIdHeaderName
        {
            get => ActivityHelpers.ParentOperationIdHeaderName;

            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    ActivityHelpers.ParentOperationIdHeaderName = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the name of the header to get root operation Id from.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Part of Public API and too late to change.")]
        public string RootOperationIdHeaderName
        {
            get => ActivityHelpers.RootOperationIdHeaderName;

            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    ActivityHelpers.RootOperationIdHeaderName = value;
                }
            }
        }

        /// <summary>
        /// Implements initialization logic.
        /// </summary>
        /// <param name="platformContext">Http context.</param>
        /// <param name="requestTelemetry">Request telemetry object associated with the current request.</param>
        /// <param name="telemetry">Telemetry item to initialize.</param>
        protected override void OnInitializeTelemetry(HttpContext platformContext, RequestTelemetry requestTelemetry, ITelemetry telemetry)
        {
            if (telemetry == null)
            {
                throw new ArgumentNullException(nameof(telemetry));
            }

            if (requestTelemetry == null)
            {
                throw new ArgumentNullException(nameof(requestTelemetry));
            }

            if (platformContext == null)
            {
                throw new ArgumentNullException(nameof(platformContext));
            }

            // Telemetry is initialized by Base SDK OperationCorrelationTelemetryInitializer from the call context /Current Activity
            // However we still may lose CorrelationContext/AsyncLocal due to IIS managed/native thread hops. 
            // We protect from it with PreRequestHandlerExecute event, that happens right before the handler
            // However some telemetry may be reported between BeginRequest and PreRequestHandlerExecute in the HttpModule pipeline
            // So this telemetry initializer works when:
            //   - telemetry was tracked before PreRequestHandlerExecute
            //   - AND execution context was lost in the HttpModule pipeline: base SDK OperationCorrelationTelemetryInitializer had nothing to initialize telemetry with
            //   - AND Telemetry was tracked from the thread that has HttpContext.Current (synchronously in HttpModule)
            //
            // In other cases, telemetry is not guaranteed to be properly initialized:
            // - if tracked in custom HttpModule
            // - AND in async method or after async method was invoked
            // - AND when the execution context is lost
            if (telemetry != requestTelemetry)
            {
                var operation = telemetry.Context.Operation;
                if (!string.IsNullOrEmpty(operation.Id))
                {
                    // telemetry is already initialized
                    return;
                }

                operation.Id = requestTelemetry.Context.Operation.Id;
                if (string.IsNullOrEmpty(operation.ParentId))
                {
                    operation.ParentId = requestTelemetry.Id;
                }

                var activity = platformContext.Items[ActivityHelpers.RequestActivityItemName] as Activity;
                if (activity == null)
                {
                    return;
                }

                var telemetryWithProperties = telemetry as ISupportProperties;
                if (telemetryWithProperties == null)
                {
                    return;
                }

                foreach (var item in activity.Baggage)
                {
                    if (!telemetryWithProperties.Properties.ContainsKey(item.Key))
                    {
                        telemetryWithProperties.Properties.Add(item);
                    }
                }
            }
        }
    }
}
