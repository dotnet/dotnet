namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse.PerfLib
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Runtime.Versioning;
    using System.Security.Permissions;
    using System.Threading;

    using Microsoft.Win32;

    /// <summary>
    /// Represents the low-level performance monitor.
    /// </summary>
    internal class PerformanceMonitor
    {
        private RegistryKey perfDataKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="PerformanceMonitor"/> class. 
        /// </summary>
        public PerformanceMonitor()
        {
            this.Init();
        }
        
        /// <summary>
        /// Closes the monitor.
        /// </summary>
        public void Close()
        {
            this.perfDataKey?.Close();

            this.perfDataKey = null;
        }

        /// <summary>
        /// Win32 <c>RegQueryValueEx</c> for performance data could deadlock (for a Mutex) up to 2 minutes in some 
        /// scenarios before they detect it and exit gracefully. In the mean time, ERROR_BUSY, 
        /// ERROR_NOT_READY etc can be seen by other concurrent calls (which is the reason for the 
        /// wait loop and switch case below). We want to wait most certainly more than a 2min window. 
        /// The current wait time of up to 10 minutes takes care of the known stress deadlock issues. In most 
        /// cases we wouldn't wait for more than 2 minutes anyways but in worst cases how much ever time 
        /// we wait may not be sufficient if the Win32 code keeps running into this deadlock again 
        /// and again. A condition very rare but possible in theory. We would get back to the user 
        /// in this case with InvalidOperationException after the wait time expires.
        /// </summary>
        public byte[] GetData(string categoryIndex)
        {
            int waitRetries = 3; // 17;   //2^16*10ms == approximately 10mins
            int waitSleep = 0;
            int error = 0;

            // no need to revert here since we'll fall off the end of the method
            new RegistryPermission(PermissionState.Unrestricted).Assert();
            while (waitRetries > 0)
            {
                try
                {
                    return (byte[])this.perfDataKey.GetValue(categoryIndex);
                }
                catch (IOException e)
                {
                    error = Marshal.GetHRForException(e);
                    switch (error)
                    {
                        case NativeMethods.RPC_S_CALL_FAILED:
                        case NativeMethods.ERROR_INVALID_HANDLE:
                        case NativeMethods.RPC_S_SERVER_UNAVAILABLE:
                            this.Init();
                            goto case NativeMethods.WAIT_TIMEOUT;

                        case NativeMethods.WAIT_TIMEOUT:
                        case NativeMethods.ERROR_NOT_READY:
                        case NativeMethods.ERROR_LOCK_FAILED:
                        case NativeMethods.ERROR_BUSY:
                            --waitRetries;
                            if (waitSleep == 0)
                            {
                                waitSleep = 10;
                            }
                            else
                            {
                                Thread.Sleep(waitSleep);
                                waitSleep *= 2;
                            }

                            break;

                        default:
                            throw SharedUtils.CreateSafeWin32Exception(error);
                    }
                }
                catch (InvalidCastException e)
                {
                    throw new InvalidOperationException("Counter data is corrupt " + this.perfDataKey, e);
                }
            }

            throw SharedUtils.CreateSafeWin32Exception(error);
        }

        [ResourceExposure(ResourceScope.None)]
        [ResourceConsumption(ResourceScope.Machine, ResourceScope.Machine)]
        private void Init()
        {
            try
            {
                this.perfDataKey = Registry.PerformanceData;
            }
            catch (UnauthorizedAccessException e)
            {
                throw new UnauthorizedAccessException("Access denied opening the performance key", e);
            }
        }
    }
}
