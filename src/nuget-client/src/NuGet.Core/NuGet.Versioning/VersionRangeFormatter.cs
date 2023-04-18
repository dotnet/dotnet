// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Text;

namespace NuGet.Versioning
{
    /// <summary>
    /// Custom formatter for NuGet <see cref="VersionRange"/>.
    /// </summary>
    public class VersionRangeFormatter : IFormatProvider, ICustomFormatter
    {
        private const string ZeroN = "{0:N}";
        private static readonly VersionFormatter VersionFormatter = VersionFormatter.Instance;

        /// <summary>
        /// A static instance of the <see cref="VersionRangeFormatter"/> class.
        /// </summary>
        public static readonly VersionRangeFormatter Instance = new VersionRangeFormatter();

        /// <summary>
        /// Format a version range string.
        /// </summary>
        public string Format(string? format, object? arg, IFormatProvider? formatProvider)
        {
            if (arg == null)
            {
                throw new ArgumentNullException(nameof(arg));
            }

            var range = arg as VersionRange;
            if (range == null)
            {
                throw ResourcesFormatter.TypeNotSupported(arg.GetType(), nameof(arg));
            }

            if (string.IsNullOrEmpty(format))
            {
                format = "N";
            }

            // single char identifiers
            if (format!.Length == 1)
            {
                string formatted = Format(format[0], range);
                return formatted;
            }
            else
            {
                var sb = new StringBuilder(format.Length);

                for (var i = 0; i < format.Length; i++)
                {
                    var s = Format(format[i], range);

                    if (s == null)
                    {
                        sb.Append(format[i]);
                    }
                    else
                    {
                        sb.Append(s);
                    }
                }

                string formatted = sb.ToString();
                return formatted;
            }
        }

        /// <summary>
        /// Format type.
        /// </summary>
        public object? GetFormat(Type? formatType)
        {
            if (typeof(VersionRange).IsAssignableFrom(formatType))
            {
                return this;
            }

            return null;
        }

        private string Format(char c, VersionRange range)
        {
            string s = string.Empty;

            switch (c)
            {
                case 'P':
                    s = PrettyPrint(range, useParentheses: true);
                    break;
                case 'p':
                    s = PrettyPrint(range, useParentheses: false);
                    break;
                case 'L':
                    s = range.HasLowerBound ? string.Format(VersionFormatter, ZeroN, range.MinVersion) : string.Empty;
                    break;
                case 'U':
                    s = range.HasUpperBound ? string.Format(VersionFormatter, ZeroN, range.MaxVersion) : string.Empty;
                    break;
                case 'S':
                    s = GetToString(range);
                    break;
                case 'N':
                    s = GetNormalizedString(range);
                    break;
                case 'D':
                    s = GetLegacyString(range);
                    break;
                case 'T':
                    s = GetLegacyShortString(range);
                    break;
                case 'A':
                    s = GetShortString(range);
                    break;
            }

            return s;
        }

        private string GetShortString(VersionRange range)
        {
            string s;

            if (range.HasLowerBound
                && range.IsMinInclusive
                && !range.HasUpperBound)
            {
                s = range.IsFloating ?
                    range.Float.ToString() :
                    string.Format(VersionFormatter, ZeroN, range.MinVersion);
            }
            else if (range.HasLowerAndUpperBounds
                     && range.IsMinInclusive
                     && range.IsMaxInclusive
                     &&
                     range.MinVersion.Equals(range.MaxVersion))
            {
                // Floating should be ignored here.
                s = string.Format(VersionFormatter, "[{0:N}]", range.MinVersion);
            }
            else
            {
                s = GetNormalizedString(range);
            }

            return s;
        }

        /// <summary>
        /// Builds a normalized string with no short hand
        /// </summary>
        private string GetNormalizedString(VersionRange range)
        {
            var sb = new StringBuilder();

            sb.Append(range.HasLowerBound && range.IsMinInclusive ? '[' : '(');

            if (range.HasLowerBound)
            {
                if (range.IsFloating)
                {
                    sb.Append(range.Float.ToString());
                }
                else
                {
                    sb.AppendFormat(VersionFormatter, ZeroN, range.MinVersion);
                }
            }

            sb.Append(", ");

            if (range.HasUpperBound)
            {
                sb.AppendFormat(VersionFormatter, ZeroN, range.MaxVersion);
            }

            sb.Append(range.HasUpperBound && range.IsMaxInclusive ? ']' : ')');

            return sb.ToString();
        }

        /// <summary>
        /// Builds a string to represent the VersionRange. This string can include short hand notations.
        /// </summary>
        private string GetToString(VersionRange range)
        {
            string s;

            if (range.HasLowerBound
                && range.IsMinInclusive
                && !range.HasUpperBound)
            {
                s = string.Format(VersionFormatter, ZeroN, range.MinVersion);
            }
            else if (range.HasLowerAndUpperBounds
                     && range.IsMinInclusive
                     && range.IsMaxInclusive
                     &&
                     range.MinVersion.Equals(range.MaxVersion))
            {
                // TODO: Does this need a specific version comparision? Does metadata matter?

                s = string.Format(VersionFormatter, "[{0:N}]", range.MinVersion);
            }
            else
            {
                s = GetNormalizedString(range);
            }

            return s;
        }

        /// <summary>
        /// Creates a legacy short string that is compatible with NuGet 2.8.3
        /// </summary>
        private string GetLegacyShortString(VersionRangeBase range)
        {
            string s;

            if (range.HasLowerBound
                && range.IsMinInclusive
                && !range.HasUpperBound)
            {
                s = string.Format(VersionFormatter, ZeroN, range.MinVersion);
            }
            else if (range.HasLowerAndUpperBounds
                     && range.IsMinInclusive
                     && range.IsMaxInclusive
                     &&
                     range.MinVersion.Equals(range.MaxVersion))
            {
                s = string.Format(VersionFormatter, "[{0:N}]", range.MinVersion);
            }
            else
            {
                s = GetLegacyString(range);
            }

            return s;
        }

        /// <summary>
        /// Creates a legacy string that is compatible with NuGet 2.8.3
        /// </summary>
        private string GetLegacyString(VersionRangeBase range)
        {
            var sb = new StringBuilder();

            sb.Append(range.HasLowerBound && range.IsMinInclusive ? '[' : '(');

            if (range.HasLowerBound)
            {
                sb.AppendFormat(VersionFormatter, ZeroN, range.MinVersion);
            }

            sb.Append(", ");

            if (range.HasUpperBound)
            {
                sb.AppendFormat(VersionFormatter, ZeroN, range.MaxVersion);
            }

            sb.Append(range.HasUpperBound && range.IsMaxInclusive ? ']' : ')');

            return sb.ToString();
        }

        /// <summary>
        /// A pretty print representation of the VersionRange.
        /// </summary>
        private string PrettyPrint(VersionRange range, bool useParentheses)
        {
            // empty range
            if (!range.HasLowerBound
                 && !range.HasUpperBound)
            {
                return string.Empty;
            }

            // single version
            if (range.HasLowerAndUpperBounds
                     && range.MaxVersion.Equals(range.MinVersion)
                     && range.IsMinInclusive
                     && range.IsMaxInclusive)
            {
                if (useParentheses)
                {
                    return string.Format(VersionFormatter, "(= {0:N})", range.MinVersion);
                }
                else return string.Format(VersionFormatter, "= {0:N}", range.MinVersion);
            }

            // normal case with a lower, upper, or both.
            var sb = new StringBuilder(useParentheses ? "(" : string.Empty);

            if (range.HasLowerBound)
            {
                PrettyPrintBound(sb, range.MinVersion, range.IsMinInclusive, ">");
            }

            if (range.HasLowerAndUpperBounds)
            {
                sb.Append(" && ");
            }

            if (range.HasUpperBound)
            {
                PrettyPrintBound(sb, range.MaxVersion, range.IsMaxInclusive, "<");
            }

            if (useParentheses)
            {
                sb.Append(")");
            }

            return sb.ToString();
        }

        private void PrettyPrintBound(StringBuilder sb, NuGetVersion version, bool inclusive, string boundChar)
        {
            sb.Append(boundChar);

            if (inclusive)
            {
                sb.Append("=");
            }

            sb.Append(" ");
            sb.AppendFormat(VersionFormatter, ZeroN, version);
        }
    }
}
