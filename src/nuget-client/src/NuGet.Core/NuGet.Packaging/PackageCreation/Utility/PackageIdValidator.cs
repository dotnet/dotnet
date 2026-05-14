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
    }
}
