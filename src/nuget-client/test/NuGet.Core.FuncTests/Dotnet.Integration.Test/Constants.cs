// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Frameworks;

namespace Dotnet.Integration.Test
{
    internal static class Constants
    {
#if NET8_0
        internal static readonly NuGetFramework DefaultTargetFramework = new NuGetFramework(FrameworkConstants.FrameworkIdentifiers.NetCoreApp, new Version(8, 0, 0, 0));
#elif NET7_0
        internal static readonly NuGetFramework DefaultTargetFramework = FrameworkConstants.CommonFrameworks.Net70;
#else
        // Unknown target framework, update this list to support it
#endif

        internal static readonly Uri DotNetPackageSource = new("https://dnceng.pkgs.visualstudio.com/public/_packaging/dotnet7/nuget/v3/index.json");
    }
}
