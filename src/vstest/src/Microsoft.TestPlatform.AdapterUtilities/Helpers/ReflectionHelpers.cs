// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.Text;

using Microsoft.TestPlatform.AdapterUtilities.ManagedNameUtilities;

namespace Microsoft.TestPlatform.AdapterUtilities.Helpers;

internal static partial class ReflectionHelpers
{
    private static void AssertSupport<T>(T obj, string methodName, string className)
        where T : class
    {
        if (obj == null)
        {
            throw new NotImplementedException(string.Format(CultureInfo.CurrentCulture, Resources.Resources.MethodNotImplementedOnPlatform, className, methodName));
        }
    }

    internal static string ParseEscapedString(string escapedString)
    {
        var stringBuilder = new StringBuilder();
        var end = 0;
        for (int i = 0; i < escapedString.Length; i++)
        {
            if (escapedString[i] == '\'')
            {
                stringBuilder.Append(escapedString, end, i - end);
                end = i = ParseEscapedStringSegment(escapedString, i + 1, stringBuilder);
            }
        }

        if (stringBuilder.Length == 0)
        {
            return escapedString;
        }

        if (end != 0 && end < escapedString.Length)
        {
            stringBuilder.Append(escapedString, end, escapedString.Length - end);
        }

        return stringBuilder.ToString();
    }

    // Unescapes a C# style escaped string.
    private static int ParseEscapedStringSegment(string escapedStringSegment, int pos, StringBuilder stringBuilder)
    {
        for (int i = pos; i < escapedStringSegment.Length; i++)
        {
            switch (escapedStringSegment[i])
            {
                case '\\':
                    if (escapedStringSegment[i + 1] == 'u')
                    {
                        char c;

                        try
                        {
                            var code = escapedStringSegment.Substring(i + 2, 4);
                            c = (char)Convert.ToInt32(code, 16);
                        }
                        catch
                        {
                            throw new InvalidManagedNameException(
                                string.Format(CultureInfo.CurrentCulture, Resources.Resources.ErrorInvalidSequenceAt, escapedStringSegment, i)
                            );
                        }

                        stringBuilder.Append(c);
                        i += 5;
                    }
                    else
                    {
                        stringBuilder.Append(escapedStringSegment[++i]);
                    }

                    break;

                case '\'':
                    return i + 1;

                default:
                    stringBuilder.Append(escapedStringSegment[i]);
                    break;
            }
        }

        string message = string.Format(CultureInfo.CurrentCulture, Resources.Resources.ErrorNoClosingQuote, escapedStringSegment);
        throw new InvalidManagedNameException(message);
    }
}
