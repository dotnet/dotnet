// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.PackageManagement
{
    [Flags]
    public enum VersionConstraints
    {
        None = 0,
        ExactMajor = 1,
        ExactMinor = 2,
        ExactPatch = 4,
        ExactRelease = 8
    }
}
