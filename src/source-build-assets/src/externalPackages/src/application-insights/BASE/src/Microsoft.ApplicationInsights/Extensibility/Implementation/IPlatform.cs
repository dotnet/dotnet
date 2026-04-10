namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using Microsoft.ApplicationInsights.Extensibility;

    /// <summary>
    /// Encapsulates platform-specific functionality required by the API.
    /// </summary>
    /// <remarks>
    /// This type is public to enable mocking on Windows Phone.
    /// </remarks>
    internal interface IPlatform
    {
        /// <summary>
        /// Returns contents of the ApplicationInsights.config file in the application directory.
        /// </summary>
        string ReadConfigurationXml();

        /// <summary>
        /// Returns the platform specific Debugger writer to the VS output console.
        /// </summary>
        IDebugOutput GetDebugOutput();

        /// <summary>
        /// Find an environment variable by name. Will evaluate if that variable is empty.
        /// </summary>
        /// <param name="name">Name of environment variable.</param>
        /// <param name="value">Contains the value of the specified name.</param>
        /// <returns>Returns true if a non-empty value was found.</returns>
        bool TryGetEnvironmentVariable(string name, out string value);

        /// <summary>
        /// Returns the machine name.
        /// </summary>
        /// <returns>The machine name.</returns>
        string GetMachineName();
    }
}
