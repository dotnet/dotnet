// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.PackageManagement.UI
{
    public enum PackageStatus
    {
        NotInstalled,

        // the latest applicable version is installed.
        Installed,

        UpdateAvailable,

        // The package is installed but may not be managed.
        AutoReferenced
    }
}
