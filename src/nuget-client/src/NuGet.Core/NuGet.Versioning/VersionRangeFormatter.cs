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

            if (arg is not VersionRange range)
            {
                throw ResourcesFormatter.TypeNotSupported(arg.GetType(), nameof(arg));
            }

            if (string.IsNullOrEmpty(format))
            {
                format = "N";
            }

            var builder = StringBuilderPool.Shared.Rent(256);

            for (var i = 0; i < format!.Length; i++)
            {
                Format(builder, format[i], range);
            }

            return StringBuilderPool.Shared.ToStringAndReturn(builder);
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

        private static void Format(StringBuilder builder, char c, VersionRange range)
        {
            switch (c)
            {
                case 'P':
                    PrettyPrint(builder, range, useParentheses: true);
                    break;
                case 'p':
                    PrettyPrint(builder, range, useParentheses: false);
                    break;
                case 'L':
                    if (range.HasLowerBound)
                    {
                        VersionFormatter.AppendNormalized(builder, range.MinVersion);
                    }
                    break;
                case 'U':
                    if (range.HasUpperBound)
                    {
                        VersionFormatter.AppendNormalized(builder, range.MaxVersion);
                    }
                    break;
                case 'S':
                    GetToString(builder, range);
                    break;
                case 'N':
                    GetNormalizedString(builder, range);
                    break;
                case 'D':
                    GetLegacyString(builder, range);
                    break;
                case 'T':
                    GetLegacyShortString(builder, range);
                    break;
                case 'A':
                    GetShortString(builder, range);
                    break;
                default:
                    builder.Append(c);
                    break;
            }
        }

        private static void GetShortString(StringBuilder builder, VersionRange range)
        {
            if (range.HasLowerBound
                && range.IsMinInclusive
                && !range.HasUpperBound)
            {
                if (range.IsFloating)
                {
                    range.Float.ToString(builder);
                }
                else
                {
                    VersionFormatter.AppendNormalized(builder, range.MinVersion);
                }
            }
            else if (range.HasLowerAndUpperBounds
                     && range.IsMinInclusive
                     && range.IsMaxInclusive
                     && range.MinVersion.Equals(range.MaxVersion))
            {
                // Floating should be ignored here.
                builder.Append('[');
                VersionFormatter.AppendNormalized(builder, range.MinVersion);
                builder.Append(']');
            }
            else
            {
                GetNormalizedString(builder, range);
            }
        }

        /// <summary>
        /// Builds a normalized string with no short hand
        /// </summary>
        private static void GetNormalizedString(StringBuilder builder, VersionRange range)
        {
            builder.Append(range.HasLowerBound && range.IsMinInclusive ? '[' : '(');

            if (range.HasLowerBound)
            {
                if (range.IsFloating)
                {
                    range.Float.ToString(builder);
                }
                else
                {
                    VersionFormatter.AppendNormalized(builder, range.MinVersion);
                }
            }

            builder.Append(", ");

            if (range.HasUpperBound)
            {
                VersionFormatter.AppendNormalized(builder, range.MaxVersion);
            }

            builder.Append(range.HasUpperBound && range.IsMaxInclusive ? ']' : ')');
        }

        /// <summary>
        /// Builds a string to represent the VersionRange. This string can include short hand notations.
        /// </summary>
        private static void GetToString(StringBuilder builder, VersionRange range)
        {
            if (range.HasLowerBound
                && range.IsMinInclusive
                && !range.HasUpperBound)
            {
                VersionFormatter.AppendNormalized(builder, range.MinVersion);
            }
            else if (range.HasLowerAndUpperBounds
                     && range.IsMinInclusive
                     && range.IsMaxInclusive
                     && range.MinVersion.Equals(range.MaxVersion))
            {
                // TODO: Does this need a specific version comparison? Does metadata matter?

                builder.Append('[');
                VersionFormatter.AppendNormalized(builder, range.MinVersion);
                builder.Append(']');
            }
            else
            {
                GetNormalizedString(builder, range);
            }
        }

        /// <summary>
        /// Creates a legacy short string that is compatible with NuGet 2.8.3
        /// </summary>
        private static void GetLegacyShortString(StringBuilder builder, VersionRangeBase range)
        {
            if (range.HasLowerBound
                && range.IsMinInclusive
                && !range.HasUpperBound)
            {
                VersionFormatter.AppendNormalized(builder, range.MinVersion);
            }
            else if (range.HasLowerAndUpperBounds
                     && range.IsMinInclusive
                     && range.IsMaxInclusive
                     && range.MinVersion.Equals(range.MaxVersion))
            {
                builder.Append('[');
                VersionFormatter.AppendNormalized(builder, range.MinVersion);
                builder.Append(']');
            }
            else
            {
                GetLegacyString(builder, range);
            }
        }

        /// <summary>
        /// Creates a legacy string that is compatible with NuGet 2.8.3
        /// </summary>
        private static void GetLegacyString(StringBuilder builder, VersionRangeBase range)
        {
            builder.Append(range.HasLowerBound && range.IsMinInclusive ? '[' : '(');

            if (range.HasLowerBound)
            {
                VersionFormatter.AppendNormalized(builder, range.MinVersion);
            }

            builder.Append(", ");

            if (range.HasUpperBound)
            {
                VersionFormatter.AppendNormalized(builder, range.MaxVersion);
            }

            builder.Append(range.HasUpperBound && range.IsMaxInclusive ? ']' : ')');
        }

        /// <summary>
        /// A pretty print representation of the VersionRange.
        /// </summary>
        private static void PrettyPrint(StringBuilder builder, VersionRange range, bool useParentheses)
        {
            if (!range.HasLowerBound
                && !range.HasUpperBound)
            {
                // empty range
                return;
            }

            if (useParentheses)
            {
                builder.Append('(');
            }

            if (range.HasLowerAndUpperBounds
                && range.MaxVersion.Equals(range.MinVersion)
                && range.IsMinInclusive
                && range.IsMaxInclusive)
            {
                // single version
                builder.Append("= ");
                VersionFormatter.AppendNormalized(builder, range.MinVersion);
            }
            else
            {
                // normal case with a lower, upper, or both.
                if (range.HasLowerBound)
                {
                    PrettyPrintBound(builder, range.MinVersion, range.IsMinInclusive, ">");
                }

                if (range.HasLowerAndUpperBounds)
                {
                    builder.Append(" && ");
                }

                if (range.HasUpperBound)
                {
                    PrettyPrintBound(builder, range.MaxVersion, range.IsMaxInclusive, "<");
                }
            }

            if (useParentheses)
            {
                builder.Append(')');
            }
        }

        private static void PrettyPrintBound(StringBuilder builder, NuGetVersion version, bool inclusive, string boundChar)
        {
            builder.Append(boundChar);

            if (inclusive)
            {
                builder.Append("= ");
            }
            else
            {
                builder.Append(' ');
            }

            VersionFormatter.AppendNormalized(builder, version);
        }
    }
}
