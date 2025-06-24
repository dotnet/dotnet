namespace Microsoft.ApplicationInsights.Common
{
    using System;
    using System.Globalization;

    internal static class GuidExtensions
    {
        /// <summary>
        /// Overload for Guid.ToString(). 
        /// </summary>
        /// <remarks>
        /// This method encapsulates the language switch for NetStandard and NetFramework and resolves the error "The behavior of guid.ToStrinc() could vary based on the current user's locale settings".
        /// </remarks>
        public static string ToStringInvariant(this Guid guid, string format)
        {
            return guid.ToString(format, CultureInfo.InvariantCulture);
        }
    }
}
