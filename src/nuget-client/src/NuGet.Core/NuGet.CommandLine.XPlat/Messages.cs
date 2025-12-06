// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System.Globalization;

namespace NuGet.CommandLine.XPlat
{
    internal static class Messages
    {
        /// <inheritdoc cref="Strings.Error_NoVersionsAvailable"/>
        internal static string Error_NoVersionsAvailable(string packageId)
        {
            return string.Format(CultureInfo.CurrentCulture, Strings.Error_NoVersionsAvailable, packageId);
        }

        /// <inheritdoc cref="Strings.Error_CouldNotFindPackageVersionForCpmPackage"/>
        internal static string Error_CouldNotFindPackageVersionForCpmPackage(string packageId)
        {
            return string.Format(CultureInfo.CurrentCulture, Strings.Error_CouldNotFindPackageVersionForCpmPackage, packageId);
        }

        /// <inheritdoc cref="Strings.Unsupported_UpdatePackageWithDifferentPerTfmVersions"/>
        internal static string Unsupported_UpdatePackageWithDifferentPerTfmVersions(string packageId, string projectPath)
        {
            return string.Format(CultureInfo.CurrentCulture, Strings.Unsupported_UpdatePackageWithDifferentPerTfmVersions, packageId, projectPath);
        }

        /// <inheritdoc cref="Strings.Warning_AlreadyHighestVersion"/>
        internal static string Warning_AlreadyHighestVersion(string packageId, string version, string projectPath)
        {
            return string.Format(CultureInfo.CurrentCulture, Strings.Warning_AlreadyHighestVersion, packageId, version, projectPath);
        }

        /// <inheritdoc cref="Strings.Warning_AlreadyUsingSameVersion"/>
        internal static string Warning_AlreadyUsingSameVersion(string packageId, string version)
        {
            return string.Format(CultureInfo.CurrentCulture, Strings.Warning_AlreadyUsingSameVersion, packageId, version);
        }

        /// <inheritdoc cref="Strings.Error_MissingVersion"/>
        internal static string Error_MissingVersion(string packageId)
        {
            return string.Format(CultureInfo.CurrentCulture, Strings.Error_MissingVersion, packageId);
        }

        /// <inheritdoc cref="Strings.Error_InvalidVersionRange"/>
        internal static string Error_InvalidVersionRange(string input)
        {
            return string.Format(CultureInfo.CurrentCulture, Strings.Error_InvalidVersionRange, input);
        }

        /// <inheritdoc cref="Strings.Error_InvalidVersion"/>
        internal static string Error_InvalidVersion(string input)
        {
            return string.Format(CultureInfo.CurrentCulture, Strings.Error_InvalidVersion, input);
        }

        /// <inheritdoc cref="Strings.Error_PackageSourceMappingNotFound"/>
        internal static string Error_PackageSourceMappingNotFound(string packageId)
        {
            return string.Format(CultureInfo.CurrentCulture, Strings.Error_PackageSourceMappingNotFound, packageId);
        }

        /// <inheritdoc cref="Strings.Error_CannotUpgradeAutoReferencedPackage"/>
        internal static string Error_CannotUpgradeAutoReferencedPackage(string project, string packageId)
        {
            return string.Format(CultureInfo.CurrentCulture, Strings.Error_CannotUpgradeAutoReferencedPackage, project, packageId);
        }
    }
}
