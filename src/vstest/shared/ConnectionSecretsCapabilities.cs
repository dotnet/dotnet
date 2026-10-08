// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

#pragma warning disable IDE0161 // Linked into servicing branches whose compilers predate file-scoped namespaces.
namespace Microsoft.VisualStudio.TestPlatform.Utilities
{
    /// <summary>
    /// Where a user is sent when a connection negotiation fails, and how the detail lines under such
    /// a message are put together.
    ///
    /// The link is used rather than a list of versions on purpose. A version list compiled into a
    /// package is a promise that cannot be corrected: it is published in every serviced band at once,
    /// and if a band then ships as a different patch, or does not ship at all, every already published
    /// package names a version that does not exist. A page can be corrected after the fact.
    ///
    /// Whether a given build can negotiate at all is decided by ConnectionSecretsMarker, which reads a
    /// marker compiled into that build, so nothing here takes part in the decision.
    /// </summary>
    internal static class ConnectionSecretsCapabilities
    {
        /// <summary>
        /// Named in every negotiation failure, so a user is told where to find the versions that carry
        /// the fix instead of being left with a connection timeout.
        /// </summary>
        internal const string NegotiationErrorLink = "https://aka.ms/vstest/negotiation-error";

        /// <summary>
        /// Formats one detail line, or returns null when any of its values is missing.
        ///
        /// A value we could not determine is left out entirely rather than printed as "unknown". An
        /// absent line reads as "we could not determine this", while a line saying "unknown" reads
        /// like a value and invites the reader to wonder whether that is the problem.
        /// </summary>
        internal static string Detail(string format, params object[] values)
        {
            foreach (var value in values)
            {
                if (value == null)
                {
                    return string.Empty;
                }

                if (value is string text && text.Length == 0)
                {
                    return string.Empty;
                }
            }

            return string.Format(CultureInfo.CurrentCulture, format, values);
        }

        /// <summary>
        /// Appends the detail lines under a message, separated from it by a blank line. Lines that
        /// could not be determined are skipped, and a message whose details are all missing is
        /// returned unchanged.
        /// </summary>
        internal static string WithDetails(string message, IEnumerable<string> details)
        {
            var builder = new StringBuilder(message);
            var first = true;

            foreach (var line in details)
            {
                if (line == null || line.Length == 0)
                {
                    continue;
                }

                if (first)
                {
                    builder.Append(Environment.NewLine);
                    first = false;
                }

                builder.Append(Environment.NewLine).Append(line);
            }

            return builder.ToString();
        }
    }
}
#pragma warning restore IDE0161
