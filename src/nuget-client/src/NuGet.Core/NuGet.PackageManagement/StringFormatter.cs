// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Globalization;

namespace NuGet.PackageManagement
{
    internal static class StringFormatter
    {
        internal static string Log_PackageSourceMappingMatchFound(
            string packageId,
            string packageSourcesAtPrefix)
        {
            return string.Format(
                CultureInfo.CurrentCulture,
                Strings.PackageSourceMappingPatternMatchFound,
                packageId,
                packageSourcesAtPrefix);
        }

        internal static string Log_PackageSourceMappingNoMatchFound(
            string packageId)
        {
            return string.Format(
                CultureInfo.CurrentCulture,
                Strings.PackageSourceMappingPatternNoMatchFound,
                packageId);
        }
    }
}
