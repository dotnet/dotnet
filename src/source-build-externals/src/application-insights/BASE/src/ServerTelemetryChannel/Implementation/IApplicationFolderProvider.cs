namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation
{
    internal interface IApplicationFolderProvider
    {
        /// <summary>
        /// Returns a per-user/per-application folder.
        /// </summary>
        /// <returns>
        /// An <see cref="IPlatformFolder"/> instance, or <c>null</c> if current application does not have access to file system.
        /// </returns>
        IPlatformFolder GetApplicationFolder();
    }
}
