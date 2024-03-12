namespace Microsoft.ApplicationInsights.Extensibility
{
    using System;
    using System.Diagnostics;

    using Microsoft.ApplicationInsights;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;

    /// <summary>
    /// Telemetry initializer that populates OperationContext for the telemetry item from Activity.
    /// This initializer is responsible for correlation of telemetry items within the same process.
    /// </summary>
    public class OperationCorrelationTelemetryInitializer : ITelemetryInitializer
    {
        private const string TracestatePropertyKey = "tracestate";

        /// <summary>
        /// Initializes/Adds operation context to the existing telemetry item.
        /// </summary>
        /// <param name="telemetryItem">Target telemetry item to add operation context.</param>
        public void Initialize(ITelemetry telemetryItem)
        {
            if (telemetryItem == null)
            {
                throw new ArgumentNullException(nameof(telemetryItem));
            }

            var itemOperationContext = telemetryItem.Context.Operation;
            var telemetryProp = telemetryItem as ISupportProperties;            

            bool isActivityAvailable = false;
            isActivityAvailable = ActivityExtensions.TryRun(() =>
            {
                var currentActivity = Activity.Current;
                if (currentActivity != null)
                {
                    // we are going to set tracestate property on requests and dependencies only
                    if (currentActivity.IdFormat == ActivityIdFormat.W3C &&
                        !string.IsNullOrEmpty(currentActivity.TraceStateString) &&
                        telemetryItem is OperationTelemetry &&
                        telemetryProp != null &&
                        !telemetryProp.Properties.ContainsKey(TracestatePropertyKey))
                    {
                        telemetryProp.Properties.Add(TracestatePropertyKey, currentActivity.TraceStateString);
                    }

                    // update proactive sampling decision if Activity is recorded
                    // sampling processor may change the decision
                    if (currentActivity.Recorded &&
                        telemetryItem is ISupportAdvancedSampling supportSamplingTelemetry &&
                        supportSamplingTelemetry.ProactiveSamplingDecision == SamplingDecision.None)
                    {
                        supportSamplingTelemetry.ProactiveSamplingDecision = SamplingDecision.SampledIn;
                    }

                    if (string.IsNullOrEmpty(itemOperationContext.Id))
                    {
                        if (currentActivity.IdFormat == ActivityIdFormat.W3C)
                        {
                            // Set OperationID to Activity.TraceId
                            // itemOperationContext.Id = currentActivity.RootId; // check if this can be used
                            itemOperationContext.Id = currentActivity.TraceId.ToHexString();

                            if (string.IsNullOrEmpty(itemOperationContext.ParentId))
                            {
                                itemOperationContext.ParentId = currentActivity.SpanId.ToHexString();
                            }
                        }
                        else
                        {
                            itemOperationContext.Id = currentActivity.RootId;

                            if (string.IsNullOrEmpty(itemOperationContext.ParentId))
                            {
                                itemOperationContext.ParentId = currentActivity.Id;
                            }
                        }

                        foreach (var baggage in currentActivity.Baggage)
                        {
                            if (telemetryProp != null && !telemetryProp.Properties.ContainsKey(baggage.Key))
                            {
                                telemetryProp.Properties.Add(baggage);
                            }
                        }

                        if (string.IsNullOrEmpty(itemOperationContext.Name))
                        {
                            string operationName = currentActivity.GetOperationName();
                            if (!string.IsNullOrEmpty(operationName))
                            {
                                itemOperationContext.Name = operationName;
                            }
                        }
                    }
                }
            });

            if (!isActivityAvailable)
            {
                if (string.IsNullOrEmpty(itemOperationContext.ParentId) || string.IsNullOrEmpty(itemOperationContext.Id) || string.IsNullOrEmpty(itemOperationContext.Name))
                {
                    var parentContext = CallContextHelpers.GetCurrentOperationContext();
                    if (parentContext != null)
                    {
                        if (string.IsNullOrEmpty(itemOperationContext.ParentId)
                            && !string.IsNullOrEmpty(parentContext.ParentOperationId))
                        {
                            itemOperationContext.ParentId = parentContext.ParentOperationId;
                        }

                        if (string.IsNullOrEmpty(itemOperationContext.Id)
                            && !string.IsNullOrEmpty(parentContext.RootOperationId))
                        {
                            itemOperationContext.Id = parentContext.RootOperationId;
                        }

                        if (string.IsNullOrEmpty(itemOperationContext.Name)
                            && !string.IsNullOrEmpty(parentContext.RootOperationName))
                        {
                            itemOperationContext.Name = parentContext.RootOperationName;
                        }

                        if (parentContext.CorrelationContext != null)
                        {
                            foreach (var item in parentContext.CorrelationContext)
                            {
                                if (telemetryProp != null && !telemetryProp.Properties.ContainsKey(item.Key))
                                {
                                    telemetryProp.Properties.Add(item);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
