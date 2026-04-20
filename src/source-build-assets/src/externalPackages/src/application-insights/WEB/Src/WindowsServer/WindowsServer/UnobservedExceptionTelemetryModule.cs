namespace Microsoft.ApplicationInsights.WindowsServer
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights.Common;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.WindowsServer.Implementation;

    /// <summary>
    /// The module subscribed to TaskScheduler.UnobservedTaskException to send exceptions to ApplicationInsights.
    /// </summary>
    public sealed class UnobservedExceptionTelemetryModule : ITelemetryModule, IDisposable
    {
        private readonly Action<EventHandler<UnobservedTaskExceptionEventArgs>> registerAction;
        private readonly Action<EventHandler<UnobservedTaskExceptionEventArgs>> unregisterAction;
        private readonly object lockObject = new object();

        private TelemetryClient telemetryClient;
        private bool isInitialized = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnobservedExceptionTelemetryModule" /> class.
        /// </summary>
        public UnobservedExceptionTelemetryModule() : this(
            action => TaskScheduler.UnobservedTaskException += action,
            action => TaskScheduler.UnobservedTaskException -= action)
        {
        }

        internal UnobservedExceptionTelemetryModule(
            Action<EventHandler<UnobservedTaskExceptionEventArgs>> registerAction,
            Action<EventHandler<UnobservedTaskExceptionEventArgs>> unregisterAction)
        {
            this.registerAction = registerAction;
            this.unregisterAction = unregisterAction;
        }

        /// <summary>
        /// Initializes the telemetry module.
        /// </summary>
        /// <param name="configuration">Telemetry Configuration used for creating TelemetryClient for sending exceptions to ApplicationInsights.</param>
        public void Initialize(TelemetryConfiguration configuration)
        {
            // Core SDK creates 1 instance of a module but calls Initialize multiple times
            if (!this.isInitialized)
            {
                lock (this.lockObject)
                {
                    if (!this.isInitialized)
                    {
                        this.isInitialized = true;

                        this.telemetryClient = new TelemetryClient(configuration);
                        this.telemetryClient.Context.GetInternalContext().SdkVersion = SdkVersionUtils.GetSdkVersion("unobs:");

                        this.registerAction(this.TaskSchedulerOnUnobservedTaskException);
                    }
                }
            }
        }

        /// <summary>
        /// Disposing TaskSchedulerOnUnobservedTaskException instance.
        /// </summary>
        public void Dispose()
        {
            this.unregisterAction(this.TaskSchedulerOnUnobservedTaskException);
        }

        private void TaskSchedulerOnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs unobservedTaskExceptionEventArgs)
        {
            if (unobservedTaskExceptionEventArgs.Observed)
            {
                return;
            }

            WindowsServerEventSource.Log.TaskSchedulerOnUnobservedTaskException();

            var exp = new ExceptionTelemetry(unobservedTaskExceptionEventArgs.Exception)
            {
                SeverityLevel = SeverityLevel.Critical,
            };

            // It is theoretically possible for TrackException to throw another UnobservedTaskException but in practice
            // it won't, and even if it did, it would not cause an out of memory exception nor would it cause a stack
            // overflow exception.  The existing channels InMemoryChannel and ServerTelemetryChannel both have try
            // catches to make sure they don't throw unhandled exceptions.  In the event of complete failure they write
            // to the event source and do not throw.  If someone were to write their own channel which was not well
            // behaved, or someone were to break one of our channels so that it threw an exception, then we still would
            // not get a stack overflow because the unhandled exception would occur on another thread.  When calls are
            // made to TelemetryClient.Track they simply queue an item in the channel's buffer.  When the channel tries
            // to actually send the data in the buffer that occurs later on another thread.  So if the sending of the
            // data had an unhandled exception then the worst thing that would happen is that a single new telemetry
            // item would be queued in the buffer for the unhandled exception.  Since the buffer holds 500 items by
            // default we would at most get 1 extra item for every 499 regular items.  With our channels we currently
            // do not have such a bug.
            try
            {
                this.telemetryClient.TrackException(exp);
            }
            catch (Exception e)
            {
                WindowsServerEventSource.Log.UnobservedTaskExceptionThrewUnhandledException(e.ToString());
            }
        }
    }
}
