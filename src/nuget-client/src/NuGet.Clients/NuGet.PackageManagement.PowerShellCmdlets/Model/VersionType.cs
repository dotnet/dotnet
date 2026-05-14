// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.PackageManagement.PowerShellCmdlets
{
    /// <summary>
    /// Enum for types of version to output, which can be all versions, latest version or update versions.
    /// </summary>
    public enum VersionType
    {
        All,
        Latest,
        Updates,
    }
}
