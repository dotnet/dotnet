#if NETFRAMEWORK
namespace Microsoft.ApplicationInsights.WindowsServer.Implementation
{
    /// <summary>
    /// The user context reader interface used while reading user related information in a platform specific way.
    /// </summary>
    internal interface IAzureRoleEnvironmentContextReader
    {
        /// <summary>
        /// Initializes the current reader with respect to its environment.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Gets the Azure role name.
        /// </summary>
        /// <returns>The extracted data.</returns>
        string GetRoleName();

        /// <summary>
        /// Gets the Azure role instance name.
        /// </summary>
        /// <returns>The extracted data.</returns>
        string GetRoleInstanceName();
    }
}
#endif