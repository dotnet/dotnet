// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;

namespace NuGet.Common
{
    public static class LoggingExtensions
    {
        /// <summary>
        /// Formats a ILogMessage into a string representation containg the log code and message.
        /// The log code is added only if it is a valid NuGetLogCode and is greater than NuGetLogCode.Undefined.
        /// </summary>
        /// <param name="message">ILogMessage to be formatted.</param>
        /// <returns>string representation of the ILogMessage.</returns>
        public static string FormatWithCode(this ILogMessage message)
        {
            if (message.Code > NuGetLogCode.Undefined && message.Code.TryGetName(out var codeString))
            {
                return $"{codeString}: {message.Message}";
            }
            else
            {
                return message.Message;
            }
        }

        /// <summary>
        /// Formats a NuGetLogCode into a string representation.
        /// </summary>
        /// <param name="code">NuGetLogCode to be formatted into string.</param>
        /// <returns>strings representation of the NuGetLogCode, or null if no such constant exists.</returns>
        public static string? GetName(this NuGetLogCode code)
        {
            return Enum.GetName(typeof(NuGetLogCode), code);
        }

        /// <summary>
        /// Tries to get the string from the NuGetLogCode enum.
        /// </summary>
        /// <param name="code">NuGetLogCode to be formatted into string.</param>
        /// <param name="codeString">strings representation of the NuGetLogCode if the result is true else null.</param>
        /// <returns>bool indicating if the GetName operation was successful or not.</returns>
        public static bool TryGetName(this NuGetLogCode code, [NotNullWhen(true)] out string? codeString)
        {
            codeString = code.GetName();
            return codeString != null;
        }
    }
}
