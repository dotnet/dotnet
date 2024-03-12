namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;
    using Microsoft.ApplicationInsights.Extensibility.W3C;

    /// <summary>
    /// Operation class that holds the telemetry item and the corresponding telemetry client.
    /// </summary>
    internal class OperationHolder<T> : IOperationHolder<T> where T : OperationTelemetry
    {
        /// <summary>
        /// Parent context store that is used to restore call context.
        /// </summary>
        public OperationContextForCallContext ParentContext;

        private static readonly object LockObj = new object();

        private readonly TelemetryClient telemetryClient;

        private readonly object originalActivity = null;

        /// <summary>
        /// Indicates if this instance has been disposed of.
        /// </summary>
        private bool isDisposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationHolder{T}"/> class.
        /// Initializes telemetry client.
        /// </summary>
        /// <param name="telemetryClient">Initializes telemetry client object.</param>
        /// <param name="telemetry">Operation telemetry item that is assigned to the telemetry associated to the current operation item.</param>
        /// <param name="originalActivity">Original activity to restore after operation stops. Provide it if Activity created for the operation
        /// is detached from the scope it was created in because custom Ids were provided by user. Null indicates that Activity was not detached
        /// and no explicit restore is needed. It's passed around as object to allow ApplicationInsights.dll to be used in standalone mode
        /// for backward compatibility. </param>
        public OperationHolder(TelemetryClient telemetryClient, T telemetry, object originalActivity = null)
        {
            this.telemetryClient = telemetryClient ?? throw new ArgumentNullException(nameof(telemetryClient));
            this.Telemetry = telemetry ?? throw new ArgumentNullException(nameof(telemetry));
            this.originalActivity = originalActivity; // it's ok if it's null
        }

        /// <summary>
        /// Gets Telemetry item of interest that is created when StartOperation function of ClientExtensions is invoked.
        /// </summary>
        public T Telemetry { get; }

        /// <summary>
        /// Dispose method to clear the variables.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Computes the duration and tracks the respective telemetry item on dispose.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !this.isDisposed)
            {
                // We need to compare the operation id and name of telemetry with operation id and name of current call context before tracking it 
                // to make sure that the customer is tracking the right telemetry.
                lock (LockObj)
                {
                    if (!this.isDisposed)
                    {
                        var operationTelemetry = this.Telemetry;
                        operationTelemetry.Stop();

                        bool isActivityAvailable = false;
                        isActivityAvailable = ActivityExtensions.TryRun(() =>
                        {
                            var currentActivity = Activity.Current;
                            if (currentActivity == null 
                            || (Activity.DefaultIdFormat != ActivityIdFormat.W3C && operationTelemetry.Id != currentActivity.Id) 
                            || (Activity.DefaultIdFormat == ActivityIdFormat.W3C && operationTelemetry.Id != currentActivity.SpanId.ToHexString()))
                            {
                                // W3COperationCorrelationTelemetryInitializer changes Id
                                // but keeps an original one in 'ai_legacyRequestId' property

                                if (!operationTelemetry.Properties.TryGetValue("ai_legacyRequestId", out var legacyId) ||
                                    legacyId != currentActivity?.Id)
                                {
                                    // this is for internal error reporting
                                    CoreEventSource.Log.InvalidOperationToStopError();

                                    // this are details with unique ids for debugging
                                    CoreEventSource.Log.InvalidOperationToStopDetails(
                                        string.Format(
                                            CultureInfo.InvariantCulture,
                                            "Telemetry Id '{0}' does not match current Activity '{1}'",
                                            operationTelemetry.Id,
                                            currentActivity?.Id));

                                    return;
                                }
                            }

                            this.telemetryClient.Track(operationTelemetry);

                            currentActivity?.Stop();

                            if (this.originalActivity != null && 
                                Activity.Current != this.originalActivity && 
                                this.originalActivity is Activity activity)
                            {
                                Activity.Current = activity;
                            }
                        });

                        if (!isActivityAvailable)
                        {
                            var currentOperationContext = CallContextHelpers.GetCurrentOperationContext();
                            if (currentOperationContext == null || operationTelemetry.Id != currentOperationContext.ParentOperationId)
                            {
                                // this is for internal error reporting
                                CoreEventSource.Log.InvalidOperationToStopError();

                                // this are details with unique ids for debugging
                                CoreEventSource.Log.InvalidOperationToStopDetails(
                                    string.Format(
                                        CultureInfo.InvariantCulture,
                                        "Telemetry Id '{0}' does not match current Activity '{1}'",
                                        operationTelemetry.Id,
                                        currentOperationContext?.ParentOperationId));
                                return;
                            }

                            this.telemetryClient.Track(operationTelemetry);

                            CallContextHelpers.RestoreOperationContext(this.ParentContext);
                        }
                    }

                    this.isDisposed = true;
                }
            }
        }
    }
}
