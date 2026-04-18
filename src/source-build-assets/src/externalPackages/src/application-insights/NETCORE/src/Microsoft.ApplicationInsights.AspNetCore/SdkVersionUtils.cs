namespace Microsoft.ApplicationInsights.AspNetCore
{
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Utility class for the version information of the current assembly.
    /// </summary>
    internal class SdkVersionUtils
    {
#if NETFRAMEWORK
        /// <summary>
        /// SDK Version Prefix.
        /// </summary>
        public const string VersionPrefix = "aspnet5f:";
#else
        /// <summary>
        /// SDK Version Prefix.
        /// </summary>
        public const string VersionPrefix = "aspnet5c:";
#endif

        /// <summary>
        /// Get the Assembly Version with SDK prefix.
        /// </summary>
        /// <returns>Assembly version combined with this assembly's version prefix.</returns>
        internal static string GetVersion()
        {
            return VersionPrefix + GetAssemblyVersion();
        }

        /// <summary>
        /// Get the Assembly Version with given SDK prefix.
        /// </summary>
        /// <param name="versionPrefix">Prefix string to be included with the version.</param>
        /// <returns>Returns a string representing the current assembly version.</returns>
        internal static string GetVersion(string versionPrefix)
        {
            return versionPrefix + GetAssemblyVersion();
        }

        private static string GetAssemblyVersion()
        {
            return typeof(SdkVersionUtils).GetTypeInfo().Assembly.GetCustomAttributes<AssemblyInformationalVersionAttribute>()
                      .First()
                      .InformationalVersion;
        }
    }
}
