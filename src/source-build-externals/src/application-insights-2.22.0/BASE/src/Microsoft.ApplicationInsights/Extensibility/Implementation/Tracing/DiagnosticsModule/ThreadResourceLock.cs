namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.DiagnosticsModule
{
    using System;

    /// <summary>
    /// Thread level resource section lock.
    /// </summary>
    internal class ThreadResourceLock : IDisposable
    {
        /// <summary>
        /// Thread level lock object.
        /// </summary>
        [ThreadStatic]
        private static object syncObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadResourceLock" /> class.
        /// Marks section locked.
        /// </summary>
        public ThreadResourceLock()
        {
            syncObject = new object();
        }

        /// <summary>
        /// Gets a value indicating whether lock is set on the section.
        /// </summary>
        public static bool IsResourceLocked
        {
            get { return syncObject != null; }
        }

        /// <summary>
        /// Release lock.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private static void Dispose(bool disponing)
        {
            if (disponing)
            {
                syncObject = null;
            }
        }
    }
}
