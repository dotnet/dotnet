namespace Microsoft.ApplicationInsights.Extensibility.Implementation.External
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Microsoft.ApplicationInsights;
    using Microsoft.ApplicationInsights.DataContracts;

    /// <summary>
    /// Partial class to add the Lazy instantiation of ConcurrentDictionary.
    /// </summary>
    internal partial class EventData
    {
        private IDictionary<string, double> measurementsInternal;

#pragma warning disable SA1300 // Element must begin with upper-case letter
        public IDictionary<string, double> measurements
#pragma warning restore SA1300 // Element must begin with upper-case letter
        {
            get { return System.Threading.LazyInitializer.EnsureInitialized(ref this.measurementsInternal, () => new ConcurrentDictionary<string, double>()); }
            set { this.measurementsInternal = value; }
        }
    }
}
