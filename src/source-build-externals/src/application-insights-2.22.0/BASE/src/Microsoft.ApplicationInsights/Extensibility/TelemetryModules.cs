namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Threading;

    /// <summary>
    /// This API supports the AI Framework infrastructure and is not intended to be used directly from your code.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class TelemetryModules
    {
        private static TelemetryModules instance;

        /// <summary>
        /// Initializes a new instance of the <see cref="TelemetryModules"/> class.
        /// </summary>
        protected TelemetryModules()
        {
            this.Modules = new SnapshottingList<ITelemetryModule>();
        }

        /// <summary>
        /// Gets the TelemetryModules collection.
        /// </summary>
        public static TelemetryModules Instance
        {
            get { return LazyInitializer.EnsureInitialized(ref instance, () => new TelemetryModules()); }
        }

        /// <summary>
        /// Gets the telemetry modules collection.
        /// </summary>
        public IList<ITelemetryModule> Modules { get; private set; }        
    }
}
