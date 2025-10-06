namespace Microsoft.ApplicationInsights
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;
    using Microsoft.ApplicationInsights.Extensibility.W3C;

    /// <summary>
    /// Extension class to telemetry client that creates operation object with the respective fields initialized.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class TelemetryClientExtensions
    {
        private const string ChildActivityName = "Microsoft.ApplicationInsights.OperationContext";        

        /// <summary>
        /// Start operation creates an operation object with a respective telemetry item. 
        /// </summary>
        /// <typeparam name="T">Type of the telemetry item.</typeparam>
        /// <param name="telemetryClient">Telemetry client object.</param>
        /// <param name="operationName">Name of the operation that customer is planning to propagate.</param>
        /// <returns>Operation item object with a new telemetry item having current start time and timestamp.</returns>
        public static IOperationHolder<T> StartOperation<T>(this TelemetryClient telemetryClient, string operationName) where T : OperationTelemetry, new()
        {
            return StartOperation<T>(telemetryClient, operationName, operationId: null, parentOperationId: null);
        }

        /// <summary>
        /// Start operation creates an operation object with a respective telemetry item. 
        /// </summary>
        /// <typeparam name="T">Type of the telemetry item.</typeparam>
        /// <param name="telemetryClient">Telemetry client object.</param>
        /// <param name="operationName">Name of the operation that customer is planning to propagate.</param>
        /// <param name="operationId">Operation ID to set in the new operation.</param>
        /// <param name="parentOperationId">Optional parent operation ID to set in the new operation.</param>
        /// <returns>Operation item object with a new telemetry item having current start time and timestamp.</returns>
        public static IOperationHolder<T> StartOperation<T>(this TelemetryClient telemetryClient, string operationName, string operationId, string parentOperationId = null) where T : OperationTelemetry, new()
        {
            if (telemetryClient == null)
            {
                throw new ArgumentNullException(nameof(telemetryClient));
            }

            var operationTelemetry = new T();

            if (string.IsNullOrEmpty(operationTelemetry.Name) && !string.IsNullOrEmpty(operationName))
            {
                operationTelemetry.Name = operationName;
            }

            if (string.IsNullOrEmpty(operationTelemetry.Context.Operation.Id) && !string.IsNullOrEmpty(operationId))
            {
                var isActivityAvailable = ActivityExtensions.TryRun(() =>
                {
                    if (Activity.DefaultIdFormat == ActivityIdFormat.W3C)
                    {
                        if (W3CUtilities.IsCompatibleW3CTraceId(operationId))
                        {
                            // If the user provided operationid is W3C Compatible, use it.
                            operationTelemetry.Context.Operation.Id = operationId;
                        }
                        else
                        {
                            // If user provided operationid is not W3C compatible, generate a new one instead.
                            // and store supplied value inside customproperty.
                            operationTelemetry.Context.Operation.Id = ActivityTraceId.CreateRandom().ToHexString();
                            operationTelemetry.Properties.Add(W3CConstants.LegacyRootIdProperty, operationId);
                        }
                    }
                    else
                    {
                        operationTelemetry.Context.Operation.Id = operationId;
                    }
                });

                if (!isActivityAvailable)
                {
                    operationTelemetry.Context.Operation.Id = operationId;
                }
            }

            if (string.IsNullOrEmpty(operationTelemetry.Context.Operation.ParentId) && !string.IsNullOrEmpty(parentOperationId))
            {
                operationTelemetry.Context.Operation.ParentId = parentOperationId;
            }

            return StartOperation(telemetryClient, operationTelemetry);
        }

        /// <summary>
        /// Creates an operation object with a given telemetry item. 
        /// </summary>
        /// <typeparam name="T">Type of the telemetry item.</typeparam>
        /// <param name="telemetryClient">Telemetry client object.</param>
        /// <param name="operationTelemetry">Operation to start.</param>
        /// <returns>Operation item object with a new telemetry item having current start time and timestamp.</returns>
        public static IOperationHolder<T> StartOperation<T>(this TelemetryClient telemetryClient, T operationTelemetry) where T : OperationTelemetry
        {
            if (telemetryClient == null)
            {
                throw new ArgumentNullException(nameof(telemetryClient));
            }

            if (operationTelemetry == null)
            {
                throw new ArgumentNullException(nameof(operationTelemetry));
            }

            var telemetryContext = operationTelemetry.Context.Operation;
            bool idsAssignedByUser = !string.IsNullOrEmpty(telemetryContext.Id);

            // We initialize telemetry here AND in Track method because of RichPayloadEventSource.
            // It sends Start and Stop events for OperationTelemetry. During Start event telemetry
            // has to contain essential telemetry properties such as correlations ids and ikey.
            // Also, examples in our documentation rely on the fact that correlation Ids are set
            // after StartOperation call and before operation is stopped.
            // Before removing this call (for optimization), make sure:
            // 1) correlation ids are set before method leaves
            // 2) RichPayloadEventSource is re-factored to work without ikey in Start event (or ikey is set)
            //    and does not require other properties in telemetry
            telemetryClient.Initialize(operationTelemetry);

            // Initialize operation id if it wasn't initialized by telemetry initializers
            if (string.IsNullOrEmpty(operationTelemetry.Id))
            {
                operationTelemetry.GenerateOperationId();
            }

            // If the operation is not executing in the context of any other operation
            // set its name as a context (root) operation name.
            if (string.IsNullOrEmpty(telemetryContext.Name))
            {
                telemetryContext.Name = operationTelemetry.Name;
            }

            OperationHolder<T> operationHolder = null;

            var isActivityAvailable = ActivityExtensions.TryRun(() =>
            {
                var parentActivity = Activity.Current;
                var operationActivity = new Activity(ChildActivityName);

                string operationName = telemetryContext.Name;
                if (string.IsNullOrEmpty(operationName))
                {
                    operationName = parentActivity?.GetOperationName();
                }

                if (!string.IsNullOrEmpty(operationName))
                {
                    operationActivity.SetOperationName(operationName);
                }

                if (idsAssignedByUser)
                {
                    if (Activity.DefaultIdFormat == ActivityIdFormat.W3C)
                    {
                        if (W3CUtilities.IsCompatibleW3CTraceId(telemetryContext.Id))
                        {
                            // If the user provided operationId is W3C Compatible, use it.
                            operationActivity.SetParentId(ActivityTraceId.CreateFromString(telemetryContext.Id.AsSpan()),
                                default(ActivitySpanId), ActivityTraceFlags.None);
                        }
                        else
                        {
                            // If user provided operationId is not W3C compatible, generate a new one instead.
                            // and store supplied value inside custom property.
                            operationTelemetry.Properties.Add(W3CConstants.LegacyRootIdProperty, telemetryContext.Id);
                            telemetryContext.Id = null;
                        }
                    }
                    else
                    {
                        operationActivity.SetParentId(telemetryContext.Id);
                    }
                }

                operationActivity.Start();

                if (operationActivity.IdFormat == ActivityIdFormat.W3C)
                {
                    if (string.IsNullOrEmpty(telemetryContext.Id))
                    {
                        telemetryContext.Id = operationActivity.TraceId.ToHexString();
                    }

                    operationTelemetry.Id = operationActivity.SpanId.ToHexString();
                }
                else
                {
                    if (string.IsNullOrEmpty(telemetryContext.Id))
                    {
                        telemetryContext.Id = operationActivity.RootId;
                    }

                    operationTelemetry.Id = operationActivity.Id;
                }

                operationHolder = new OperationHolder<T>(telemetryClient, operationTelemetry, parentActivity == operationActivity.Parent ? null : parentActivity);
            });

            if (!isActivityAvailable)
            {
                // Parent context store is assigned to operation that is used to restore call context.
                operationHolder = new OperationHolder<T>(telemetryClient, operationTelemetry)
                {
                    ParentContext = CallContextHelpers.GetCurrentOperationContext(),
                };
                telemetryContext.Id = operationTelemetry.Id;
            }

            operationTelemetry.Start();

            if (!isActivityAvailable)
            {
                // Update the call context to store certain fields that can be used for subsequent operations.
                var operationContext = new OperationContextForCallContext
                {
                    ParentOperationId = operationTelemetry.Id,
                    RootOperationId = operationTelemetry.Context.Operation.Id,
                    RootOperationName = operationTelemetry.Context.Operation.Name,
                };
                CallContextHelpers.SaveOperationContext(operationContext);
            }

            return operationHolder;
        }

        /// <summary>
        /// Stop operation computes the duration of the operation and tracks it using the respective telemetry client.
        /// </summary>
        /// <param name="telemetryClient">Telemetry client object.</param>
        /// <param name="operation">Operation object to compute duration and track.</param>
        public static void StopOperation<T>(this TelemetryClient telemetryClient, IOperationHolder<T> operation) where T : OperationTelemetry
        {
            if (telemetryClient == null)
            {
                throw new ArgumentNullException(nameof(telemetryClient));
            }

            if (operation == null)
            {
                CoreEventSource.Log.OperationIsNullWarning();
                return;
            }

            operation.Dispose();
        }

        /// <summary>
        /// Creates an operation object with a respective telemetry item using <see cref="Activity"/> instance that holds tracing context. 
        /// </summary>
        /// <example>
        /// <code>
        ///   // receive message from a queue service (or any kind of the request/message from external service)
        ///   var message = queue.Receive();
        /// 
        ///   // Extract tracing context from the message before processing it
        ///   // Note that some protocols may define how Activity should be serialized into the message,
        ///   // and some client SDKs implementing them may provide Extract method.
        ///   // For other protocols/libraries, serialization has to be agreed between producer and consumer
        ///   // and Inject/Extract pattern to be implemented
        ///   var activity = message.ExtractActivity();
        /// 
        ///   // Track processing of the message
        ///   using (telemetryClient.StartOperation&lt;RequestTelemetry&gt;(activity))
        ///   {
        ///     // process message
        ///   }
        ///  // telemetry is reported when operation is disposed.
        /// </code>
        /// </example>
        /// <remarks><para>Activity represents tracing context; it contains correlation identifiers and extended properties that are propagated to external calls.
        /// See <a href="https://github.com/dotnet/corefx/blob/master/src/System.Diagnostics.DiagnosticSource/src/ActivityUserGuide.md"/> for more details.</para>
        /// <para>When Activity instance is passed to StartOperation, it is expected that Activity has ParentId (if it was provided by upstream service), but has not been started yet.
        /// It may also have additional Tags and Baggage to augment telemetry.</para>
        /// </remarks>
        /// <typeparam name="T">Type of the telemetry item.</typeparam>
        /// <param name="telemetryClient">Telemetry client object.</param>
        /// <param name="activity">Activity to get tracing context and telemetry properties from.</param>
        /// <returns>Operation item object with a new telemetry item having current start time and timestamp.</returns>
        public static IOperationHolder<T> StartOperation<T>(this TelemetryClient telemetryClient, Activity activity) where T : OperationTelemetry, new()
        {
            if (telemetryClient == null)
            {
                throw new ArgumentNullException(nameof(telemetryClient));
            }

            if (activity == null)
            {
                throw new ArgumentNullException(nameof(activity));
            }

            Activity originalActivity = null;
            
            // not started activity, default case
            if (activity.Id == null)
            {
                originalActivity = Activity.Current;
            }

            string legacyRoot = null;
            string legacyParent = null;
            if (Activity.DefaultIdFormat == ActivityIdFormat.W3C)
            {
                // if parent is not  W3C
                if (activity.ParentId != null &&
                    !activity.ParentId.StartsWith("00-", StringComparison.Ordinal))
                {
                    // save parent
                    legacyParent = activity.ParentId;

                    if (W3CUtilities.IsCompatibleW3CTraceId(activity.RootId))
                    {
                        // reuse root id when compatible with trace ID
                        activity = CopyFromCompatibleRoot(activity);
                    }
                    else
                    {
                        // or store legacy root in custom property
                        legacyRoot = activity.RootId;
                    }
                }
            }

            activity.Start();
            T operationTelemetry = ActivityToTelemetry<T>(activity);

            if (legacyRoot != null)
            {
                operationTelemetry.Properties.Add(W3CConstants.LegacyRootIdProperty, legacyRoot);
            }

            if (legacyParent != null)
            {
                operationTelemetry.Context.Operation.ParentId = legacyParent;
            }

            // We initialize telemetry here AND in Track method because of RichPayloadEventSource.
            // It sends Start and Stop events for OperationTelemetry. During Start event telemetry
            // has to contain essential telemetry properties such as correlations ids and ikey.
            // Also, examples in our documentation rely on the fact that correlation Ids are set
            // after StartOperation call and before operation is stopped.
            // Before removing this call (for optimization), make sure:
            // 1) correlation ids are set before method leaves
            // 2) RichPayloadEventSource is re-factored to work without ikey in Start event (or ikey is set)
            //    and does not require other properties in telemetry
            telemetryClient.Initialize(operationTelemetry);

            operationTelemetry.Start();

            return new OperationHolder<T>(telemetryClient, operationTelemetry, originalActivity);
        }

        private static T ActivityToTelemetry<T>(Activity activity) where T : OperationTelemetry, new()
        {
            Debug.Assert(activity.Id != null, "Activity must be started prior calling this method");

            var telemetry = new T { Name = activity.OperationName };

            OperationContext operationContext = telemetry.Context.Operation;
            operationContext.Name = activity.GetOperationName();            
            
            if (activity.IdFormat == ActivityIdFormat.W3C)
            {
                operationContext.Id = activity.TraceId.ToHexString();
                telemetry.Id = activity.SpanId.ToHexString();

                if (string.IsNullOrEmpty(operationContext.ParentId) && activity.ParentSpanId != default)
                {
                    operationContext.ParentId = activity.ParentSpanId.ToHexString();
                }
            }
            else
            {
                operationContext.Id = activity.RootId;
                operationContext.ParentId = activity.ParentId;
                telemetry.Id = activity.Id;
            }

            foreach (var item in activity.Baggage)
            {
                if (!telemetry.Properties.ContainsKey(item.Key))
                {
                    telemetry.Properties.Add(item);
                }
            }

            foreach (var item in activity.Tags)
            {
                if (!telemetry.Properties.ContainsKey(item.Key))
                {
                    telemetry.Properties.Add(item);
                }
            }

            return telemetry;
        }

        private static Activity CopyFromCompatibleRoot(Activity from)
        {
            var copy = new Activity(from.OperationName);
            copy.SetParentId(ActivityTraceId.CreateFromString(from.RootId.AsSpan()),
                default(ActivitySpanId), from.ActivityTraceFlags);

            foreach (var tag in from.Tags)
            {
                copy.AddTag(tag.Key, tag.Value);
            }

            foreach (var baggage in from.Baggage)
            {
                copy.AddBaggage(baggage.Key, baggage.Value);
            }

            copy.TraceStateString = from.TraceStateString;

            return copy;
        }
    }
}
