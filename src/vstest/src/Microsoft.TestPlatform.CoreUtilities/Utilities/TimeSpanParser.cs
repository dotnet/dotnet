// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text.RegularExpressions;

namespace Microsoft.VisualStudio.TestPlatform.Utilities;

public static class TimeSpanParser
{
    static readonly Regex Pattern = new(@"(?<value>^\d+(?:\.\d+)?)\s*(?<suffix>ms|mil|m|h|d|s?[a-z]*)$", RegexOptions.IgnoreCase);

    public static TimeSpan Parse(string? time)
    {
        return TryParse(time, out var result) ? result : throw GetFormatException(time);
    }

    public static bool TryParse(string? time, out TimeSpan result)
    {
        if (string.IsNullOrWhiteSpace(time))
        {
            result = TimeSpan.Zero;
            return true;
        }

        var match = Pattern.Match(time);
        if (!match.Success)
        {
            result = TimeSpan.Zero;
            return false;
        }

        var value = match.Groups["value"].Value;
        if (!double.TryParse(value, out var number))
        {
            throw GetFormatException(value);
        }

        var suffix = match.Groups["suffix"].Value;
        var c = StringComparison.OrdinalIgnoreCase;

        // mil to distinguish milliseconds from minutes
        // ""  when there is just the raw milliseconds value
        if (suffix.StartsWith("ms", c) || suffix.StartsWith("mil", c) || suffix == string.Empty)
        {
            result = TimeSpan.FromMilliseconds(number);
            return true;
        }

        if (suffix.StartsWith("s", c))
        {
            result = TimeSpan.FromSeconds(number);
            return true;
        }

        if (suffix.StartsWith("m", c))
        {
            result = TimeSpan.FromMinutes(number);
            return true;
        }

        if (suffix.StartsWith("h", c))
        {
            result = TimeSpan.FromHours(number);
            return true;
        }

        if (suffix.StartsWith("d", c))
        {
            result = TimeSpan.FromDays(number);
            return true;
        }

        result = TimeSpan.Zero;
        return false;
    }

    static FormatException GetFormatException(string? value)
    {
        return new FormatException($"The value '{value}' is not a valid time string. Use a time string in this format 5400000 / 5400000ms / 5400s / 90m / 1.5h / 0.625d.");
    }
}
