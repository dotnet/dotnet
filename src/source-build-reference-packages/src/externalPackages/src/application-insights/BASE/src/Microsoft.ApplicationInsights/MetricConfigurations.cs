namespace Microsoft.ApplicationInsights
{
    using System;

    /// <summary>
    /// Static container for the most commonly used metric configurations.
    /// </summary>
    public sealed class MetricConfigurations
    {
        /// <summary>
        /// Groups extension methods that return pre-defined metric configurations and related constants.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Singleton is intended.")]
        public static readonly MetricConfigurations Common = new MetricConfigurations();

        private MetricConfigurations()
        {
        }
    }
}
