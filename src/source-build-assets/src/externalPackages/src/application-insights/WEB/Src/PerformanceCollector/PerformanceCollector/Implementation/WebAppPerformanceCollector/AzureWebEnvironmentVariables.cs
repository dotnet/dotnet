namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.WebAppPerfCollector
{
    using System;

    /// <summary>
    /// Enum for Azure Web App environment variables.
    /// </summary>
    [Flags]
    internal enum AzureWebApEnvironmentVariables
    {
        /// <summary>
        /// For ASP.NET.
        /// </summary>
        AspDotNet = 0,

        /// <summary>
        /// For Application.
        /// </summary>
        App = 1,

        /// <summary>
        /// For Common Language Runtime.
        /// </summary>
        CLR = 2,

        /// <summary>
        /// All of the above.
        /// </summary>
        All = 3,
    }
}
