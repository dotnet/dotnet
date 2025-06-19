#if NET452
namespace Microsoft.ApplicationInsights.DependencyCollector.Implementation
{
    using System;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.DependencyCollector.Implementation.Operation;

    internal class DependencyTableStore : IDisposable
    {
        internal static bool IsDesktopHttpDiagnosticSourceActivated = false;
        internal CacheBasedOperationHolder WebRequestCacheHolder;
        internal CacheBasedOperationHolder SqlRequestCacheHolder;
        internal ObjectInstanceBasedOperationHolder<DependencyTelemetry> WebRequestConditionalHolder;
        internal ObjectInstanceBasedOperationHolder<DependencyTelemetry> SqlRequestConditionalHolder;

        internal bool IsProfilerActivated = false;

        private static readonly DependencyTableStore SingletonInstance = new DependencyTableStore();

        private DependencyTableStore()
        {
            this.WebRequestCacheHolder = new CacheBasedOperationHolder("aisdkwebrequests", 100 * 1000);
            this.SqlRequestCacheHolder = new CacheBasedOperationHolder("aisdksqlrequests", 100 * 1000);
            this.WebRequestConditionalHolder = new ObjectInstanceBasedOperationHolder<DependencyTelemetry>();
            this.SqlRequestConditionalHolder = new ObjectInstanceBasedOperationHolder<DependencyTelemetry>();
        }

        internal static DependencyTableStore Instance
        {
            get
            {
                return SingletonInstance;
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.WebRequestCacheHolder.Dispose();
                this.SqlRequestCacheHolder.Dispose();
            }
        }
    }
}
#endif