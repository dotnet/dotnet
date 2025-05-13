namespace Microsoft.ApplicationInsights.Extensibility.Implementation.External
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    /// <summary>
    /// Partial class to implement ISerializableWithWriter.
    /// </summary>
    internal partial class MessageData
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
