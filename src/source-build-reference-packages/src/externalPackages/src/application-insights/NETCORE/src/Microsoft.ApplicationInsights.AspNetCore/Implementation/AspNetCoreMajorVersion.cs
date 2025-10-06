namespace Microsoft.ApplicationInsights.AspNetCore.Implementation
{
    /// <summary>
    /// Represents the runtime version of AspNetCore.
    /// </summary>
    internal enum AspNetCoreMajorVersion
    {
        /// <summary>
        /// .NET Core Version 1.0
        /// </summary>
        One,

        /// <summary>
        /// .NET Core Version 2.0
        /// </summary>
        Two,

        /// <summary>
        /// .NET Core Version 3.0 or higher
        /// </summary>
        Three,
    }
}
