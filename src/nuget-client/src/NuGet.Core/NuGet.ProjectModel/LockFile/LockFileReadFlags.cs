// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.ProjectModel
{
    [Flags]
    public enum LockFileReadFlags
    {
        Libraries = 1 << 0,
        Targets = 1 << 1,
        ProjectFileDependencyGroups = 1 << 2,
        PackageFolders = 1 << 3,
        PackageSpec = 1 << 4,
        CentralTransitiveDependencyGroups = 1 << 5,
        LogMessages = 1 << 6,

        // Update if any values are added after LogMessage
        All = (LogMessages << 1) - 1,
    }
}
