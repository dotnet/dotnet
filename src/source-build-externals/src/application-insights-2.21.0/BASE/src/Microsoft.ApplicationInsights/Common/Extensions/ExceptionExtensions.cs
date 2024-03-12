namespace Microsoft.ApplicationInsights.Common.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    /// <summary>
    /// Provides a set of extension methods for <see cref="Exception"/>.
    /// </summary>
    internal static class ExceptionExtensions
    {
        /// <summary>
        /// Concatenate the Message property of an Exception and any InnerExceptions.
        /// </summary>
        /// <param name="ex">Exception to flatten.</param>
        /// <returns>Returns a concatenated string of exception messages.</returns>
        public static string FlattenMessages(this Exception ex)
        {
            var list = new List<string>();

            for (var tempEx = ex; tempEx != null; tempEx = tempEx.InnerException)
            {
                list.Add(tempEx.Message);
            }

            return string.Join(" | ", list);
        }

        /// <summary>
        /// Get a string representing an Exception. Includes Type and Message.
        /// </summary>
        /// <param name="ex">Input exception.</param>
        /// <returns>Returns a string representing the exception.</returns>
        public static string ToLogString(this Exception ex)
        {
            string msg = "Type: '{0}' Message: '{1}'";
            return string.Format(CultureInfo.InvariantCulture, msg, ex.GetType().ToString(), ex.FlattenMessages());
        }
    }
}
