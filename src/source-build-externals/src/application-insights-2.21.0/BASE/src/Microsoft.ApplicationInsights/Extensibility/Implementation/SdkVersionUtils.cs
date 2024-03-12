namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    internal class SdkVersionUtils
    {
        /// <summary>
        /// Builds a string representing file version of the assembly with added prefix
        /// in format prefix:major.minor-revision.
        /// </summary>
        /// <param name="versionPrefix">Prefix to add to version.</param>
        /// <returns>String representation of the version with prefix added.</returns>
        internal static string GetSdkVersion(string versionPrefix)
        {
            string versionStr = typeof(TelemetryClient).Assembly.GetCustomAttributes(false)
                    .OfType<AssemblyFileVersionAttribute>()
                    .First()
                    .Version;

            Version version;

            // this may happen when Application Insights SDK assembly was merged into another assembly 
            // See https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/435 for details.
            if (string.IsNullOrEmpty(versionStr) || !Version.TryParse(versionStr, out version))
            {
                return null;
            }

            string postfix = version.Revision.ToString(CultureInfo.InvariantCulture);

            return versionPrefix + version.ToString(3) + "-" + postfix;
        }
    }
}