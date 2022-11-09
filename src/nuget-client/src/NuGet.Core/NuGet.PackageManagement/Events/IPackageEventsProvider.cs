// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.ProjectManagement
{
    /// <summary>
    /// Internal version of the public IVsPackageInstallerEvents
    /// </summary>
    public interface IPackageEventsProvider
    {
        PackageEvents GetPackageEvents();
    }
}
