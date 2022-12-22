// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.UnitTests;

public static class StringExtensions
{
    /// <summary>
    /// Replace all \r\n with \n to get Unix line endings to make strings easy to compare while still
    /// keeping the formatting in mind.
    /// </summary>
    /// <returns></returns>
    public static string NormalizeLineEndings(this string text)
    {
        return text.Replace("\r\n", "\n");
    }

    /// <summary>
    /// Replace whitespace with printable characters (and still keep \r newlines for easy readability)
    /// </summary>
    /// <returns></returns>
    public static string ShowWhiteSpace(this string text)
    {
        // use mongolian vowel separator as placeholder for the newline that we add for formatting
        var placeholder = "\u180E";
        if (text.Contains(placeholder))
        {
            throw new InvalidOperationException(
                "The text contains mongolian vowel separator character that we use as a placeholder.");
        }

        var whiteSpaced = text
            .Replace("\r\n", "\\r\\n\u180E")
            .Replace("\r", "\\r")
            .Replace("\n", "\\n\u180E")
            .Replace("\t", "\\t")
            .Replace(" ", "␣")
            .Replace("\u180E", "\n");

        // prepend one newline to get better output from assertion where both expected
        // and actual output start on the same position
        return "\n" + whiteSpaced;
    }
}
