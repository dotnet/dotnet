// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.Text.RegularExpressions;
using NuGet.Packaging.PackageCreation.Resources;

namespace NuGet.Packaging
{
    public static class PackageIdValidator
    {
        /// <summary>
        /// Max allowed length for package Id.
        /// In case update this value please update in src\NuGet.Core\NuGet.Configuration\PackageSourceMapping\PackageSourceMapping.cs too.
        /// </summary>
        public const int MaxPackageIdLength = 100;
        private static readonly Regex IdRegex = new Regex(pattern: @"^\w+([.-]\w+)*$",
            options: RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.CultureInvariant,
            matchTimeout: TimeSpan.FromSeconds(10));

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
        /// with no consecutive dots or dashes, and is 100 characters or less.
        /// </summary>
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

        public static void ValidatePackageId(string packageId)
        {
            if (packageId.Length > MaxPackageIdLength)
            {
                throw new ArgumentException(NuGetResources.Manifest_IdMaxLengthExceeded);
            }

            if (!IsValidPackageId(packageId))
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
