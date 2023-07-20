// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;

namespace NuGet.Versioning
{
    /// <summary>
    /// A base version operations
    /// </summary>
    public partial class SemanticVersion : IFormattable, IComparable, IComparable<SemanticVersion>, IEquatable<SemanticVersion>
    {
        /// <summary>
        /// Gives a normalized representation of the version.
        /// This string is unique to the identity of the version and does not contain metadata.
        /// </summary>
        public virtual string ToNormalizedString()
        {
            return ToString("N", VersionFormatter.Instance);
        }

        /// <summary>
        /// Gives a full representation of the version include metadata.
        /// This string is not unique to the identity of the version. Other versions 
        /// that differ on metadata will have a different full string representation.
        /// </summary>
        public virtual string ToFullString()
        {
            return ToString("F", VersionFormatter.Instance);
        }

        /// <summary>
        /// Get the normalized string.
        /// </summary>
        public override string ToString()
        {
            return ToNormalizedString();
        }

        /// <summary>
        /// Custom string format.
        /// </summary>
        public virtual string ToString(string? format, IFormatProvider? formatProvider)
        {
            if (formatProvider == null
                || !TryFormatter(format, formatProvider, out string? formattedString))
            {
                return ToString();
            }

            return formattedString;
        }

        /// <summary>
        /// Internal string formatter.
        /// </summary>
        protected bool TryFormatter(string? format, IFormatProvider formatProvider, [NotNullWhen(true)] out string? formattedString)
        {
            var formatted = false;
            formattedString = null;

            if (formatProvider != null)
            {
                var formatter = formatProvider.GetFormat(GetType()) as ICustomFormatter;
                if (formatter != null)
                {
                    formatted = true;
                    formattedString = formatter.Format(format, this, formatProvider);
                }
            }

            return formatted;
        }

        /// <summary>
        /// Hash code
        /// </summary>
        public override int GetHashCode()
        {
            return VersionComparer.Default.GetHashCode(this);
        }

        /// <summary>
        /// Object compare.
        /// </summary>
        public virtual int CompareTo(object? obj)
        {
            return CompareTo(obj as SemanticVersion);
        }

        /// <summary>
        /// Compare to another SemanticVersion.
        /// </summary>
        public virtual int CompareTo(SemanticVersion? other)
        {
            return CompareTo(other, VersionComparison.Default);
        }

        /// <summary>
        /// Equals
        /// </summary>
        public override bool Equals(object? obj)
        {
            return Equals(obj as SemanticVersion);
        }

        /// <summary>
        /// Equals
        /// </summary>
        public virtual bool Equals(SemanticVersion? other)
        {
#pragma warning disable CS8604 // Possible null reference argument.
            // The BCL is missing nullable annotations on IComparer<T> before net5.0
            return VersionComparer.Default.Equals(this, other);
#pragma warning restore CS8604 // Possible null reference argument.
        }

        /// <summary>
        /// True if the VersionBase objects are equal based on the given comparison mode.
        /// </summary>
        public virtual bool Equals(SemanticVersion? other, VersionComparison versionComparison)
        {
            var comparer = new VersionComparer(versionComparison);
            return comparer.Equals(this, other);
        }

        /// <summary>
        /// Compares NuGetVersion objects using the given comparison mode.
        /// </summary>
        public virtual int CompareTo(SemanticVersion? other, VersionComparison versionComparison)
        {
            var comparer = new VersionComparer(versionComparison);
            return comparer.Compare(this, other);
        }

        /// <summary>
        /// Equals
        /// </summary>
        public static bool operator ==(SemanticVersion? version1, SemanticVersion? version2)
        {
            return Equals(version1, version2);
        }

        /// <summary>
        /// Not equal
        /// </summary>
        public static bool operator !=(SemanticVersion? version1, SemanticVersion? version2)
        {
            return !Equals(version1, version2);
        }

        /// <summary>
        /// Less than
        /// </summary>
        public static bool operator <(SemanticVersion version1, SemanticVersion version2)
        {
            return Compare(version1, version2) < 0;
        }

        /// <summary>
        /// Less than or equal
        /// </summary>
        public static bool operator <=(SemanticVersion version1, SemanticVersion version2)
        {
            return Compare(version1, version2) <= 0;
        }

        /// <summary>
        /// Greater than
        /// </summary>
        public static bool operator >(SemanticVersion version1, SemanticVersion version2)
        {
            return Compare(version1, version2) > 0;
        }

        /// <summary>
        /// Greater than or equal
        /// </summary>
        public static bool operator >=(SemanticVersion version1, SemanticVersion version2)
        {
            return Compare(version1, version2) >= 0;
        }

        private static int Compare(SemanticVersion? version1, SemanticVersion? version2)
        {
#pragma warning disable CS8604 // Possible null reference argument.
            // The BCL is missing nullable annotations in IComparer<T> before net5.0
            return VersionComparer.Default.Compare(version1, version2);
#pragma warning restore CS8604 // Possible null reference argument.
        }
    }
}
