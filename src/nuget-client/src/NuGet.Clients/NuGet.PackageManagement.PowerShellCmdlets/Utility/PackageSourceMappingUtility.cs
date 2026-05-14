// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.Configuration;

namespace NuGet.PackageManagement.PowerShellCmdlets
{
    internal static class PackageSourceMappingUtility
    {
        public static bool IsMappingEnabled(ISettings settings)
        {
            var packageSourceMapping = PackageSourceMapping.GetPackageSourceMapping(settings);
            bool isMappingEnabled = packageSourceMapping?.IsEnabled ?? false;
            return isMappingEnabled;
        }
    }
}
