namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using System;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.External;
    using DpSeverityLevel = Microsoft.ApplicationInsights.DataContracts.SeverityLevel;

    internal static class SeverityLevelExtensions
    {
        public static DpSeverityLevel? TranslateSeverityLevel(this SeverityLevel? sdkSeverityLevel)
        {
            if (sdkSeverityLevel == null)
            {
                return null;
            }

            switch (sdkSeverityLevel.Value)
            {
                case SeverityLevel.Critical: return DpSeverityLevel.Critical;
                case SeverityLevel.Error: return DpSeverityLevel.Error;
                case SeverityLevel.Warning: return DpSeverityLevel.Warning;
                case SeverityLevel.Information: return DpSeverityLevel.Information;
                default: return DpSeverityLevel.Verbose;
            }
        }

        public static SeverityLevel? TranslateSeverityLevel(this DpSeverityLevel? dataPlatformSeverityLevel)
        {
            if (dataPlatformSeverityLevel == null)
            {
                return null;
            }

            switch (dataPlatformSeverityLevel.Value)
            {
                case DpSeverityLevel.Critical: return SeverityLevel.Critical;
                case DpSeverityLevel.Error: return SeverityLevel.Error;
                case DpSeverityLevel.Warning: return SeverityLevel.Warning;
                case DpSeverityLevel.Information: return SeverityLevel.Information;
                default: return SeverityLevel.Verbose;
            }
        }
    }
}
