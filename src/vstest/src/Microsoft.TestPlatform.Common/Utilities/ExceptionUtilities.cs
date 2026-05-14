// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text;

namespace Microsoft.VisualStudio.TestPlatform.Common.Utilities;

/// <summary>
/// Exception utilities.
/// </summary>
public class ExceptionUtilities
{
    /// <summary>
    /// Returns an exception message with all inner exceptions messages.
    /// </summary>
    /// <param name="exception"> The exception. </param>
    /// <returns> The formatted string message of the exception. </returns>
    public static string GetExceptionMessage(Exception? exception)
    {
        if (exception == null)
        {
            return string.Empty;
        }

        var exceptionString = new StringBuilder(exception.Message);
        AppendStackTrace(exceptionString, exception);

        var inner = exception.InnerException;
        while (inner != null)
        {
            exceptionString
                .AppendLine()
                .Append(Resources.Resources.InnerException).Append(' ').AppendLine(inner.Message);
            AppendStackTrace(exceptionString, inner);
            inner = inner.InnerException;
        }

        return exceptionString.ToString();
    }

    private static void AppendStackTrace(StringBuilder stringBuilder, Exception exception)
    {
        if (!exception.StackTrace.IsNullOrEmpty())
        {
            stringBuilder
                .AppendLine()
                .AppendLine(Resources.Resources.StackTrace)
                .AppendLine(exception.StackTrace);
        }
    }
}
