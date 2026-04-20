namespace Microsoft.ApplicationInsights.AspNetCore.DiagnosticListeners
{
    using System;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;

    /// <summary>
    /// Utility functions for dealing with exceptions.
    /// </summary>
    internal class ExceptionUtilities
    {
        /// <summary>
        /// Get the string representation of this Exception with special handling for AggregateExceptions.
        /// </summary>
        /// <param name="ex">The exception to convert to a string.</param>
        /// <returns>Returns a string representing the Exception message, and call stack.</returns>
        internal static string GetExceptionDetailString(Exception ex)
        {
            if (ex is AggregateException ae)
            {
                return ae.Flatten().InnerException.ToInvariantString();
            }

            return ex.ToInvariantString();
        }
    }
}