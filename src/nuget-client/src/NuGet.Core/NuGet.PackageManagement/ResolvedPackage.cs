// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.Versioning;

namespace NuGet.PackageManagement
{
    public class ResolvedPackage
    {
        public ResolvedPackage(NuGetVersion latestVersion, bool exists)
        {
            LatestVersion = latestVersion;
            Exists = exists;
        }

        public NuGetVersion LatestVersion { get; }

        public bool Exists { get; }
    }
}
