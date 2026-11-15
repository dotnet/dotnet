// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.Text.RegularExpressions;
using NuGet.Packaging.PackageCreation.Resources;

namespace NuGet.Packaging
{
    /// <summary>
    /// Validates the format of package IDs.
    /// </summary>
    /// <remarks>
    /// The unrestricted format checks do not enforce a length limit, so existing packages with longer IDs can be read.
    /// The restricted character set checks enforce <see cref="MaxPackageIdLength"/>.
    /// The obsolete <see cref="ValidatePackageId(string)"/> overload also enforces that limit without applying
    /// the restricted character set. Pack enforces the limit when creating packages.
    /// </remarks>
    public static class PackageIdValidator
    {
        /// <summary>
        /// Maximum package ID length enforced by pack, restricted validation, and the obsolete
        /// <see cref="ValidatePackageId(string)"/> overload.
        /// If this value is updated, also update it in src\NuGet.Core\NuGet.Configuration\PackageSourceMapping\PackageSourceMapping.cs.
        /// </summary>
        public const int MaxPackageIdLength = 100;
        private static readonly Regex IdRegex = new Regex(pattern: @"^\w+([.-]\w+)*\z",
            options: RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.CultureInvariant,
            matchTimeout: TimeSpan.FromSeconds(10));

        /// <summary>
        /// Checks whether the package ID has a valid format. The package ID's length is not validated, for backwards compatibility.
        /// </summary>
        /// <param name="packageId">The package ID to check.</param>
        /// <returns><c>true</c> if the package ID has a valid format; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="packageId"/> is <c>null</c>.</exception>
        public static bool IsValidPackageId(string packageId)
        {
            if (packageId == null)
            {
                throw new ArgumentNullException(nameof(packageId));
            }
            return IdRegex.IsMatch(packageId);
        }

        /// <summary>
        /// Validates the package ID. When <paramref name="useRestrictedCharacterSet"/> is <c>true</c>,
        /// additionally checks that the ID contains only ASCII letters, digits, dots, dashes, and underscores,
        /// with no consecutive dots or dashes, and is <see cref="MaxPackageIdLength"/> characters or less.
        /// When <paramref name="useRestrictedCharacterSet"/> is <c>false</c>, the package ID's length is not validated, for backwards compatibility.
        /// </summary>
        /// <param name="packageId">The package ID to check.</param>
        /// <param name="useRestrictedCharacterSet">Whether to also apply the restricted character set and length checks.</param>
        /// <returns><c>true</c> if the package ID is valid; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="packageId"/> is <c>null</c>.</exception>
        public static bool IsValidPackageId(string packageId, bool useRestrictedCharacterSet)
        {
            if (!IsValidPackageId(packageId))
            {
                return false;
            }

            if (useRestrictedCharacterSet)
            {
                return IsRestrictedId(packageId);
            }

            return true;
        }

        /// <summary>
        /// Validates the package ID's format and checks that its length does not exceed <see cref="MaxPackageIdLength"/>.
        /// </summary>
        /// <remarks>This overload does not apply the restricted character set.</remarks>
        /// <param name="packageId">The package ID to validate.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="packageId"/> does not have a valid format or exceeds <see cref="MaxPackageIdLength"/>.
        /// </exception>
        /// <exception cref="NullReferenceException"><paramref name="packageId"/> is <c>null</c>.</exception>
        [Obsolete("Use the ValidatePackageId overload that specifies whether to use the restricted character set.")]
        public static void ValidatePackageId(string packageId)
        {
            if (packageId.Length > MaxPackageIdLength)
            {
                throw new ArgumentException(NuGetResources.Manifest_IdMaxLengthExceeded);
            }

            ValidatePackageId(packageId, useRestrictedSet: false);
        }

        /// <summary>
        /// Validates the package ID using the specified character set.
        /// </summary>
        /// <remarks>
        /// When <paramref name="useRestrictedSet"/> is <c>true</c>, the restricted character set and
        /// <see cref="MaxPackageIdLength"/> are enforced. Otherwise, the package ID's length is not validated.
        /// See <see cref="IsValidPackageId(string, bool)"/> for the restricted character set rules.
        /// </remarks>
        /// <param name="packageId">The package ID to validate.</param>
        /// <param name="useRestrictedSet">Whether to apply the restricted character set and length limit.</param>
        /// <exception cref="ArgumentNullException"><paramref name="packageId"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="packageId"/> does not have a valid format or exceeds the length limit when it is enforced.
        /// </exception>
        public static void ValidatePackageId(string packageId, bool useRestrictedSet)
        {
            if (packageId == null)
            {
                throw new ArgumentNullException(nameof(packageId));
            }

            if (useRestrictedSet && packageId.Length > MaxPackageIdLength)
            {
                throw new ArgumentException(NuGetResources.Manifest_IdMaxLengthExceeded);
            }

            if (!IsValidPackageId(packageId, useRestrictedSet))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, NuGetResources.InvalidPackageId, packageId));
            }
        }

        private static bool IsRestrictedId(string packageId)
        {
            static bool IsAsciiLetterOrDigit(char c)
            {
                return (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || (c >= '0' && c <= '9');
            }

            if (packageId.Length == 0 || packageId.Length > MaxPackageIdLength)
            {
                return false;
            }

            char first = packageId[0];
            if (!IsAsciiLetterOrDigit(first) && first != '_')
            {
                return false;
            }

            char prev = first;
            for (int i = 1; i < packageId.Length; i++)
            {
                char c = packageId[i];
                if (!IsAsciiLetterOrDigit(c) && c != '.' && c != '-' && c != '_')
                {
                    return false;
                }

                if ((c == '.' || c == '-') && (prev == '.' || prev == '-'))
                {
                    return false;
                }

                prev = c;
            }

            return true;
        }
    }
}
