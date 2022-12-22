// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.PackageManagement.PowerShellCmdlets
{
    internal static class ProgressActivityIds
    {
        // represents the activity Id for the Get-Package command to report its progress
        public const int GetPackageId = 1;

        // represents the activity Id for download progress operation
        public const int DownloadPackageId = 2;
    }
}
