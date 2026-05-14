// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.VisualStudio.Implementation.Extensibility
{
    public class VsNuGetFramework : IVsNuGetFramework
    {
        public VsNuGetFramework(
            string targetFrameworkMoniker,
            string targetPlatformMoniker,
            string targetPlatformMinVersion)
        {
            TargetFrameworkMoniker = targetFrameworkMoniker;
            TargetPlatformMoniker = targetPlatformMoniker;
            TargetPlatformMinVersion = targetPlatformMinVersion;
        }

        public string TargetFrameworkMoniker { get; }

        public string TargetPlatformMoniker { get; }

        public string TargetPlatformMinVersion { get; }
    }
}
