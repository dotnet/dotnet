namespace Microsoft.ApplicationInsights.WindowsServer
{
    using System;
    using System.ComponentModel;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;
    using Microsoft.ApplicationInsights.WindowsServer.Implementation;

    /// <summary>
    /// A telemetry module that adds Azure instance metadata context information to the heartbeat, if it is available.
    /// </summary>
    public sealed class AzureInstanceMetadataTelemetryModule : ITelemetryModule
    {
        private object lockObject = new object();
        private IHeartbeatPropertyManager heartbeatManager;

        /// <summary>
        /// Initializes a new instance of the<see cref="AzureInstanceMetadataTelemetryModule" /> class.
        /// </summary>
        public AzureInstanceMetadataTelemetryModule() : this(null)
        {
        }

        internal AzureInstanceMetadataTelemetryModule(IHeartbeatPropertyManager heartbeatPropertyManager) => this.HeartbeatPropertyManager = heartbeatPropertyManager;

        /// <summary>
        /// Gets or sets an instance of IHeartbeatPropertyManager. 
        /// </summary>
        /// <remarks>
        /// This is expected to be an instance of <see cref="DiagnosticsTelemetryModule"/>.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public IHeartbeatPropertyManager HeartbeatPropertyManager
        {
            get
            {
                if (this.heartbeatManager == null)
                {
                    this.heartbeatManager = HeartbeatPropertyManagerProvider.GetHeartbeatPropertyManager();
                }

                return this.heartbeatManager;
            }

            set => this.heartbeatManager = value;
        }

        /// <summary>Gets a value indicating whether this module has been initialized.</summary>
        internal bool IsInitialized { get; private set; } = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureInstanceMetadataTelemetryModule" /> class.
        /// Creates a heartbeat property collector that obtains and inserts data from the Azure Instance
        /// Metadata service if it is present and available to the currently running process. If it is not
        /// present no added IMS data is added to the heartbeat.
        /// </summary>
        /// <param name="unused">Unused parameter for this TelemetryModule.</param>
        public void Initialize(TelemetryConfiguration unused)
        {
            // Core SDK creates 1 instance of a module but calls Initialize multiple times
            if (!this.IsInitialized)
            {
                lock (this.lockObject)
                {
                    if (!this.IsInitialized)
                    {
                        var hbeatManager = this.HeartbeatPropertyManager;
                        if (hbeatManager != null)
                        {
                            // start off the heartbeat property collection process, but don't wait for it nor report
                            // any status from here, fire and forget. The thread running the collection will report 
                            // to the core event log.
                            try
                            {
                                var heartbeatProperties = new AzureComputeMetadataHeartbeatPropertyProvider();
                                Task.Factory.StartNew(
                                    async () => await heartbeatProperties.SetDefaultPayloadAsync(hbeatManager)
                                    .ConfigureAwait(false));
                            }
                            catch (Exception heartbeatAquisitionException)
                            {
                                WindowsServerEventSource.Log.AzureInstanceMetadataFailureWithException(heartbeatAquisitionException.Message, heartbeatAquisitionException.InnerException?.Message);
                            }
                        }

                        this.IsInitialized = true;
                    }
                }
            }
        }
    }
}
