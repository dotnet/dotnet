// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Text;

namespace NuGet.Versioning
{
    /// <summary>
    /// Custom formatter for NuGet versions.
    /// </summary>
    public class VersionFormatter : IFormatProvider, ICustomFormatter
    {
        /// <summary>
        /// A static instance of the VersionFormatter class.
        /// </summary>
        public static readonly VersionFormatter Instance = new();

        /// <summary>
        /// Format a version string.
        /// </summary>
        public string Format(string? format, object? arg, IFormatProvider? formatProvider)
        {
            if (arg == null)
            {
                throw new ArgumentNullException(nameof(arg));
            }

            if (arg is string stringValue)
            {
                return stringValue;
            }

            if (arg is not SemanticVersion version)
            {
                throw ResourcesFormatter.TypeNotSupported(arg.GetType(), nameof(arg));
            }

            if (string.IsNullOrEmpty(format))
            {
                format = "N";
            }

            StringBuilder builder = StringBuilderPool.Shared.Rent(256);

            foreach (char c in format!)
            {
                Format(builder, c, version);
            }

            return StringBuilderPool.Shared.ToStringAndReturn(builder);
        }

        /// <summary>
        /// Get version format type.
        /// </summary>
        public object? GetFormat(Type? formatType)
        {
            if (formatType == typeof(ICustomFormatter)
                || formatType == typeof(NuGetVersion)
                || typeof(SemanticVersion).IsAssignableFrom(formatType))
            {
                return this;
            }

            return null;
        }

        private static void Format(StringBuilder builder, char c, SemanticVersion version)
        {
            switch (c)
            {
                case 'N':
                    AppendNormalized(builder, version);
                    return;
                case 'V':
                    AppendVersion(builder, version);
                    return;
                case 'F':
                    AppendFull(builder, version);
                    return;
                case 'R':
                    builder.Append(version.Release);
                    return;
                case 'M':
                    builder.Append(version.Metadata);
                    return;
                case 'x':
                    AppendInt(builder, version.Major);
                    return;
                case 'y':
                    AppendInt(builder, version.Minor);
                    return;
                case 'z':
                    AppendInt(builder, version.Patch);
                    return;
                case 'r':
                    AppendInt(builder, version is NuGetVersion nuGetVersion && nuGetVersion.IsLegacyVersion ? nuGetVersion.Version.Revision : 0);
                    return;

                default:
                    builder.Append(c);
                    return;
            }
        }

        /// <summary>
        /// Appends the full version string including metadata. This is primarily for display purposes.
        /// </summary>
        private static void AppendFull(StringBuilder builder, SemanticVersion version)
        {
            AppendNormalized(builder, version);

            if (version.HasMetadata)
            {
                builder.Append('+');
                builder.Append(version.Metadata);
            }
        }

        /// <summary>
        /// Appends a normalized version string. This string is unique for each version 'identity' 
        /// and does not include leading zeros or metadata.
        /// </summary>
        internal static void AppendNormalized(StringBuilder builder, SemanticVersion version)
        {
            AppendVersion(builder, version);

            if (version.IsPrerelease)
            {
                builder.Append('-');
                builder.Append(version.Release);
            }
        }

        private static void AppendVersion(StringBuilder builder, SemanticVersion version)
        {
            AppendInt(builder, version.Major);
            builder.Append('.');
            AppendInt(builder, version.Minor);
            builder.Append('.');
            AppendInt(builder, version.Patch);

            if (version is NuGetVersion nuGetVersion && nuGetVersion.IsLegacyVersion)
            {
                builder.Append('.');
                AppendInt(builder, nuGetVersion.Version.Revision);
            }
        }

        /// <summary>
        /// Helper function to append an <see cref="int"/> to a <see cref="StringBuilder"/>. Calling
        /// <see cref="StringBuilder.Append(int)"/> directly causes an allocation by first converting the
        /// <see cref="int"/> to a string and then appending that result:
        /// <code>
        /// public StringBuilder Append(int value)
        /// {
        ///     return Append(value.ToString(CultureInfo.CurrentCulture));
        /// }
        /// </code>
        ///
        /// Note that this uses the current culture to do the conversion while <see cref="AppendInt(StringBuilder, int)"/> does
        /// not do any cultural sensitive conversion.
        /// </summary>
        /// <param name="sb">The <see cref="StringBuilder"/> to append to.</param>
        /// <param name="value">The <see cref="int"/> to append.</param>
        private static void AppendInt(StringBuilder sb, int value)
        {
            if (value == 0)
            {
                sb.Append('0');
                return;
            }

            // special case min value since it'll overflow if we negate it
            if (value == int.MinValue)
            {
                sb.Append("-2147483648");
                return;
            }

            // do all math with positive integers
            if (value < 0)
            {
                sb.Append('-');
                value = -value;
            }

            // upper range of int is 1 billion, so we start dividing by that to get the digit at that position
            int divisor = 1_000_000_000;

            // remember when we found our first digit so we can keep adding intermediate zeroes
            bool digitFound = false;
            while (divisor > 0)
            {
                if (digitFound || value >= divisor)
                {
                    digitFound = true;
                    int digit = value / divisor;
                    value -= digit * divisor;

                    // convert the digit to char by adding the value to '0'.
                    // '0' + 0 = 48 + 0 = 48 = '0'
                    // '0' + 1 = 48 + 1 = 49 = '1'
                    // '0' + 2 = 48 + 2 = 50 = '2'
                    // etc...
                    sb.Append((char)('0' + digit));
                }

                divisor /= 10;
            }
        }
    }
}
