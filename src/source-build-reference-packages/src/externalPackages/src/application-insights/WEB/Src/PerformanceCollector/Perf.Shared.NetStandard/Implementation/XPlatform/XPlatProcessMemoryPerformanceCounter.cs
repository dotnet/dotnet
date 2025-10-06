namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.XPlatform
{
    using System;
    using System.Diagnostics;
    using System.Globalization;

    /// <summary>
    /// Represents Private Bytes performance counter equivalent for the current process.
    /// https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.process.privatememorysize64.
    /// </summary>
    internal class XPlatProcessMemoryPerformanceCounter : ICounterValue
    {        
        /// <summary>
        /// Returns the current value of the counter.
        /// </summary>
        /// <returns>Value of the counter.</returns>
        public double Collect()
        {
            try
            {
                return Process.GetCurrentProcess().PrivateMemorySize64;
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        "Failed to perform a read for performance counter XPlatProcessMemoryPerformanceCounter. Exception: {0}", e));
            }
        }
    }
}
