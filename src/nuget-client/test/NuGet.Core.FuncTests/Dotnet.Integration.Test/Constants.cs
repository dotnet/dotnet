// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Frameworks;

namespace Dotnet.Integration.Test
{
    internal static class Constants
    {
        internal static readonly NuGetFramework DefaultTargetFramework = FrameworkConstants.CommonFrameworks.Net70;

        internal static readonly Uri DotNetPackageSource = new("https://dnceng.pkgs.visualstudio.com/public/_packaging/dotnet7/nuget/v3/index.json");
    }
}
