namespace Microsoft.ApplicationInsights.Extensibility
{
#if NET452
    using System;
    using System.Runtime.Remoting.Messaging;

    /// <summary>
    /// Helps to define whether thread is performing SDK internal operation at the moment.
    /// </summary>
    public static class SdkInternalOperationsMonitor
    {
        internal const string InternalOperationsMonitorSlotName = "Microsoft.ApplicationInsights.InternalSdkOperation";

        private static Object syncObj = new object();

        /// <summary>
        /// Determines whether the current thread executing the internal operation.
        /// </summary>
        /// <returns>true if the current thread executing the internal operation; otherwise, false.</returns>
        public static bool IsEntered()
        {
            object data = null;
            try
            {
                data = CallContext.LogicalGetData(InternalOperationsMonitorSlotName);
            }
            catch (Exception)
            {
                // CallContext may fail in partially trusted environment
            }

            return data != null;
        }

        /// <summary>
        /// Marks the thread as executing the internal operation.
        /// </summary>
        public static void Enter()
        {
            try
            {
                CallContext.LogicalSetData(InternalOperationsMonitorSlotName, syncObj);
            }
            catch (Exception)
            {
                // CallContext may fail in partially trusted environment
            }
        }

        /// <summary>
        /// Unmarks the thread as executing the internal operation.
        /// </summary>
        public static void Exit()
        {
            try
            {
                CallContext.FreeNamedDataSlot(InternalOperationsMonitorSlotName);
            }
            catch (Exception)
            {
                // CallContext may fail in partially trusted environment
            }
        }
    }
#else
    using System.Threading;

    /// <summary>
    /// Helps to define whether thread is performing SDK internal operation at the moment.
    /// </summary>
    public static class SdkInternalOperationsMonitor
    {
        private static AsyncLocal<object> asyncLocalContext = new AsyncLocal<object>();

        private static object syncObj = new object();

        /// <summary>
        /// Determines whether the current thread executing the internal operation.
        /// </summary>
        /// <returns>true if the current thread executing the internal operation; otherwise, false.</returns>
        public static bool IsEntered()
        {
            return asyncLocalContext.Value != null;
        }

        /// <summary>
        /// Marks the thread as executing the internal operation.
        /// </summary>
        public static void Enter()
        {
            asyncLocalContext.Value = syncObj;
        }

        /// <summary>
        /// Unmarks the thread as executing the internal operation.
        /// </summary>
        public static void Exit()
        {
            asyncLocalContext.Value = null;
        }
    }
#endif
}
