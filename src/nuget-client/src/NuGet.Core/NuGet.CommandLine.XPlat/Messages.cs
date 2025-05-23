// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Globalization;

namespace NuGet.CommandLine.XPlat
{
    internal static class Messages
    {
        internal static string Error_NoVersionsAvailable(string packageId)
        {
            return string.Format(CultureInfo.CurrentCulture, Strings.Error_NoVersionsAvailable, packageId);
        }

        internal static string Error_CouldNotFindPackageVersionForCpmPackage(string packageId)
        {
            return string.Format(CultureInfo.CurrentCulture, Strings.Error_CouldNotFindPackageVersionForCpmPackage, packageId);
        }

        internal static string Unsupported_UpdatePackageWithDifferentPerTfmVersions(string packageId, string projectPath)
        {
            return string.Format(CultureInfo.CurrentCulture, Strings.Unsupported_UpdatePackageWithDifferentPerTfmVersions, packageId, projectPath);
        }

        internal static string Error_PackageNotReferenced(string packageId, string projectPath)
        {
            return string.Format(CultureInfo.CurrentCulture, Strings.Error_PackageNotReferenced, packageId, projectPath);
        }

        internal static string Warning_AlreadyHighestVersion(string packageId, string version, string projectPath)
        {
            return string.Format(CultureInfo.CurrentCulture, Strings.Warning_AlreadyHighestVersion, packageId, version, projectPath);
        }
    }
}
